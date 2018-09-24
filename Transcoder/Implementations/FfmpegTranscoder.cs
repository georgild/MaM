using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Transcoder.Contracts;

namespace Transcoder.Implementations {
    public class FFMPEGTranscoder : ITranscoder {
        public Task<Stream> ExtractImage(string source, uint width, uint height, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public double GetProgress() {
            throw new NotImplementedException();
        }

        public ETranscoderState GetState() {
            throw new NotImplementedException();
        }

        public Task<bool> Transcode(string source, string destination, string format, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
