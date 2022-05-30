using BlackSmith.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlackSmith.Domain.Repositories;
public interface IRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includes);
    Task<T?> GetById(int id);
    Task<T?> GetById(int id, params Expression<Func<T, object>>[] includes);
    Task<T> Add(T entity);
    Task<T> Update(T entity);
    Task<bool> Delete(T entity);
}
