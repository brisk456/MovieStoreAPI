using MoviesStore.Domain.Interfaces;
using MoviesStore.Domain.Models;

namespace MoviesStore.Domain.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            return await _movieRepository.GetAll();
        }

        public async Task<Movie> GetById(int id)
        {
            return await _movieRepository.GetById(id);
        }

        public async Task<Movie> Add(Movie movie)
        {
            if (_movieRepository.Search(x => x.Title == movie.Title).Result.Any())
            {
                return null;
            }

            await _movieRepository.Add(movie);
            return movie;
        }

        public async Task<Movie> Update(Movie movie)
        {
            if (_movieRepository.Search(x => x.Title == movie.Title && x.Id != movie.Id).Result.Any())
            {
                return null;
            }

            await _movieRepository.Update(movie);
            return movie;
        }

        public async Task<bool> Remove(Movie movie)
        {
            await _movieRepository.Remove(movie);
            return true;
        }

        public async Task<IEnumerable<Movie>> GetMovieByCategory(int categoryId)
        {
            return await _movieRepository.GetMovieByCategory(categoryId);
        }

        public async Task<IEnumerable<Movie>> Search(string movieTitle)
        {
            return await _movieRepository.Search(x => x.Title.Contains(movieTitle));
        }

        public async Task<IEnumerable<Movie>> SearchMovieWithCategory(string movieTitle)
        {
            return await _movieRepository.SearchMovieWithCategory(movieTitle);
        }

        public void Dispose()
        {
            _movieRepository.Dispose();
        }
    }
}
