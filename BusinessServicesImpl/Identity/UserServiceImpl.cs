using BusinessServicesContracts.Identity;
using BusinessServicesImpl.Base;
using Models.Identity;
using RepositoryContracts.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessServicesImpl.Identity {
    public class UserServiceImpl : BaseEntityServiceImpl<User>, IUserService {

        public UserServiceImpl(
                IBaseEntityUnitOfWork<User> unitOfWork)
            : base(unitOfWork) {
        }
    }
}
