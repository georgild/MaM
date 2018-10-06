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
using BusinessServicesContracts.Tasks;
using BizModels.Tasks;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ITaskService _taskService;

        private readonly IFileService _fileService;

        private readonly IMapper _mapper;

        private readonly ContextPrincipal _principal;

        public FilesController(
            ITaskService taskService,
            IFileService fileService, 
            IMapper mapper
        ) {
            _taskService = taskService;
            _fileService = fileService;
            _mapper = mapper;
            _principal = new ContextPrincipal(HttpContext.User);
        }

        [HttpGet("{id}", Name = "GetFileById")]
        public async Task<IActionResult> GetFileById(Guid id) {

            if (id.Equals(Guid.Empty)) {
                return BadRequest(new ErrorResponse().AddError(EErrorCodes.GeneralGetErrorCode, "Invalid Id."));
            }

            return Ok(await _fileService.GetAsBiz(_principal, id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] FileBizModel value) {

            if (!ModelState.IsValid) {
                return BadRequest(new ErrorResponse()
                    .AddModelStateErrors(ModelState));
            }

            bool success = await _fileService.Update(_principal, _mapper.Map<VFileSystemItem>(value), id);

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

            await _fileService.SoftDelete(_principal, id);

            return NoContent();
        }


        [HttpGet("{id}/tasks")]
        public async Task<IActionResult> Get(
            Guid id,
            [FromQuery]int start, 
            [FromQuery]int limit) 
        {

            IEnumerable<Guid> childrenGuids = await _fileService.GetDirectChildrenAsGuids(_principal, id, start, limit);

            IEnumerable<TaskBizModel> bizModels = await _taskService.FindAsBiz(_principal, u => childrenGuids.Contains(u.Id), start, limit);

            return Ok(bizModels);
        }

        [HttpPost("{id}/tasks")]
        public async Task<IActionResult> Post(Guid id, [FromBody] FileBizModel value) {
            if (!ModelState.IsValid) {
                return BadRequest(new ErrorResponse()
                    .AddModelStateErrors(ModelState));
            }

            value.Id = await _fileService.Create(_principal,
                    _mapper.Map<VFileSystemItem>(value), id);

            return CreatedAtAction(nameof(GetFileById), new { id = value.Id }, value);
        }
    }
}
