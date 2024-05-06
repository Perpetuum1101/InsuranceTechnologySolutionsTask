using Application.DTOs;
using Application.Repository.Abstraction;
using Application.Repository.Contracts;
using Application.Service.Abstraction;
using Application.Service.Contracts;
using AutoMapper;
using Domain.Entities;
using FluentValidation;

namespace Application.Service;

public class ClaimService(
       IUnitOfWork unitOfWork,
       IClaimRepository repository,
       IValidator<ClaimDTO> validator,
       IMapper mapper) : 
       CrudService<Claim, ClaimDTO>(unitOfWork, repository, validator, mapper), IClaimService
{
}
