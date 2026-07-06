using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopNest.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);                                  // Ek single record uski Id se
        Task<IEnumerable<T>> GetAllAsync();                              // Saari records
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);  // Condition ke saath filter (jaise IsActive == true)
        Task AddAsync(T entity);                                          // Naya record add karna
        void Update(T entity);                                            // Record update karna
        void Delete(T entity);                                            // Record delete karna (hum mostly IsActive use karenge, but interface mein rakhna standard hai)
    }
}
