using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ClaimDTO, Claim>().ForMember(x => x.Id, c => c.Ignore());
        CreateMap<Claim, ClaimDTO>();
        CreateMap<CoverDTO, Cover>().ForMember(x => x.Id, c => c.Ignore());
        CreateMap<Cover, CoverDTO>();
    }
}
