using System;
using System.Collections.Generic;
using System.Text;
using Models.Identity;
using RepositoryContracts.Base;

namespace RepositoryContracts.Identity {
    public interface ITokenRepository : IGenericRepository<RefreshToken> {
    }
}
