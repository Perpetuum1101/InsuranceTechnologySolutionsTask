using Application.Repository.Contracts;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories;

internal class ClaimRepository(ClaimsContext context) : 
               Repository<Claim>(context),
               IClaimRepository
{
}