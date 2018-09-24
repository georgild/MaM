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
using Models.Tasks;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        private readonly IMapper _mapper;

        private readonly ContextPrincipal _principal;

        public TasksController(
            ITaskService taskService,
            IMapper mapper
        ) {
            _taskService = taskService;
            _mapper = mapper;
            _principal = new ContextPrincipal(HttpContext.User);
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetById(Guid id) {

            if (id.Equals(Guid.Empty)) {
                return BadRequest(new ErrorResponse().AddError(EErrorCodes.GeneralGetErrorCode, "Invalid Id."));
            }

            return Ok(await _taskService.GetAsBiz(_principal, id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] TaskBizModel value) {

            if (!ModelState.IsValid) {
                return BadRequest(new ErrorResponse()
                    .AddModelStateErrors(ModelState));
            }

            bool success = await _taskService.Update(_principal, _mapper.Map<TaskModel>(value), id);

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

            await _taskService.SoftDelete(_principal, id);

            return NoContent();
        }
    }
}
