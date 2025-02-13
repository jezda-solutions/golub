using System.Linq.Expressions;

namespace Golub.Interfaces
{
    /// <summary>
    /// Generic repository interface
    /// Constains all default methods that one can expect from a repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<T> GetById(Guid id);
        T GetById<TId>(TId id);
        Task<List<T>> GetPagedReponseAsync(int pageNumber, int pageSize);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Try to use Begin* methods
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Try to use Begin* methods
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update<U>(U id, T entity);

        /// <summary>
        /// Try to use Begin* methods
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(List<T> entities);

        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> where);

        Task<bool> AnyAsync();

        Task<bool> AnyAsync(Expression<Func<T, bool>> where);

        Task<List<T>> GetAsync();
        Task<List<T>> GetAsync(Expression<Func<T, bool>> where);

        Task<T> GetFirstAsync(Expression<Func<T, bool>> where);

        Task<bool> AllAsync(Expression<Func<T, bool>> where);
    }
}