using Application.DTOs;
using Application.Service.Abstraction;
using Domain.Entities;

namespace Application.Service.Contracts;

public interface IClaimService : ICrudService<Claim, ClaimDTO> { }