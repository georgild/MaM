using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actors;
using Microsoft.AspNetCore.Mvc;
using Orleans;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers {
    [Route("api/v1/[controller]")]

    public class UploadsController : ControllerBase {

        private readonly IClusterClient _client;
        public UploadsController(IClusterClient client) {
            _client = client;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post() {
            ITasksProcessorActor tasksWorkflow = _client.GetGrain<ITasksProcessorActor>(Guid.Empty);
            await tasksWorkflow.QualityCheck(@"D:\Pics\VIDEO0002.mp4");

            throw new NotImplementedException();
        }
    }
}
