using Microsoft.EntityFrameworkCore;
using RepositoryContracts.Repos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Models.Base;

namespace Repository.Repos.Base {
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IBaseRecord {

        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context) {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TResult>> AggregateAsync<TKey, TResult>(
            Expression<Func<TEntity, bool>> filter,
            Func<TEntity, TKey> groupBy,
            Func<IGrouping<TKey, TEntity>, TResult> selector) {

            return await (dbSet.Where(filter).GroupBy(groupBy).Select(selector)).AsQueryable().ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int start = 0,
            int limit = 25,
            Expression<Func<TEntity, object>>[] includeProperties = null) { // includeProperties is used for relational elements

            IQueryable<TEntity> query = CreateQuery(filter, orderBy, start, limit, includeProperties);

            query = query.Skip(start).Take(limit);

            if (null == orderBy) {
                orderBy = (q => q.OrderBy(u => u.Id));
            }

            return await orderBy(query).ToListAsync();
        }

        public virtual async Task<TEntity> FindOneAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, object>>[] includeFilters = null) { // includeProperties is used for relational elements

            //return FindAsync(filter, orderBy, 0, 1, includeFilters).FirstOrDefault();
            IQueryable<TEntity> query = CreateQuery(filter, orderBy, 0, 1, includeFilters);

            if (orderBy != null) {
                return await orderBy(query).FirstOrDefaultAsync();
            }
            else {
                return await query.FirstOrDefaultAsync();
            }
        }

        public virtual async Task<IEnumerable<TResult>> FindProjectionAsync<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> projection,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int start = 0,
            int limit = 25) {

            IQueryable<TEntity> query = dbSet;

            if (filter != null) {
                query = query.Where(filter);
            }

            query = query.Skip(start).Take(limit);

            return await orderBy(query).Select(projection).ToListAsync();
        }

        public virtual async Task<TEntity> FindByIDAsync(object id) {
            return await dbSet.FindAsync(id);
        }

        // IMPORTANT: This method does not persist the changes in the database!
        // Use UnitOfWork to commit changes.
        public virtual void Insert(TEntity entity) {
            dbSet.Add(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities) {
            dbSet.AddRange(entities);
        }

        // IMPORTANT: This method does not persist the changes in the database!
        // Use UnitOfWork to commit changes.
        public virtual void DeleteById(Guid id) {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        // IMPORTANT: This method does not persist the changes in the database!
        // Use UnitOfWork to commit changes.
        public virtual void Delete(TEntity entityToDelete) {
            if (context.Entry(entityToDelete).State == EntityState.Detached) {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        // IMPORTANT: This method does not persist the changes in the database!
        // Use UnitOfWork to commit changes.
        /// <summary>
        /// Track range of items for deletion.
        /// Best Practice: Always remove instances, not Ids.
        /// </summary>
        public virtual void DeleteRange(IEnumerable<TEntity> list) {
            if (!list.Any()) {
                throw new ArgumentException(@"The lits of items to delete is empty!");
            }
            dbSet.AttachRange(list.Where(p => context.Entry(p).State == EntityState.Detached));
            dbSet.RemoveRange(list);
        }

        // IMPORTANT: This method does not persist the changes in the database!
        // Use UnitOfWork to commit changes.
        public virtual void Update(TEntity entityToUpdate) {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter) {
            IQueryable<TEntity> query = dbSet;

            if (filter != null) {
                query = query.Where(filter);
            }

            return query.AnyAsync();
        }

        public virtual Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null) {
            IQueryable<TEntity> query = dbSet;

            if (filter != null) {
                query = query.Where(filter);
            }

            return query.CountAsync();
        }

        // This method is not async, because it only creates a query. The execution of the query,
        // which is the async operation is performed in other methods.
        private IQueryable<TEntity> CreateQuery(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           int start = 0,
           int limit = 25,
           Expression<Func<TEntity, object>>[] includeProperties = null) {

            IQueryable<TEntity> query = dbSet;

            if (filter != null) {
                query = query.Where(filter);
            }

            if (includeProperties != null) {
                foreach (var include in includeProperties) {
                    query = query.Include(include);
                }
            }

            query = query.Skip(start).Take(limit);

            if (null == orderBy) {
                orderBy = (q => q.OrderBy(u => u.Id));
            }

            return orderBy(query);
        }
    }
}
