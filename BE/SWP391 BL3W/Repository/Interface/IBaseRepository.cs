﻿using SWP391_BL3W.Database;
using System.Linq.Expressions;

namespace SWP391_BL3W.Repository.Interface
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get();

        Task<TEntity?> GetById(object id, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        void Delete(params TEntity[] entities);

        void Update(params TEntity[] entities);

        void Delete(TEntity entity);

        void Update(TEntity entity);
        void Add(TEntity entity);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        public Task<IEnumerable<TEntity>?> Find(Expression<Func<TEntity, bool>>? filter = null,
            string includeProperties = "",
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
        public Task<TEntity?> FindOne(Expression<Func<TEntity, bool>>? filter = null, string includeProperties = "");
        public Task<bool> AddHashKey(TEntity entity, CancellationToken cancellationToken = default);

    }
}
