using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Transcoder.Contracts {
    public interface ITranscoder {

        double GetProgress();

        ETranscoderState GetState();

        Task<Stream> ExtractImage(string source, uint width, uint height, CancellationToken cancellationToken);

        bool Transcode(string source, string destination, string format);

        string QualityCheck(string source);
    }
}
