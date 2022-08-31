using BlackSmith.Domain.Interfaces;
using BlackSmith.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq.Expressions;

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
        query = includes.Aggregate(query, (current,
            include) => current.Include(include));
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAll(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        await using var context = _context.CreateDbContext();
        IQueryable<T> query = context.Set<T>();
        query = includes.Aggregate(query, (current,
            include) => current.Include(include));
        return await query.Where(predicate).ToListAsync();
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

    public async Task<T?> GetById(int id,
        params Expression<Func<T, object>>[] includes)
    {
        await using var context = _context.CreateDbContext();
        IQueryable<T> query = context.Set<T>();
        query = includes.Aggregate(query, (current,
            include) => current.Include(include));
        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<T> Update(T entity)
    {
        await using var context = _context.CreateDbContext();
        context.Set<T>();
        context.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(T entity,
        params Expression<Func<T, object>>[] includes)
    {
        await using var context = _context.CreateDbContext();

        var dbEntity = await context.FindAsync<T>(entity.Id) ??
                       throw new DbUpdateException("An error ocurred getting the provided entity");
        var dbEntry = context.Entry(dbEntity);

        dbEntry.CurrentValues.SetValues(entity);

        foreach (var property in includes)
        {
            if (IsPropertyTypeCollection(property))
            {
                var propertyName = property.GetPropertyAccess().Name;
                var dbItemsEntry = dbEntry.Collection(propertyName);
                var accessor = dbItemsEntry.Metadata.GetCollectionAccessor();

                if (accessor is null)
                    throw new DbUpdateException("An error ocurred getting the accessor to modify the collection");

                await dbItemsEntry.LoadAsync();

                if (dbItemsEntry.CurrentValue is null)
                    throw new DbUpdateException("An error ocurred getting the entity items current value");

                var dbItemsMap = ((IEnumerable<BaseEntity>)dbItemsEntry.CurrentValue).ToDictionary(e => e.Id);

                var items = (IEnumerable<BaseEntity>)accessor.GetOrCreate(entity, true);

                foreach (var item in items)
                {
                    var itemNotExists = dbItemsMap.TryGetValue(item.Id, out var oldItem) is false;

                    if (itemNotExists)
                    {
                        accessor.Add(dbEntity, item, true);
                    }
                    else
                    {
                        if (oldItem is null)
                            continue;
                        context.Entry(oldItem).CurrentValues.SetValues(item);
                        dbItemsMap.Remove(item.Id);
                    }
                }

                foreach (var oldItem in dbItemsMap.Values)
                    accessor.Remove(dbEntity, oldItem);
            }
            else
            {
                var propertyName = property.GetPropertyAccess().Name;
                var referenceEntry = dbEntry.Reference(propertyName);

                await referenceEntry.LoadAsync();

                var item = (BaseEntity)entity.GetType().GetProperty(propertyName)!.GetValue(entity)!;
                var dbChildEntity = (BaseEntity)referenceEntry.CurrentValue!;

                context.Entry(dbChildEntity).CurrentValues.SetValues(item);
            }
        }

        await context.SaveChangesAsync();

        return entity;
    }

    private static bool IsPropertyTypeCollection(Expression<Func<T, object>> property)
    {
        return property.GetPropertyAccess().PropertyType.IsGenericType &&
               typeof(ICollection<>).IsAssignableFrom(property.GetPropertyAccess().PropertyType
                   .GetGenericTypeDefinition());
    }

    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate)
    {
        await using var context = _context.CreateDbContext();
        return await context.Set<T>().Where(predicate).ToListAsync();
    }

}