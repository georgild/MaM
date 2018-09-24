using BizModels.VFileSystem;
using BusinessServicesContracts.Base;
using Models.VFileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessServicesContracts.VFileSystem {
    public interface IFileService : IBaseEntityService<VFileSystemItem, FileBizModel> {
    }
}
