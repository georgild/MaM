using BusinessServicesContracts.VFileSystem;
using BusinessServicesImpl.Base;
using Models.VFileSystem;
using RepositoryContracts.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessServicesImpl.VFileSystem {
    public class FileServiceImpl : BaseEntityServiceImpl<VFileSystemItem>, IFileService {

        public FileServiceImpl(
                IBaseEntityUnitOfWork<VFileSystemItem> unitOfWork)
            : base(unitOfWork) {
        }
    }
}
