using MetadataExtractor.Contracts;
using Orleans;
using System;
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

            throw new NotImplementedException();
        }
    }
}
