using MoviesStore.Domain.Interfaces;
using MoviesStore.Domain.Services;
using MoviesStore.Infrastructure.Context;
using MoviesStore.Infrastructure.Repositories;

namespace MovieStore.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MovieStoreDbContext>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMovieService, MovieService>();

            return services;
        }
    }
}
