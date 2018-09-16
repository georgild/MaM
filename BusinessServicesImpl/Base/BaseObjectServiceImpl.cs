using BusinessServicesContracts.Base;
using CustomPolicyAuth;
using Models.Base;
using Repository.Repos.Base;
using RepositoryContracts.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServicesImpl.Base {
    public abstract class BaseObjectServiceImpl<TEntity> : BaseServiceImpl, IBaseObjectService<TEntity>
        where TEntity : class, IBaseRecord {

        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IGenericRepository<TEntity> _repo;

        public BaseObjectServiceImpl(
                IGenericRepository<TEntity> repo) {
            _repo = repo;
        }

        public virtual async Task<Guid> Create(ContextPrincipal principal, TEntity obj) {
            await _repo.InsertAndSaveAsync(obj);
            return await Task.FromResult(obj.Id);
        }

        public virtual async Task<Tuple<int, int>> Delete(
                ContextPrincipal principal, Expression<Func<TEntity, bool>> expression) {

            IEnumerable<TEntity> matching = await _repo.FindAsync(expression);
            int matchingCount = matching.Count();
            return await _repo.DeleteRangeAndSaveAsync(matching).ContinueWith((deleted) => new Tuple<int, int>(matchingCount, deleted.Result));
        }

        public virtual async Task<IEnumerable<TEntity>> Find(
                ContextPrincipal principal,
                Expression<Func<TEntity, bool>> expression,
                int start,
                int limit) {

            return await _repo.FindAsync(p => true, null, start, limit);
        }

        public virtual async Task<bool> Purge(ContextPrincipal principal, Guid resourceId) {

            TEntity obj = null;
            try {
                obj = await _repo.FindByIDAsync(resourceId);
            }
            catch (Exception e) {
                Logger.Error(
                    string.Format(@"An exception has been thrown searching for resource {0}!", resourceId), e);
                return false;
            }

            if (obj == null) {
                throw new ApplicationException(@"Object not found.");
            }

            try {
                return await _repo.DeleteAndSaveAsync(obj) > 0;
            }
            catch (Exception e) {
                Logger.Error(@"Commit failed for unit-of-work!", e);
                return false;
            }
        }
        public virtual async Task<bool> ObjectExistsAsync(ContextPrincipal principal, Guid resourceId) {

            return await _repo.ExistsAsync(obj => obj.Id == resourceId);
        }

        public virtual async Task<TEntity> Get(ContextPrincipal principal, Guid resourceId) {
            return await _repo.FindByIDAsync(resourceId);
        }

        public virtual async Task<bool> Update(ContextPrincipal principal, TEntity obj, Guid resourceId) {

            obj.Id = resourceId;
            return await _repo.UpdateAndSaveAsync(obj) > 0;
        }
    }
}
