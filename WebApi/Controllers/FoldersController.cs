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
    public class FoldersController : ControllerBase
    {
        private readonly IAssetService _assetService;

        private readonly IFolderService _folderService;

        private readonly IMapper _mapper;

        private readonly ContextPrincipal _principal;

        public FoldersController(
            IFolderService folderService,
            IAssetService assetService, 
            IMapper mapper
        ) {
            _folderService = folderService;
            _assetService = assetService;
            _mapper = mapper;
            _principal = new ContextPrincipal(HttpContext.User);
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetById(Guid id) {

            if (id.Equals(Guid.Empty)) {
                return BadRequest(new ErrorResponse().AddError(EErrorCodes.GeneralGetErrorCode, "Invalid Id."));
            }

            return Ok(await _folderService.GetAsBiz(_principal, id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] FolderBizModel value) {

            if (!ModelState.IsValid) {
                return BadRequest(new ErrorResponse()
                    .AddModelStateErrors(ModelState));
            }

            bool success = await _folderService.Update(_principal, _mapper.Map<VFileSystemItem>(value), id);

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

            await _folderService.SoftDelete(_principal, id);

            return NoContent();
        }


        [HttpGet("{id}/folders")]
        public async Task<IActionResult> Get(
            Guid id,
            [FromQuery]int start, 
            [FromQuery]int limit) 
        {

            IEnumerable<Guid> childrenGuids = await _folderService.GetDirectChildrenAsGuids(_principal, id, start, limit);

            IEnumerable<FolderBizModel> bizModels = await _folderService.FindAsBiz(_principal, u => childrenGuids.Contains(u.Id), start, limit);

            return Ok(bizModels);
        }

        [HttpPost("{id}/folders")]
        public async Task<IActionResult> Post(Guid id, [FromBody] FolderBizModel value) {
            if (!ModelState.IsValid) {
                return BadRequest(new ErrorResponse()
                    .AddModelStateErrors(ModelState));
            }

            value.Id = await _folderService.Create(_principal,
                    _mapper.Map<VFileSystemItem>(value), id);

            return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
        }

        [HttpGet("{id}/assets")]
        public async Task<IActionResult> GetAssets(
            Guid id,
            [FromQuery]int start,
            [FromQuery]int limit) {

            IEnumerable<Guid> childrenGuids = await _folderService.GetDirectChildrenAsGuids(_principal, id, start, limit);

            IEnumerable<AssetBizModel> bizModels = await _assetService.FindAsBiz(_principal, u => childrenGuids.Contains(u.Id), start, limit);

            return Ok(bizModels);
        }

        [HttpPost("{id}/assets")]
        public async Task<IActionResult> PostAsset(Guid id, [FromBody] AssetBizModel value) {
            if (!ModelState.IsValid) {
                return BadRequest(new ErrorResponse()
                    .AddModelStateErrors(ModelState));
            }

            value.Id = await _assetService.Create(_principal,
                    _mapper.Map<VFileSystemItem>(value), id);

            return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
        }
    }
}
