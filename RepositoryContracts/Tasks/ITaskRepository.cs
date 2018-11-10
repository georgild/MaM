using Models.Tasks;
using RepositoryContracts.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryContracts.Tasks {
    public interface ITaskRepository : IGenericRepository<TaskModel> {
    }
}
