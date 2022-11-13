using Microsoft.EntityFrameworkCore;
using MoviesStore.Domain.Interfaces;
using MoviesStore.Domain.Models;
using MoviesStore.Infrastructure.Context;

namespace MoviesStore.Infrastructure.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieStoreDbContext context) : base(context) { }

        public override async Task<List<Movie>> GetAll()
        {
            return await Db.Movies.AsNoTracking().Include(x => x.Category)
                .OrderBy(x => x.Title)
                .ToListAsync();
        }

        public override async Task<Movie> GetById(int id)
        {
            return await Db.Movies.AsNoTracking().Include(x => x.Category)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Movie>> GetMovieByCategory(int categoryId)
        {
            return await Search(x => x.CategoryId == categoryId);
        }

        public async Task<IEnumerable<Movie>> SearchMovieWithCategory(string searchedValue)
        {
            return await Db.Movies.AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.Title.Contains(searchedValue) ||
                            x.Author.Contains(searchedValue) ||
                            x.Description.Contains(searchedValue) ||
                            x.Category.Name.Contains(searchedValue))
                .ToListAsync();
        }
    }
}
