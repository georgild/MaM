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
using Orleans;
using Actors;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ITaskService _taskService;

        private readonly IFileService _fileService;

        private readonly IClusterClient _orleansClient;

        private readonly IMapper _mapper;

        private readonly ContextPrincipal _principal;

        private readonly List<VFileSystemItem> _mockData = new List<VFileSystemItem> {
            new VFileSystemItem {
                Id = Guid.Parse("0ce860a4-1b84-4910-957d-764e7ea80956"),
                Location = @"D:\Pics\VIDEO0002.mp4",
                Title = "File 1"
            }
        };

        public FilesController(
            ITaskService taskService,
            IFileService fileService, 
            IMapper mapper,
            IClusterClient orleansClient
        ) {
            _taskService = taskService;
            _fileService = fileService;
            _mapper = mapper;
            _orleansClient = orleansClient;
            _principal = new ContextPrincipal();
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

        [HttpPost("{id}/transcodertasks")]
        public async Task<IActionResult> Post(Guid id, [FromBody] TranscoderTaskBizModel value) {
            if (!ModelState.IsValid) {
                return BadRequest(new ErrorResponse()
                    .AddModelStateErrors(ModelState));
            }

            VFileSystemItem file =   _mockData[0]; //await _fileService.Get(_principal, id); //
            string location = ConfigUtils.ConfigurationProvider
                    .GetDefaultConfig()
                    .GetSection("Storage").GetValue<string>("TranscoderLocation");
            //value.Id = await _taskService.Create(_principal,
            //        _mapper.Map<TaskModel>(value), id);

            ITasksProcessorActor tasksProcessor = _orleansClient.GetGrain<ITasksProcessorActor>(Guid.Empty);

            string transcoded = Path.Combine(location, file.Id.ToString());

            //bool succeeded = await tasksProcessor.ScheduleTranscode(file.Location, transcoded + "." + value.Format, value.Format);
            await tasksProcessor.ScheduleMetadata(file.Location);
            return CreatedAtAction(nameof(GetFileById), new { id = value.Id }, value);
        }
    }
}
