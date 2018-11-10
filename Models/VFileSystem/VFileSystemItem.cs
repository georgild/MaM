using Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.VFileSystem {
    public class VFileSystemItem : Entity {

        public EVItemType Type { get; set; }

        public string Location { get; set; }
    }
}
