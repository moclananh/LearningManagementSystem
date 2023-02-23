using Applications.ViewModels.ClassViewModels;
using AutoMapper;
using Domain.Entities;

namespace Infrastructures.Mappers
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<ClassViewModel, Class>().ReverseMap();
        }
    }
}
