using Application.Repository.Abstraction;
using Application.Repository.Contracts;
using Application.Service.Abstraction;
using AutoMapper;
using FluentValidation;

namespace Claims.UnitTests.Utility;

public class TestService(
               IUnitOfWork unitOfWork,
               IRepository<TestEntity> repository,
               IValidator<TestDTO> validator,
               IMapper mapper) : 
               CrudService<TestEntity, TestDTO>(unitOfWork, repository, validator, mapper)
{
}
