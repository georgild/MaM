using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizModels.Errors;
using BizModels.Identity;
using AutoMapper;
using BusinessServicesContracts.Identity;
using BusinessServicesContracts.VFileSystem;
using CustomPolicyAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using BizModels.VFileSystem;
using Models.VFileSystem;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;

        private readonly IFileService _fileService;

        private readonly IMapper _mapper;

        private readonly ContextPrincipal _principal;

        public AssetsController(
            IAssetService assetService,
            IFileService fileService, 
            IMapper mapper
        ) {
            _assetService = assetService;
            _fileService = fileService;
            _mapper = mapper;
            _principal = new ContextPrincipal(HttpContext.User);
        }

        [HttpGet("{id}", Name = "GetAssetById")]
        public async Task<IActionResult> GetAssetById(Guid id) {

            if (id.Equals(Guid.Empty)) {
                return BadRequest(new ErrorResponse().AddError(EErrorCodes.GeneralGetErrorCode, "Invalid Id."));
            }

            return Ok(await _assetService.GetAsBiz(_principal, id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] FolderBizModel value) {

            if (!ModelState.IsValid) {
                return BadRequest(new ErrorResponse()
                    .AddModelStateErrors(ModelState));
            }

            bool success = await _assetService.Update(_principal, _mapper.Map<VFileSystemItem>(value), id);

            if (!success) {
                return BadRequest(new ErrorResponse().AddError(EErrorCodes.GeneralUpdateErrorCode,
                    "Update didn't affect any items."));
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {

            if (id.Equals(Guid.Empty)) {
                return BadRequest(new ErrorResponse().AddError(EErrorCodes.GeneralGetErrorCode, "Invalid Id."));
            }

            await _assetService.SoftDelete(_principal, id);

            return NoContent();
        }


        [HttpGet("{id}/files")]
        public async Task<IActionResult> Get(
            Guid id,
            [FromQuery]int start, 
            [FromQuery]int limit) 
        {

            IEnumerable<Guid> childrenGuids = await _assetService.GetDirectChildrenAsGuids(_principal, id, start, limit);

            IEnumerable<FileBizModel> bizModels = await _fileService.FindAsBiz(_principal, u => childrenGuids.Contains(u.Id), start, limit);

            return Ok(bizModels);
        }

        [HttpPost("{id}/files")]
        public async Task<IActionResult> Post(Guid id, [FromBody] FileBizModel value) {
            if (!ModelState.IsValid) {
                return BadRequest(new ErrorResponse()
                    .AddModelStateErrors(ModelState));
            }

            value.Id = await _fileService.Create(_principal,
                    _mapper.Map<VFileSystemItem>(value), id);

            return CreatedAtAction(nameof(GetAssetById), new { id = value.Id }, value);
        }
    }
}
