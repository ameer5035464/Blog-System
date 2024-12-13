using BlogSystem.DAL._Data;
using BlogSystem.DAL.Contracts;
using System.Collections.Concurrent;

namespace BlogSystem.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlogSystemDbContext _dbContext;
        private readonly ConcurrentDictionary<string,object> _check;

        public UnitOfWork(BlogSystemDbContext dbContext)
        {
            _dbContext = dbContext;
            _check = new ConcurrentDictionary<string, object>();
        }
            
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class 
        {
            var key = typeof(TEntity).Name;
            var value = new GenericRepository<TEntity>(_dbContext);

            var repo = _check.GetOrAdd(key, value);

            return (IGenericRepository<TEntity>) repo;
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public ValueTask DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }
    }
}
