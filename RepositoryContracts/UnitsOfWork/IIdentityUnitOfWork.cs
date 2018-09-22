using RepositoryContracts.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryContracts.UnitsOfWork {
    public interface IIdentityUnitOfWork : IBaseUnitOfWork {

        ITokenRepository TokenRepository { get; }
    }
}
