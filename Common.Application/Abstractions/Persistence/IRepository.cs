﻿using System.Linq.Expressions;

namespace Common.Application.Abstractions.Persistence;

public interface IRepository<TEntity> where TEntity : class, new()
{
    Task<TEntity[]> GetListAsync(
        int? offset = null,
        int? limit = null,
        Expression<Func<TEntity, bool>>? filterBy = null,
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool? descending = null,
        CancellationToken cancellationToken = default);
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,
            Expression<Func<TEntity, bool>>? filterBy = null,
            CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}
