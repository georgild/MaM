using Models.Identity;
using RepositoryContracts.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryContracts.Identity {
    public interface IUserRepository : IGenericRepository<User> {
    }
}
