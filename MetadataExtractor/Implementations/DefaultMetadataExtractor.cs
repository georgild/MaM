using MetadataExtractor.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetadataExtractor.Implementations {
    public class DefaultMetadataExtractor : IMetadataExtractor {
        public Dictionary<string, string> DumpFlat(string fileLocation) {

            using (XmpReader reader = XmpReader.InitFromMediaFile(fileLocation)) {
                return reader.DumpFlat();
            }
        }
    }
}
