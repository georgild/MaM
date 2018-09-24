using AutoMapper;
using BizModels.Identity;
using BusinessServicesContracts.Identity;
using BusinessServicesImpl.Base;
using Models.Identity;
using RepositoryContracts.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessServicesImpl.Identity {
    public class UserServiceImpl : BaseEntityServiceImpl<User, UserBizModel>, IUserService {

        public UserServiceImpl(
                IBaseEntityUnitOfWork<User> unitOfWork,
                IMapper mapper)
                : base(unitOfWork, mapper) {
        }
    }
}
