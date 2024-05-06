using Application.Repository.Abstraction;
using Application.Repository.Contracts;
using Application.Result;
using AutoMapper;
using Domain.Entities.Contracts;
using FluentValidation;

namespace Application.Service.Abstraction;

public abstract class CrudService<TEntity, TDTO>(
                      IUnitOfWork unitOfWork,
                      IRepository<TEntity> repository,
                      IValidator<TDTO> validator,
                      IMapper mapper) : 
                      ICrudService<TEntity, TDTO> where TEntity : class, IHasId where TDTO : class
{
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;
    protected readonly IRepository<TEntity> _repository = repository;
    protected readonly IMapper _mapper = mapper;
    protected readonly IValidator<TDTO> _validator = validator;

    public virtual async Task<TDTO> Get(Guid id)
    {
        var result = await _repository.Get(id);
        var dto = _mapper.Map<TDTO>(result);

        return dto;
    }

    public virtual async Task<IEnumerable<TDTO>> GetAll()
    {
        var results = await _repository.GetAll();
        var dto = results.Select(_mapper.Map<TDTO>);

        return dto;
    }

    public virtual async Task<Result<TDTO>> Create(TDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            return Result<TDTO>.Failure(errors);
        }
        AfterValidation(dto);
        var entity = _mapper.Map<TEntity>(dto);
        _repository.Create(entity);
        await _unitOfWork.SaveChangesAsync();
        var response = _mapper.Map<TDTO>(entity);

        return Result<TDTO>.Success(response);
    }

    protected virtual void AfterValidation(TDTO dto) { }

    public async Task Delete(Guid id)
    {
        await _repository.Delete(id);
        await _unitOfWork.SaveChangesAsync();
    }
}