using BizModels.Tasks;
using BusinessServicesContracts.Base;
using Models.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessServicesContracts.Tasks {
    public interface ITaskService : IBaseEntityService<TaskModel, TaskBizModel> {
    }
}
