using MoviesStore.Domain.Interfaces;
using MoviesStore.Domain.Models;

namespace MoviesStore.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMovieService _movieService;

        public CategoryService(ICategoryRepository categoryRepository, IMovieService movieService)
        {
            _categoryRepository = categoryRepository;
            _movieService = movieService;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Category> GetById(int id)
        {
            return await _categoryRepository.GetById(id);
        }

        public async Task<Category> Add(Category category)
        {
            if (_categoryRepository.Search(x => x.Name == category.Name).Result.Any())
            {
                return null;
            }

            await _categoryRepository.Add(category);
            return category;
        }

        public async Task<Category> Update(Category category)
        {
            if (_categoryRepository.Search(x => x.Name == category.Name && x.Id != category.Id).Result.Any())
            {
                return null;
            }

            await _categoryRepository.Update(category);
            return category;
        }

        public async Task<bool> Remove(Category category)
        {
            var movies = await _movieService.GetMovieByCategory(category.Id);
            if (movies.Any())
            {
                return false;
            }

            await _categoryRepository.Remove(category);
            return true;
        }

        public async Task<IEnumerable<Category>> Search(string categorName)
        {
            return await _categoryRepository.Search(x => x.Name.Contains(categorName));
        }

        public void Dispose()
        {
            _categoryRepository?.Dispose();
        }

    }
}
