using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Transcoder.Contracts;

namespace Actors {
    public class TranscoderActor : Orleans.Grain {

        private readonly ITranscoder _transcoder;

        public TranscoderActor(ITranscoder transcoder) {
            _transcoder = transcoder;
        }

        public Task<Stream> ExtractImage(string source, uint width, uint height, CancellationToken cancellationToken) {
            return _transcoder.ExtractImage(source, width, height, cancellationToken);
        }

        public double GetProgress() {
            throw new NotImplementedException();
        }

        public Task<bool> Transcode(string source, string destination, string format, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
