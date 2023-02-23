using Application.ViewModels.QuizzViewModels;
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
            CreateMap<CreateQuizzViewModel, Quizz>().ReverseMap();
            CreateMap<QuizzViewModel, Quizz>().ReverseMap();
            CreateMap<UpdateQuizzViewModel, Quizz>().ReverseMap();
            CreateMap<Quizz, QuizzViewModel>()
               .ForMember(dest => dest.QuizzId, src => src.MapFrom(x => x.Id));
        }
    }
}
