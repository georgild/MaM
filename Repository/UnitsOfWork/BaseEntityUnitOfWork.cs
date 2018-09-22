using Models.Base;
using Repository.Contexts;
using RepositoryContracts;
using RepositoryContracts.Base;
using RepositoryContracts.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitsOfWork {
    public class BaseEntityUnitOfWork<TEntity> : IBaseEntityUnitOfWork<TEntity> where TEntity : IEntity {

        private readonly Context context = default(Context);

        private bool disposed = false;

        public IGenericRepository<TEntity> EntityResourceRepository { get; set; }

        public IEntityAncestorRepository EntityAncestorRepository { get; set; }

        public BaseEntityUnitOfWork(Context context,
            IEntityAncestorRepository entityAncestorRepository,
            IGenericRepository<TEntity> entityResourceRepository) {

            this.context = context;
            EntityAncestorRepository = entityAncestorRepository;
            EntityResourceRepository = entityResourceRepository;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {

            if (!this.disposed && disposing) {
                context.Dispose();
            }

            disposed = true;
        }

        public async Task<int> Save() {
            return await context.SaveChangesAsync();
        }
    }
}
