using Domain.Common;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.Reposatory.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> ApplySort(IQueryable<TEntity> entities, IList<OrderByModel> OrderingParameters);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool trackable = true, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool trackable = true, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool trackable = true, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null, bool trackable = true, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> GetPagedAsync(PagedListInfo pagedListInfo, IQueryable<TEntity> query);
        Task<IEnumerable<TEntity>> GetPagedDistinctAsync(PagedListInfo pagedListInfo, IQueryable<TEntity> query);
        Task<TEntity> GetByIdAsync(object id);
        Task InsertAsync(TEntity entity);
        void Update(TEntity entityToUpdate);
        void Delete(TEntity entityToDelete);
        void DeleteByQueryAsync(Expression<Func<TEntity, bool>> filter = null);
        Task Delete(object id);
    }
}
