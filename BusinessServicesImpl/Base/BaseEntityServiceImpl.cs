using BusinessServicesContracts.Base;
using CommonUtils;
using CustomPolicyAuth;
using Models.Base;
using RepositoryContracts.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServicesImpl.Base {
    public abstract class BaseEntityServiceImpl<TEntity> : BaseObjectServiceImpl<TEntity>, IBaseEntityService<TEntity>
        where TEntity : Entity {

        protected readonly IBaseEntityUnitOfWork<TEntity> _unitOfWork;

        public BaseEntityServiceImpl(
                IBaseEntityUnitOfWork<TEntity> unitOfWork)
            : base(unitOfWork.EntityResourceRepository) {

            _unitOfWork = unitOfWork;
        }

        public virtual async Task<Guid> Create(ContextPrincipal principal, TEntity entity, Guid parentId) {

            if (null == principal || null == entity || Guid.Empty == parentId) {
                throw new ArgumentException();
            }

            entity.Id = Guid.NewGuid();
            entity.CreatedBy = principal.UserId;
            entity.ModifiedBy = principal.UserId;

            await InsertAncestors(entity, parentId);

            _unitOfWork.EntityResourceRepository.Insert(entity);

            int persistedObjects = await _unitOfWork.Save();
            if (persistedObjects <= 0) {
                throw new InvalidOperationException(@"No DB records have been persisted!");
            }

            return entity.Id;
        }

        /// <summary>
        /// This helper method mirrors all the parent ancestor records for the new child and
        /// inserts them (WITHOUT PERSISTENCE) in the ancestors repo.
        /// </summary>
        /// <param name="child"></param>
        protected async Task InsertAncestors(IEntity child, Guid parentId) {

            IEnumerable<EntityAncestor> parentAncestors = await _unitOfWork.EntityAncestorRepository.FindAsync(
                anc => anc.EntityId.Equals(parentId),
                o => o.OrderBy(u => u.Rank)
            );

            List<EntityAncestor> childAncestors = new List<EntityAncestor>();

            foreach (EntityAncestor parentAncestor in parentAncestors) {

                childAncestors.Add(new EntityAncestor {
                    Rank = parentAncestor.Rank,
                    AncestorId = parentAncestor.AncestorId,
                    EntityId = child.Id
                });
            }

            childAncestors.Add(new EntityAncestor {
                Rank = parentAncestors.Count() + 1,
                AncestorId = child.Id,
                EntityId = child.Id
            });

            _unitOfWork.EntityAncestorRepository.InsertRange(childAncestors);
        }

        public override async Task<IEnumerable<TEntity>> Find(
                ContextPrincipal principal,
                Expression<Func<TEntity, bool>> expression,
                int start,
                int limit) {

            if (null == principal || null == expression) {
                throw new ArgumentNullException();
            }

            expression = expression.And(ent => !ent.Deleted);

            return await base.Find(principal, expression, start, limit);
        }

        public virtual async Task<bool> SoftDelete(ContextPrincipal principal, Guid resourceId) {

            TEntity entityToUpdate = await base.Get(principal, resourceId);
            if (null == entityToUpdate) {
                throw new ApplicationException(@"Could not find entity to soft delete it!");
            }

            entityToUpdate.Deleted = true;
            return await Update(principal, entityToUpdate, resourceId);
        }


        public override async Task<bool> Update(ContextPrincipal principal, TEntity entity, Guid resourceId) {

            if (null == principal || null == entity) {
                throw new ArgumentNullException();
            }

            entity.ModifiedBy = principal.UserId;
            entity.DateModified = DateTime.UtcNow;

            return await base.Update(principal, entity, resourceId);
        }
    }
}
