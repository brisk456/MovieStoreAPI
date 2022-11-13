using MoviesStore.Domain.Models;

namespace MoviesStore.Domain.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        new Task<List<Movie>> GetAll();
        new Task<Movie> GetById(int id);
        Task<IEnumerable<Movie>> GetMovieByCategory(int categoryId);
        Task<IEnumerable<Movie>> SearchMovieWithCategory(string searchedValue);
    }
}
