using Microsoft.EntityFrameworkCore;
using Models.Base;
using Repository.Contexts;
using Repository.Repos.Base;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repos {
    public class EntityAncestorRepository : GenericRepository<EntityAncestor>, IEntityAncestorRepository {
        public EntityAncestorRepository(Context context)
             : base(context) { }

        /// <summary>
        /// READ WHY THIS ONE IS NEEDED: 
        /// We can't make JOIN with entity framework between EntityAncestors and Folders/Documents
        /// because we don't have foreign keys between as Folders and Documents are in distinct tables.
        /// We can't use the ordinary FindWithInclude(Join) method from generic repo.
        /// </summary>
        /// <typeparam name="ТEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <returns></returns>
        public Task<List<ТEntity>> FindWithInclude<ТEntity>(
                Expression<Func<EntityAncestor, bool>> leftFilter,
                Expression<Func<EntityAncestor, Guid>> outerKeySelector,
                Expression<Func<ТEntity, Guid>> innerKeySelector,
                Expression<Func<ТEntity, bool>> rightFilter = null,
                Func<IQueryable<ТEntity>, IOrderedQueryable<ТEntity>> orderBy = null,
                int start = 0,
                int limit = 25) where ТEntity : class, IBaseRecord {

            IQueryable<ТEntity> queryable = context.Set<EntityAncestor>().
                Where(leftFilter).
                Join(context.Set<ТEntity>(),
                    outerKeySelector,
                    innerKeySelector,
                    (anc, ch) => ch);

            if (null != rightFilter) {
                queryable = queryable.Where(rightFilter);
            }

            queryable = queryable.Skip(start).Take(limit);

            if (null == orderBy) {
                orderBy = (q => q.OrderBy(u => u.Id));
            }

            return orderBy(queryable).ToListAsync();
        }
    }
}
