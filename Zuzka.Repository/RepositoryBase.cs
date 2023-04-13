using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Zuzka.RepositoryContracts;

namespace Zuzka.Repository
{
    public abstract class RepositoryBase<TContext, T> : IRepositoryBase<T> where T : class where TContext : DbContext
    {
        protected TContext _context;
        public RepositoryBase(TContext context) => this._context = context;

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges) =>
            !trackChanges ?
                _context.Set<T>()
                .Where(expression)
                .AsNoTracking() :
                _context.Set<T>()
                .Where(expression);

        public async Task Add(T entity) => await _context.AddAsync(entity);


    }
}
