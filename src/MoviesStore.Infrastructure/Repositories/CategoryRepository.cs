using MoviesStore.Domain.Interfaces;
using MoviesStore.Domain.Models;
using MoviesStore.Infrastructure.Context;

namespace MoviesStore.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(MovieStoreDbContext context) : base(context) { }
    }
}
