using BlogSystem.DAL._Data;
using BlogSystem.DAL.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.DAL
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly BlogSystemDbContext _dbContext;

        public GenericRepository(BlogSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetAsync(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }
    }
}
