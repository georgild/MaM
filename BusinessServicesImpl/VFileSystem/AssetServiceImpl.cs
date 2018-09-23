using BusinessServicesContracts.VFileSystem;
using BusinessServicesImpl.Base;
using Models.VFileSystem;
using RepositoryContracts.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessServicesImpl.VFileSystem {
    public class AssetServiceImpl : BaseEntityServiceImpl<VFileSystemItem>, IAssetService {

        public AssetServiceImpl(
                IBaseEntityUnitOfWork<VFileSystemItem> unitOfWork)
            : base(unitOfWork) {
        }
    }
}
