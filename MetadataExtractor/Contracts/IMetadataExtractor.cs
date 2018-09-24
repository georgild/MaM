using System;
using System.Collections.Generic;
using System.Text;

namespace MetadataExtractor.Contracts {
    public interface IMetadataExtractor {

        Dictionary<string, string> DumpFlat();
    }
}
