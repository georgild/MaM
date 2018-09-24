using System;
using System.Collections.Generic;
using System.Text;

namespace Transcoder.Contracts {
    public enum ETranscoderState {
        Unknown = 0,
        Pending = 1,
        InProgress = 2,
        SuccessfulyFinished = 3,
        Failed = 4
    }
}
