using Application.Repository.Abstraction;
using Domain.Entities.Contracts;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Abstractions;

internal abstract class Repository<T>(ClaimsContext context) : IRepository<T> 
                        where T : class, IHasId
{
    protected readonly ClaimsContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public void Create(T entity)
    {
        _dbSet.Add(entity);
    }

    public async Task Delete(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if(entity == null)
        {
            return;
        }
        _dbSet.Remove(entity);
    }

    public async Task<T?> Get(Guid id)
    {
        var result = await _dbSet.FindAsync(id);
        return result;
    }

    public async Task<List<T>> GetAll()
    {
        var result = await _dbSet.ToListAsync();
        return result;
    }
}
