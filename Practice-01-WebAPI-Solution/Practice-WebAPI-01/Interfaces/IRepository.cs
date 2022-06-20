using System.Linq.Expressions;

namespace Practice_WebAPI_01.Interfaces;

public interface IRepository<T> where T : class
{
    //Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter);
    Task<T> GetById(int id);
    Task<IEnumerable<T>> GetAll();
    //Task<bool> Exists(string name);
    Task<bool> Exists(Expression<Func<T, bool>> predicate);
    Task<bool> Add(T entity); // returns void to separate Save() logic
    Task<bool> Remove(T entity);
    Task<bool> RemoveRange(IEnumerable<T> entity);
    Task<bool> Update(T entity);
}
