using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Tasks {
    public enum ETaskType {
        Unknown = 0,
        Upload = 1,
        Metadata = 2,
        Transcode = 3,
        QualityControl = 4
    }
}
