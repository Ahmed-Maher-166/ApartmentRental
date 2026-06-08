using SmartRental.Core.Models;

namespace SmartRental.Core.Repository
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public IGenericRepository<TEntity> Repository<TEntity>()
       where TEntity : BaseEntity;
        public Task<int> CompleteAsync();

    }
}