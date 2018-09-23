using Models.Identity;
using Repository.Contexts;
using RepositoryContracts.Base;
using RepositoryContracts.Identity;
using RepositoryContracts.UnitsOfWork;
using RepositoryContracts.VFileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.UnitsOfWork {
    public class OrganizationUnitOfWork : BaseEntityUnitOfWork<Organization>, IOrganizationUnitOfWork {

        public IVFileSystemItemRepository VFileSystemItemRepository { get; set; }

        public OrganizationUnitOfWork(
            Context context,
            IEntityAncestorRepository entityAncestorRepository,
            IOrganizationRepository organizationRepository,
            IVFileSystemItemRepository vFileSystemItemRepository) : 
            base(context, entityAncestorRepository, organizationRepository) {

            VFileSystemItemRepository = vFileSystemItemRepository;
        }
    }
}
