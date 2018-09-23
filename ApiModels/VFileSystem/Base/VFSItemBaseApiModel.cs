using ApiModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModels.VFileSystem.Base {
    public abstract class VFSItemBaseApiModel: BaseApiModel {

        public bool Leaf { get; set; }
    }
}
