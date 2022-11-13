using MoviesStore.Domain.Models;
using System.Linq.Expressions;

namespace MoviesStore.Domain.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Add(TEntity entity);

        Task<List<TEntity>> GetAll();

        Task<TEntity> GetById(int id);

        Task Update(TEntity entity);

        Task Remove(TEntity entity);

        Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate);

        Task<int> SaveChanges();
    }
}
