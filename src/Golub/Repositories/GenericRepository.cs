using Golub.Contexts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Golub.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual T GetById<TId>(TId id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Generic FirstOrDefaultAsync with dynamic where clause
        /// </summary>
        /// <param name="where"></param>
        /// <returns>Entity or null</returns>
        public Task<T> GetFirstAsync(Expression<Func<T, bool>> where)
        {
            return _dbSet.FirstOrDefaultAsync(where);
        }

        public Task<List<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return _dbSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        // TODO: can we make this IEnumerable and not List?
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbSet.AddRange(entities);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entities;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Update<U>(U id, T entity)
        {
            T exist = _dbSet.Find(id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);

            await _dbContext.SaveChangesAsync();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbSet.AttachRange(entities);

            foreach (T entity in entities)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(List<T> entities)
        {
            _dbSet.RemoveRange(entities);

            await _dbContext.SaveChangesAsync();
        }

        public Task<List<T>> GetAsync()
        {
            return _dbSet.ToListAsync();
        }

        /// <summary>
        /// Generic ToListAsync with dynamic where clause
        /// </summary>
        /// <param name="where"></param>
        /// <returns>Entities or empty list</returns>
        public Task<List<T>> GetAsync(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).ToListAsync();
        }

        public Task<int> CountAsync()
        {
            return _dbSet.CountAsync();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> where)
        {
            return _dbSet.CountAsync(where);
        }

        public Task<bool> AnyAsync()
        {
            return _dbSet.AnyAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> where)
        {
            return _dbSet.AnyAsync(where);
        }

        public Task<bool> AllAsync(Expression<Func<T, bool>> where)
        {
            return _dbSet.AllAsync(where);
        }
    }
}