using BizModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizModels.VFileSystem.Base {
    public abstract class VFSItemBaseApiModel: BaseEntityBizModel {

        public bool Leaf { get; set; }
    }
}
