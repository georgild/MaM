using ApiModels.VFileSystem.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModels.VFileSystem {
    public class FolderApiModel : VFSItemBaseApiModel {

        public FolderApiModel() {
            Leaf = false;
        }
    }
}
