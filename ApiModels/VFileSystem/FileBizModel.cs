﻿using BizModels.VFileSystem.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizModels.VFileSystem {
    public class FileBizModel : VFSItemBaseApiModel {

        public FileBizModel() {
            Leaf = false;
        }
    }
}
