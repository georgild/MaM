using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Actors {
    public interface ITasksProcessorActor: IGrainWithGuidKey {

        //Task StartTaskChain(string fileLocation);

        Task<bool> ScheduleTranscode(string source, string destination, string format);

        Task<bool> ScheduleMetadata(string source);

        Task<bool> ScheduleQualityControl(string source);

        Task<Dictionary<string, string>> GetMetadata(string source);

        Task<string> QualityCheck(string source);
    }
}
