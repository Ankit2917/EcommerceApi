using System.Linq.Expressions;

namespace EcommerceApi.Data.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> DeleteAysncAll(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FilterListById(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int id);

        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
         
    }
}
