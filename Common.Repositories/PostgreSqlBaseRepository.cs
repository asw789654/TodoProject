using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Common.Application.Abstractions.Persistence;

namespace Common.Persistence;

public class SqlServerBaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
{
    private readonly ApplicationDbContext _applicationDbContext;

    public SqlServerBaseRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<TEntity[]> GetListAsync(
        int? offset = null,
        int? limit = null,
        Expression<Func<TEntity, bool>>? filterBy = null,
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool? descending = null,
        CancellationToken cancellationToken = default)
    {
        var queryable = _applicationDbContext.Set<TEntity>().AsQueryable();

        if (predicate is not null)
        {
            queryable = queryable.Where(predicate);
        }

        if (filterBy is not null)
        {
            queryable = queryable.Where(filterBy);
        }

        if (orderBy is not null)
        {
            queryable = descending == true ? queryable.OrderByDescending(orderBy) : queryable.OrderBy(orderBy);
        }

        if (offset.HasValue)
        {
            queryable = queryable.Skip(offset.Value);
        }

        if (limit.HasValue)
        {
            queryable = queryable.Take(limit.Value);
        }

        return await queryable.ToArrayAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, bool>>? filterBy = null,
        CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>().AsQueryable();
        if (predicate != null)
        {
            set = set.Where(predicate);
        }
        if (filterBy != null)
        {
            set = set.Where(filterBy);
        }
        return await set.CountAsync(cancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>();
        set.Add(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>();
        set.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>();
        set.Remove(entity);
        return await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>();
        return predicate == null ? await set.SingleOrDefaultAsync(cancellationToken) : await set.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
    Expression<Func<TEntity, bool>>? predicate = null,
    CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>();
        return predicate == null ? await set.FirstOrDefaultAsync(cancellationToken) : await set.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity> SingleAsync(
    Expression<Func<TEntity, bool>>? predicate = null,
    CancellationToken cancellationToken = default)
    {
        var set = _applicationDbContext.Set<TEntity>();
        return predicate == null ? await set.SingleAsync(cancellationToken) : await set.SingleOrDefaultAsync(predicate, cancellationToken);
    }
}
