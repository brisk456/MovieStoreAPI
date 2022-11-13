using MoviesStore.Domain.Models;

namespace MoviesStore.Domain.Interfaces
{
    public interface IMovieService : IDisposable
    {
        Task<IEnumerable<Movie>> GetAll();
        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie movie);
        Task<Movie> Update(Movie movie);
        Task<bool> Remove(Movie movie);
        Task<IEnumerable<Movie>> GetMovieByCategory(int categoryId);
        Task<IEnumerable<Movie>> Search(string movieName);
        Task<IEnumerable<Movie>> SearchMovieWithCategory(string searchedValue);
    }
}
