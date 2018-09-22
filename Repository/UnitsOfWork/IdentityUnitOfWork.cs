using Models.Identity;
using Repository.Contexts;
using RepositoryContracts;
using RepositoryContracts.Identity;
using RepositoryContracts.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitsOfWork {
    public class IdentityUnitOfWork : IIdentityUnitOfWork {

        private readonly Context context;

        private bool disposed = false;

        public IdentityUnitOfWork(Context context,
            ITokenRepository tokenRepository) {

            this.context = context;
            this.TokenRepository = tokenRepository;

        }

        public ITokenRepository TokenRepository { get; set; }

        /// <summary>
        /// Returns the number of affected items.
        /// </summary>
        public Task<int> Save() {
            return context.SaveChangesAsync();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    context.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
