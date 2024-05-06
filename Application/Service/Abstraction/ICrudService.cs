using Application.Result;
using Domain.Entities.Contracts;

namespace Application.Service.Abstraction;

public interface ICrudService<TEntity, TDTO> where TEntity : class, IHasId where TDTO : class
{
    Task<TDTO> Get(Guid id);
    Task<IEnumerable<TDTO>> GetAll();

    Task<Result<TDTO>> Create(TDTO dto);
    Task Delete(Guid id);
}
