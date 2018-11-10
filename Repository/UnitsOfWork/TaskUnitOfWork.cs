using Models.Identity;
using Models.Tasks;
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
    public class TaskUnitOfWork : BaseEntityUnitOfWork<TaskModel>, ITaskUnitOfWork {

        public TaskUnitOfWork(
            Context context,
            IEntityAncestorRepository entityAncestorRepository,
            ITaskRepository taskRepository) : 
            base(context, entityAncestorRepository, taskRepository) {
        }
    }
}
