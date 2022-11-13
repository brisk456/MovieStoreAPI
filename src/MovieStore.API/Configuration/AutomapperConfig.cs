using AutoMapper;
using MoviesStore.Domain.Models;
using MovieStore.API.Dtos.Category;
using MovieStore.API.Dtos.Movie;

namespace MovieStore.API.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Category, CategoryAddDto>().ReverseMap();
            CreateMap<Category, CategoryEditDto>().ReverseMap();
            CreateMap<Category, CategoryResultDto>().ReverseMap();
            CreateMap<Movie, MovieAddDto>().ReverseMap();
            CreateMap<Movie, MovieEditDto>().ReverseMap();
            CreateMap<Movie, MovieResultDto>().ReverseMap();
        }
    }
}
