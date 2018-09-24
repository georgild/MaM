using AutoMapper;
using BizModels.VFileSystem;
using BusinessServicesContracts.VFileSystem;
using BusinessServicesImpl.Base;
using Models.VFileSystem;
using RepositoryContracts.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessServicesImpl.VFileSystem {
    public class AssetServiceImpl : BaseEntityServiceImpl<VFileSystemItem, AssetBizModel>, IAssetService {

        public AssetServiceImpl(
                IBaseEntityUnitOfWork<VFileSystemItem> unitOfWork,
                IMapper mapper)
                : base(unitOfWork, mapper) {
        }
    }
}
