using Models.Base;
using RepositoryContracts.Repos.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryContracts.UnitsOfWork {
    public interface IBaseEntityUnitOfWork<TEntity> : IBaseUnitOfWork where TEntity : IEntity {

        IEntityAncestorRepository EntityAncestorRepository { get; }

        IGenericRepository<TEntity> EntityResourceRepository { get; }
    }
}
