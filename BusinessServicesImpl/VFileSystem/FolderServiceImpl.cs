﻿using BusinessServicesContracts.VFileSystem;
using BusinessServicesImpl.Base;
using Models.VFileSystem;
using RepositoryContracts.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessServicesImpl.VFileSystem {
    public class FolderServiceImpl : BaseEntityServiceImpl<VFileSystemItem>, IFolderService {

        public FolderServiceImpl(
                IBaseEntityUnitOfWork<VFileSystemItem> unitOfWork)
            : base(unitOfWork) {
        }
    }
}
