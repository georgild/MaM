using Models.Identity;
using RepositoryContracts.VFileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryContracts.UnitsOfWork {
    public interface IOrganizationUnitOfWork : IBaseEntityUnitOfWork<Organization> {

        IVFileSystemItemRepository VFileSystemItemRepository { get; set;  }
    }
}
