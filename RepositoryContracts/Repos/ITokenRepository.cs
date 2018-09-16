using RepositoryContracts.Repos.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Models.Identity;

namespace RepositoryContracts {
    public interface ITokenRepository : IGenericRepository<RefreshToken> {
    }
}
