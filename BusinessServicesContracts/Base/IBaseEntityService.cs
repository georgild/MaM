using CustomPolicyAuth;
using Models.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServicesContracts.Base {
    public interface IBaseEntityService<TEntity> : IBaseObjectService<TEntity> where TEntity : IEntity {

        /// <summary>
        /// Creates entity of type TEntity in given parent.
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="entity"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<Guid> Create(ContextPrincipal principal, TEntity entity, Guid parentId);

        /// <summary>
        /// Mark the entity as "deleted". Just updates it, DOES NOT delete it really from the DB.
        /// Returns number of affected objects.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        Task<bool> SoftDelete(ContextPrincipal principal, Guid resourceId);
    }
}
