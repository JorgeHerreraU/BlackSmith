﻿using System.Linq.Expressions;
using BlackSmith.Domain.Models;

namespace BlackSmith.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includes);
    Task<T?> Get(Expression<Func<T, bool>> predicate);
    Task<T?> GetById(int id);
    Task<T?> GetById(int id, params Expression<Func<T, object>>[] includes);
    Task<T> Add(T entity);
    Task<T> Update(T entity);
    Task<bool> Delete(T entity);
}