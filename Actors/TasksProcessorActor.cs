using MetadataExtractor.Contracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Transcoder.Contracts;

namespace Actors {

    public class TasksProcessorActor : Grain, ITasksProcessorActor {

        private readonly ITranscoder _transcoder;

        private readonly IMetadataExtractor _metadataExtractor;

        private static Queue<TranscoderActorTask> transcodingTasksQueue = new Queue<TranscoderActorTask>();

        private static Queue<MetadataActorTask> metadataTasksQueue = new Queue<MetadataActorTask>();

        private static Queue<QualityControlActorTask> qualityTasksQueue = new Queue<QualityControlActorTask>();

        public TasksProcessorActor(ITranscoder transcoder, IMetadataExtractor metadataExtractor) {
            _transcoder = transcoder;
            _metadataExtractor = metadataExtractor;
            Task.Run(() => {
                ExecuteTasks();
            });
        }

        public void ExecuteTasks() {

            while (true) {
                if (transcodingTasksQueue.Count > 0) {

                    TranscoderActorTask task = transcodingTasksQueue.Dequeue();

                    Task.Run(() => {
                        _transcoder.Transcode(task.Source, task.Destination, task.Format);
                    });
                }
                if (metadataTasksQueue.Count > 0) {

                    MetadataActorTask task = metadataTasksQueue.Dequeue();

                    Task.Run(() => {
                        Dictionary<string, string> mtd = _metadataExtractor.DumpFlat(task.Source);
                    });
                }
                if (qualityTasksQueue.Count > 0) {

                    QualityControlActorTask task = qualityTasksQueue.Dequeue();

                    Task.Run(() => {
                        _transcoder.QualityCheck(task.Source);
                    });
                }
                Thread.Sleep(2000);
            }
        }

        public Task<bool> ScheduleTranscode(string source, string destination, string format) {

            return Task.Run(() => {
                transcodingTasksQueue.Enqueue(new TranscoderActorTask {
                    Source = source,
                    Destination = destination,
                    Format = format
                });
                return true;
            });
        }

        public Task<bool> ScheduleMetadata(string source) {

            return Task.Run(() => {
                metadataTasksQueue.Enqueue(new MetadataActorTask {
                    Source = source
                });

                return true;
            });

        }

        public Task<bool> ScheduleQualityControl(string source) {

            return Task.Run(() => {
                qualityTasksQueue.Enqueue(new QualityControlActorTask {
                    Source = source
                });

                return true;
            });

        }

        public async Task<Dictionary<string, string>> GetMetadata(string source) {

            return await Task.Run(() => {
                return _metadataExtractor.DumpFlat(source);
            });
        }

        public async Task<string> QualityCheck(string source) {

            return await Task.Run(() => {
                return _transcoder.QualityCheck(source);
            });
        }
    }
}
