using AutoMapper;
using BizModels.Tasks;
using BusinessServicesContracts.Tasks;
using BusinessServicesImpl.Base;
using Models.Tasks;
using RepositoryContracts.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessServicesImpl.Tasks {
    public class TaskServiceImpl : BaseEntityServiceImpl<TaskModel, TaskBizModel>, ITaskService {

        public TaskServiceImpl(
                IBaseEntityUnitOfWork<TaskModel> unitOfWork,
                IMapper mapper)
                : base(unitOfWork, mapper) {
        }
    }
}
