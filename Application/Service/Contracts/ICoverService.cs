using Application.DTOs;
using Application.Service.Abstraction;
using Domain.Entities;

namespace Application.Service.Contracts;

public interface ICoverService : ICrudService<Cover, CoverDTO>
{
    decimal ComputePremium(ComputePremiumDTO dto);
}
