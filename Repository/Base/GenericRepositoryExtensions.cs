using Models.Base;
using RepositoryContracts.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Base {
    public static class GenericRepositoryExtensions {
        // IMPORTANT: These methods should not be included and/or used in a UnitOfWork class, because they
        // call SaveChanges() on their own and can not be chained together with others in a transaction.
        /// <summary>
        /// Inserts and saves an Entity in the database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="repository"></param>
        /// <param name="entity"></param>
        /// <returns>Count of affected rows</returns>
        public static async Task<int> InsertAndSaveAsync<TEntity>(this IGenericRepository<TEntity> repository,
            TEntity entity) where TEntity : class, IBaseRecord {
            var genericRepository = repository as GenericRepository<TEntity>;
            if (null == genericRepository) {
                throw new NullReferenceException(@"Generic Repository can not be null.");
            }

            genericRepository.Insert(entity);
            return await genericRepository.context.SaveChangesAsync().ConfigureAwait(false);
        }

        // IMPORTANT: These methods should not be included and/or used in a UnitOfWork class, because they
        // call SaveChanges() on their own and can not be chained together with others in a transaction.
        /// <summary>
        /// Deletes and saves an Entity in the database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="repository"></param>
        /// <returns>Count of affected rows</returns>
        public static async Task<int> DeleteAndSaveAsync<TEntity>(this IGenericRepository<TEntity> repository,
            Guid id) where TEntity : class, IBaseRecord {
            var genericRepository = repository as GenericRepository<TEntity>;
            if (null == genericRepository) {
                throw new NullReferenceException(@"Generic Repository can not be null.");
            }

            genericRepository.DeleteById(id);
            return await genericRepository.context.SaveChangesAsync().ConfigureAwait(false);
        }

        // IMPORTANT: These methods should not be included and/or used in a UnitOfWork class, because they
        // call SaveChanges() on their own and can not be chained together with others in a transaction.
        /// <summary>
        /// Deletes and saves an Entity in the database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="repository"></param>
        /// <param name="entity"></param>
        /// <returns>Count of affected rows</returns>
        public static async Task<int> DeleteAndSaveAsync<TEntity>(this IGenericRepository<TEntity> repository,
            TEntity entity) where TEntity : class, IBaseRecord {
            var genericRepository = repository as GenericRepository<TEntity>;
            if (null == genericRepository) {
                throw new NullReferenceException(@"Generic Repository can not be null.");
            }

            genericRepository.Delete(entity);
            return await genericRepository.context.SaveChangesAsync().ConfigureAwait(false);
        }

        // IMPORTANT: These methods should not be included and/or used in a UnitOfWork class, because they
        // call SaveChanges() on their own and can not be chained together with others in a transaction.
        /// <summary>
        /// Deletes a range of Entities in the database and saves .
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="repository"></param>
        /// <returns>Count of affected rows</returns>
        public static async Task<int> DeleteRangeAndSaveAsync<TEntity>
            (this IGenericRepository<TEntity> repository,
            IEnumerable<TEntity> list) where TEntity : class, IBaseRecord {
            var genericRepository = repository as GenericRepository<TEntity>;
            if (null == genericRepository) {
                throw new NullReferenceException(@"Generic Repository can not be null.");
            }

            genericRepository.DeleteRange(list);
            return await genericRepository.context.SaveChangesAsync().ConfigureAwait(false);
        }

        // IMPORTANT: These methods should not be included and/or used in a UnitOfWork class, because they
        // call SaveChanges() on their own and can not be chained together with others in a transaction.
        /// <summary>
        /// Updates and saves an Entity in the database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="repository"></param>
        /// <returns>Count of affected rows</returns>
        public static async Task<int> UpdateAndSaveAsync<TEntity>(this IGenericRepository<TEntity> repository,
            TEntity entityToUpdate) where TEntity : class, IBaseRecord {
            var genericRepository = repository as GenericRepository<TEntity>;
            if (null == genericRepository) {
                throw new NullReferenceException(@"Generic Repository can not be null.");
            }

            genericRepository.Update(entityToUpdate);
            return await genericRepository.context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
