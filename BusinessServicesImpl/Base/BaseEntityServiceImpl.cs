using AutoMapper;
using BizModels.Base;
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
    public abstract class BaseEntityServiceImpl<TEntity, TBizModel> : BaseObjectServiceImpl<TEntity>, IBaseEntityService<TEntity, TBizModel>
        where TEntity : Entity
        where TBizModel : BaseEntityBizModel {

        protected readonly IBaseEntityUnitOfWork<TEntity> _unitOfWork;

        private readonly IMapper _mapper;

        public BaseEntityServiceImpl(
                IBaseEntityUnitOfWork<TEntity> unitOfWork,
                IMapper mapper)
            : base(unitOfWork.EntityResourceRepository) 
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public virtual async Task<TBizModel> GetAsBiz(ContextPrincipal principal, Guid resourceId) {
            TEntity dbObject = await this.Get(principal, resourceId);

            return _mapper.Map<TBizModel>(dbObject);
        }

        public async Task<IEnumerable<Guid>> GetDirectChildrenAsGuids(ContextPrincipal principal, Guid parentId, int start, int limit) {

            List<Guid> result = new List<Guid>();

            int parentRank = (await _unitOfWork.EntityAncestorRepository.FindProjectionAsync(
                anc => anc.EntityId == parentId && anc.AncestorId.Value == parentId,
                pr => pr.Rank,
                o => o.OrderBy(u => u.AncestorId),
                0,
                1)).FirstOrDefault();

            IEnumerable<EntityAncestor> children = await _unitOfWork.EntityAncestorRepository.FindWithInclude<EntityAncestor>(
                ancLeft => ancLeft.AncestorId.Value == parentId, // all who have the parent in their path...
                ancLeft => ancLeft.EntityId,
                ancRight => ancRight.AncestorId.Value,
                ancRight => ancRight.EntityId == ancRight.AncestorId && ancRight.Rank == parentRank + 1,
                null,
                start,
                limit); // ...and their rank is parentRank + 1

            if (!children.Any()) {
                return result;
            }

            return children.Select(x => x.EntityId);
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

        public virtual async Task<IEnumerable<TBizModel>> FindAsBiz(
                ContextPrincipal principal,
                Expression<Func<TEntity, bool>> expression,
                int start,
                int limit) {


            IEnumerable<TEntity> dbObjects = await this.Find(principal, org => true, start, limit);
            IEnumerable<TBizModel> bizModels = _mapper.Map<IEnumerable<TEntity>, IEnumerable<TBizModel>>(dbObjects);

            return bizModels;
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
