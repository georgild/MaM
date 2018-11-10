using Models.Tasks;
using Repository.Base;
using Repository.Contexts;
using RepositoryContracts.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Tasks {
    public class TaskRepository : GenericRepository<TaskModel>, ITaskRepository {

        public TaskRepository(Context context)
             : base(context) { }
    }
}
