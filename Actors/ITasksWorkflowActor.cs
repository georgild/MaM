using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Actors {
    public interface ITasksWorkflowActor: IGrainWithGuidKey {

        Task StartTaskChain(string fileLocation);
    }
}
