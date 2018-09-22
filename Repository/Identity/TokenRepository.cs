using Models.Identity;
using Repository.Base;
using Repository.Contexts;
using RepositoryContracts;
using RepositoryContracts.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Identity {
    public class TokenRepository : GenericRepository<RefreshToken>, ITokenRepository {

        public TokenRepository(Context context)
             : base(context) { }
    }
}
