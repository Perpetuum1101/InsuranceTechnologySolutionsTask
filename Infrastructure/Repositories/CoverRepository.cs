using Application.Repository.Contracts;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories;

internal class CoverRepository(ClaimsContext context) : 
               Repository<Cover>(context),
               ICoverRepository
{
}
