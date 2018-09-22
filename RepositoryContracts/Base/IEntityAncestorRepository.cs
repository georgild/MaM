using Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RepositoryContracts.Base {
    public interface IEntityAncestorRepository : IGenericRepository<EntityAncestor> {
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
        Task<List<ТEntity>> FindWithInclude<ТEntity>(
                Expression<Func<EntityAncestor, bool>> leftFilter,
                Expression<Func<EntityAncestor, Guid>> outerKeySelector,
                Expression<Func<ТEntity, Guid>> innerKeySelector,
                Expression<Func<ТEntity, bool>> rightFilter = null,
                Func<IQueryable<ТEntity>, IOrderedQueryable<ТEntity>> orderBy = null,
                int start = 0,
                int limit = 25) where ТEntity : class, IBaseRecord;
    }
}
