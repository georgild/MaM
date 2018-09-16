using CustomPolicyAuth;
using Models.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServicesContracts.Base {
    public interface IBaseObjectService<T> where T : IBaseRecord {

        /// <summary>
        /// Creates the object.
        /// </summary>
        Task<Guid> Create(ContextPrincipal principal, T obj);

        /// <summary>
        /// Find enumerable of objects with expression.
        /// </summary>
        Task<IEnumerable<T>> Find(ContextPrincipal principal, Expression<Func<T, bool>> expression, int start, int limit);

        /// <summary>
        /// Deletes enumerable of objects with expression.
        /// </summary>
        Task<Tuple<int, int>> Delete(ContextPrincipal principal, Expression<Func<T, bool>> expression);

        /// <summary>
        /// Update with replace an object. Returns true if there are any affected records.
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="practice"></param>
        /// <returns></returns>
        Task<bool> Update(ContextPrincipal principal, T obj, Guid resourceId);

        /// <summary>
        /// Hard deletes(purges) an object using the grain Id.
        /// Intentionally named Purge to avoid confusion with the "soft" delete.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        Task<bool> Purge(ContextPrincipal principal, Guid resourceId);

        /// <summary>
        /// Returns true if object with the grain Id exists in DB.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        Task<bool> ObjectExistsAsync(ContextPrincipal principal, Guid resourceId);

        /// <summary>
        /// Get an object by grain Id.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        Task<T> Get(ContextPrincipal principal, Guid resourceId);
    }
}
