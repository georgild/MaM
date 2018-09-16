using Models.Identity;
using Repository.Contexts;
using Repository.Repos.Base;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repos {
    public class TokenRepository : GenericRepository<RefreshToken>, ITokenRepository {

        public TokenRepository(Context context)
             : base(context) { }
    }
}
