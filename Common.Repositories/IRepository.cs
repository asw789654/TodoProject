using System.Linq.Expressions;

namespace Common.Repositories;

public interface IRepository<TEntity> where TEntity : class, new()
{
    TEntity[] GetList(
        int? offset = null,
        int? limit = null,
        Expression<Func<TEntity, bool>>? filterBy = null,
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool? descending = null);
    TEntity? SingleOrDefault(Expression<Func<TEntity, bool>>? predicate = null);
    int Count(Expression<Func<TEntity, bool>>? predicate = null, 
            Expression<Func<TEntity, bool>>? filterBy = null);
    TEntity Add(TEntity book);
    TEntity Update(TEntity book);
    bool Delete(TEntity book);
}
