using Application.Abstraction.Reposatory.Base;
using Domain.Common;
using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        protected readonly BookDbContext _db;
        protected DbSet<TEntity> _dbSet;
        public Repository(BookDbContext db)
        {
            this._db = db;
            this._dbSet = this._db.Set<TEntity>();
        }


        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool trackable = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query).AsQueryable();
            }
            else
            {
                query = query.AsQueryable();
            }

            if (!trackable)
            {
                query = query.AsNoTracking();
            }

            return query;
        }


        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await this._dbSet.FindAsync(id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await this._dbSet.AddAsync(entity);
        }

        public void Update(TEntity entityToUpdate)
        {
            if (this._db.Entry(entityToUpdate).State == EntityState.Detached)
            {
                this._dbSet.Attach(entityToUpdate);
            }

            this._db.Entry(entityToUpdate).State = EntityState.Modified;
        }


        public void Delete(TEntity entityToDelete)
        {
            if (this._db.Entry(entityToDelete).State == EntityState.Detached)
            {
                this._dbSet.Attach(entityToDelete);
            }

            this._dbSet.Remove(entityToDelete);
        }
        private void Delete(IQueryable<TEntity> entitysToDelete)
        {
            this._dbSet.RemoveRange(entitysToDelete);
        }

        public void DeleteByQueryAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var entitysToDelete = this.Get(filter);

            if (entitysToDelete != null)
            {
                if (entitysToDelete.Count() == 1)
                {
                    Delete(entitysToDelete);
                }
                else if (entitysToDelete.Count() > 1)
                {
                    Delete(entitysToDelete);
                }
            }
        }

        public async Task Delete(object id)
        {
            TEntity entityToDelete = await this._dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(PagedListInfo pagedListInfo, IQueryable<TEntity> query)
        {
            return await PagedList<TEntity>.CreatePagedList(query, pagedListInfo.PageSize, pagedListInfo.PageNumber);
        }

        public async Task<IEnumerable<TEntity>> GetPagedDistinctAsync(PagedListInfo pagedListInfo, IQueryable<TEntity> query)
        {
            return await PagedList<TEntity>.CreateDistinctPagedList(query, pagedListInfo.PageSize, pagedListInfo.PageNumber);
        }



        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool trackable = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query).AsQueryable();
            }

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query,
                  (current, include) => current.Include(include));
            }

            if (trackable)
            {
                return await query.FirstOrDefaultAsync();
            }
            else
            {
                return await query.AsNoTracking().FirstOrDefaultAsync();
            }
        }

        public async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool trackable = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query).AsQueryable();
            }

            if (trackable)
            {
                return await query.ToListAsync();
            }
            else
            {
                return await query.AsNoTracking().ToListAsync();
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null, bool trackable = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            else
            {
                query = query.AsQueryable();
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (!trackable)
            {
                query = query.AsNoTracking();
            }

            return await query.AnyAsync();
        }


        public IQueryable<TEntity> ApplySort(IQueryable<TEntity> entities, IList<OrderByModel> OrderingParameters)
        {
            var propertyInfos = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            bool HasbeenOrderedBefore = false;

            foreach (var param in OrderingParameters)
            {
                if (string.IsNullOrWhiteSpace(param.OrderField))
                    continue;

                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(param.OrderField, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                entities = OrderingHelper<TEntity>(entities, objectProperty.Name, param.IsDesc, HasbeenOrderedBefore);

                HasbeenOrderedBefore = true;
            }

            return entities;
        }

        private IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), string.Empty);

            MemberExpression property = Expression.PropertyOrField(param, propertyName);

            LambdaExpression sort = Expression.Lambda(property, param);

            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sort));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }

    }


    public class BookReposatory : IBookReposatory
    {
        private readonly BookDbContext _db;
        private readonly DbSet<Book> _dbSet;
        public BookReposatory(BookDbContext db)
        {
                _db = db;
            _dbSet = this._db.Set<Book>();

        }

     

        public IQueryable<Book> Getall(Expression<Func<Book, bool>> filter = null, Func<IQueryable<Book>, IOrderedQueryable<Book>> orderBy = null, bool trackable = true, params Expression<Func<Book, object>>[] includeProperties)
        {
            IQueryable<Book> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query;
        }

        public IQueryable<Book> SearcTitle(IQueryable<Book> query,string title)
        {
            return query.CustomFilter(title);
        }
    }
    public static class QueryableExtensions
    {
       
            public static IQueryable<Book> CustomFilter(this IQueryable<Book> query, string filterValue)
            {
                if (string.IsNullOrWhiteSpace(filterValue))
                {
                    return query;
                }

                return query.Where(book => book.Auther.Contains(filterValue));
            }
        
    }

}
