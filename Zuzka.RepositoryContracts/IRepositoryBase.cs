using System.Linq.Expressions;

namespace Zuzka.RepositoryContracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        Task Add(T entity);
    }
}
