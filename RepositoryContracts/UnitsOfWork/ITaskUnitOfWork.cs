using Models.Identity;
using Models.Tasks;
using RepositoryContracts.VFileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryContracts.UnitsOfWork {
    public interface ITaskUnitOfWork : IBaseEntityUnitOfWork<TaskModel> {
    }
}
