using MetadataExtractor.Contracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transcoder.Contracts;

namespace Actors {
    public class TasksWorkflowActor : Grain, ITasksWorkflowActor {

        private readonly ITranscoder _transcoder;

        private readonly IMetadataExtractor _metadataExtractor;

        public TasksWorkflowActor(ITranscoder transcoder, IMetadataExtractor metadataExtractor) {
            _transcoder = transcoder;
            _metadataExtractor = metadataExtractor;
        }

        public async Task StartTaskChain(string fileLocation) {

            await Task.Run(() => {
                _transcoder.Transcode(@"D:\Pics\Телефон 24.07.2018\VID_20180505_190712.mp4",
                @"D:\Pics\Телефон 24.07.2018\VID_20180505_190712.mov", "mov");
            });
            // Dictionary<string, string> res = _metadataExtractor.DumpFlat(@"D:\Pics\VIDEO0002.mp4");
        }
    }
}
