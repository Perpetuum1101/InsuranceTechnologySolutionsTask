using Domain.Entities.Contracts;

namespace Application.Repository.Abstraction;

public interface IRepository<T> where T : IHasId
{
    public Task<T?> Get(Guid id);
    public Task<List<T>> GetAll();
    public Task Delete(Guid id);
    public void Create(T entity);
}
