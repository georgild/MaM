using Models.Identity;
using Models.Tasks;
using Models.VFileSystem;
using RepositoryContracts.VFileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryContracts.UnitsOfWork {
    public interface IVFileSystemUnitOfWork : IBaseEntityUnitOfWork<VFileSystemItem> {
    }
}
