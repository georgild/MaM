using Models.Identity;
using Models.Tasks;
using Models.VFileSystem;
using Repository.Contexts;
using RepositoryContracts.Base;
using RepositoryContracts.Identity;
using RepositoryContracts.Tasks;
using RepositoryContracts.UnitsOfWork;
using RepositoryContracts.VFileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.UnitsOfWork {
    public class VFileSystemUnitOfWork : BaseEntityUnitOfWork<VFileSystemItem>, IVFileSystemUnitOfWork {

        public VFileSystemUnitOfWork(
            Context context,
            IEntityAncestorRepository entityAncestorRepository,
            IVFileSystemItemRepository fsRepository) : 
            base(context, entityAncestorRepository, fsRepository) {
        }
    }
}
