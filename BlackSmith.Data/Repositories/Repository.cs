using System.Linq.Expressions;
using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BlackSmith.Data.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly AppDbContextFactory _context;

    public Repository(AppDbContextFactory context)
    {
        _context = context;
    }

    public async Task<T> Add(T entity)
    {
        await using var context = _context.CreateDbContext();
        context.Set<T>().Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Delete(T entity)
    {
        await using var context = _context.CreateDbContext();
        context.Set<T>().Remove(entity);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        await using var context = _context.CreateDbContext();
        return await context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includes)
    {
        await using var context = _context.CreateDbContext();
        IQueryable<T> query = context.Set<T>();
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.ToListAsync();
    }

    public async Task<T?> Get(Expression<Func<T, bool>> predicate)
    {
        await using var context = _context.CreateDbContext();
        return await context.Set<T>().Where(predicate).FirstOrDefaultAsync();
    }

    public async Task<T?> GetById(int id)
    {
        await using var context = _context.CreateDbContext();
        return await context.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetById(int id, params Expression<Func<T, object>>[] includes)
    {
        await using var context = _context.CreateDbContext();
        IQueryable<T> query = context.Set<T>();
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<T> Update(T entity)
    {
        await using var context = _context.CreateDbContext();
        context.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}