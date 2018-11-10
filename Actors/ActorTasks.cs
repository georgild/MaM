using System;
using System.Collections.Generic;
using System.Text;

namespace Actors {

    public class TranscoderActorTask {
        public string Source { get; set; }

        public string Destination { get; set; }

        public string Format { get; set; }
    }

    public class MetadataActorTask {

        public string Source { get; set; }
    }

    public class QualityControlActorTask {

        public string Source { get; set; }
    }
}
