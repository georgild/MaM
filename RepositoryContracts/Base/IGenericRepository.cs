using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts.Base {
    public interface IGenericRepository<TEntity> {
        /// <summary>
        /// Example: persistence.AggregateAsync(p => true, p => p.Id, g => g.Sum(x => x.ExampleValue));
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="filter"></param>
        /// <param name="groupBy"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> AggregateAsync<TKey, TResult>(
            Expression<Func<TEntity, bool>> filter,
            Func<TEntity, TKey> groupBy,
            Func<IGrouping<TKey, TEntity>, TResult> selector);

        Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int start = 0,
            int limit = 25,
            Expression<Func<TEntity, object>>[] includeProperties = null);

        /// <summary>
        /// Wrapped method that sends a 'FindAsync' request with start=0 and limit=1 and returns the first matched record.  
        /// </summary>
        Task<TEntity> FindOneAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, object>>[] includeFilters = null);

        Task<IEnumerable<TResult>> FindProjectionAsync<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> projection,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int start = 0,
            int limit = 25);

        Task<TEntity> FindByIDAsync(object id);

        void Insert(TEntity entity);

        /// <summary>
        /// Inserts range of entities in the context. DOES NOT PERSIST THEM IN DB.
        /// </summary>
        /// <param name="entities"></param>
        void InsertRange(IEnumerable<TEntity> entities);

        void DeleteById(Guid id);

        void DeleteRange(IEnumerable<TEntity> list);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter);

    }
}
