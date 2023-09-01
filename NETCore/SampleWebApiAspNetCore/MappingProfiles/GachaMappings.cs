using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class GachaMappings : Profile
    {
        public GachaMappings()
        {
            CreateMap<GachaEntity, GachaDto>().ReverseMap();
            CreateMap<GachaEntity, GachaUpdateDto>().ReverseMap();
            CreateMap<GachaEntity, GachaCreateDto>().ReverseMap();
        }
    }
}
