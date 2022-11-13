using Moq;
using MoviesStore.Domain.Interfaces;
using MoviesStore.Domain.Models;
using MoviesStore.Domain.Services;
using Xunit;

namespace MovieStore.Domain.Tests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IMovieService> _movieService;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _movieService = new Mock<IMovieService>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object, _movieService.Object);
        }

        #region Test Data

        private Category CreateCategory()
        {
            return new Category()
            {
                Id = 1,
                Name = "Category Name 1"
            };
        }

        private List<Category> CreateCategoryList()
        {
            return new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Category Name 1"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Category Name 2"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Category Name 3"
                }
            };
        }

        #endregion

        #region GetAll

        [Fact]
        public async void GetAll_ShouldReturnAListOfCategories_WhenCategoriesExist()
        {
            var categories = CreateCategoryList();

            _categoryRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(categories);

            var result = await _categoryService.GetAll();

            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnNull_WhenCategoriesDoNotExist()
        {
            _categoryRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync((List<Category>)null);

            var result = await _categoryService.GetAll();

            Assert.Null(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromRepository_OnlyOnce()
        {
            _categoryRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync((List<Category>)null);

            await _categoryService.GetAll();

            _categoryRepositoryMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        #endregion

        #region GetById

        [Fact]
        public async void GetById_ShouldReturnCategory_WhenCategoryExist()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(x => x.GetById(category.Id)).ReturnsAsync(category);

            var result = await _categoryService.GetById(category.Id);

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            _categoryRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync((Category)null);

            var result = await _categoryService.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromRepository_OnlyOnce()
        {
            _categoryRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync((Category)null);

            await _categoryService.GetById(1);

            _categoryRepositoryMock.Verify(mock => mock.GetById(1), Times.Once);
        }

        #endregion

        #region Add

        [Fact]
        public async void Add_ShouldAddCategory_WhenCategoryNameDoesNotExist()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(x => x.Search(x => x.Name == category.Name)).ReturnsAsync(new List<Category>());
            _categoryRepositoryMock.Setup(x => x.Add(category));

            var result = await _categoryService.Add(category);

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
        }

        [Fact]
        public async void Add_ShouldNotAddCategory_WhenCategoryNameAlreadyExist()
        {
            var category = CreateCategory();
            var categoryList = new List<Category>() { category };

            _categoryRepositoryMock.Setup(x => x.Search(x => x.Name == category.Name)).ReturnsAsync(categoryList);

            var result = await _categoryService.Add(category);

            Assert.Null(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromRepository_OnlyOnce()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(x => x.Search(x => x.Name == category.Name)).ReturnsAsync(new List<Category>());
            _categoryRepositoryMock.Setup(x => x.Add(category));

            await _categoryService.Add(category);

            _categoryRepositoryMock.Verify(mock => mock.Add(category), Times.Once);
        }

        #endregion

        #region Update

        [Fact]
        public async void Update_ShouldUpdateCategory_WhenCategoryNameDoesNotExist()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(x => x.Search(x => x.Name == category.Name && x.Id != category.Id)).ReturnsAsync(new List<Category>());
            _categoryRepositoryMock.Setup(x => x.Update(category));

            var result = await _categoryService.Update(category);

            Assert.NotNull(result);
            Assert.IsType<Category>(result);
        }

        [Fact]
        public async void Update_ShouldNotUpdateCategory_WhenCategoryDoesNotExist()
        {
            var category = CreateCategory();
            var categoryList = new List<Category>()
            {
                new Category()
                {
                    Id = 2,
                    Name = "Category Name 2"
                }
            };

            _categoryRepositoryMock.Setup(x => x.Search(x => x.Name == category.Name && x.Id != category.Id)).ReturnsAsync(categoryList);

            var result = await _categoryService.Update(category);

            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldCallUpdateFromRepository_OnlyOnce()
        {
            var category = CreateCategory();

            _categoryRepositoryMock.Setup(x => x.Search(x => x.Name == category.Name && x.Id != category.Id)).ReturnsAsync(new List<Category>());

            await _categoryService.Update(category);

            _categoryRepositoryMock.Verify(mock => mock.Update(category), Times.Once);
        }

        #endregion

        #region Remove

        [Fact]
        public async void Remove_ShouldRemoveCategory_WhenCategoryDoNotHaveRelatedMovie()
        {
            var category = CreateCategory();

            _movieService.Setup(x => x.GetMovieByCategory(category.Id)).ReturnsAsync(new List<Movie>());

            var result = await _categoryService.Remove(category);

            Assert.True(result);
        }

        [Fact]
        public async void Remove_ShouldNotRemoveCategory_WhenCategoryHasRelatedMovie()
        {
            var category = CreateCategory();

            var movies = new List<Movie>()
            {
                new Movie()
                {
                    Id = 1,
                    Title = "Test Title 1",
                    Author = "Test Author 1",
                    CategoryId = category.Id
                }
            };

            _movieService.Setup(x => x.GetMovieByCategory(category.Id)).ReturnsAsync(movies);

            var result = await _categoryService.Remove(category);

            Assert.False(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromRepository_OnlyOnce()
        {
            var category = CreateCategory();

            _movieService.Setup(x => x.GetMovieByCategory(category.Id)).ReturnsAsync(new List<Movie>());

            await _categoryService.Remove(category);

            _categoryRepositoryMock.Verify(mock => mock.Remove(category), Times.Once);
        }

        #endregion

        #region Search

        [Fact]
        public async void Search_ShouldReturnAListOfCategory_WhenCategoriesWithSearchedNameExist()
        {
            var categoryList = CreateCategoryList();
            var searchedCategory = CreateCategory();
            var categoryName = searchedCategory.Name;

            _categoryRepositoryMock.Setup(x => x.Search(x => x.Name.Contains(categoryName))).ReturnsAsync(categoryList);

            var result = await _categoryService.Search(searchedCategory.Name);

            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
        }

        [Fact]
        public async void Search_ShouldReturnNull_WhenCategoriesWithSearchedNameDoNotExist()
        {
            var searchedCategory = CreateCategory();
            var categoryName = searchedCategory.Name;

            _categoryRepositoryMock.Setup(x => x.Search(x => x.Name.Contains(categoryName))).ReturnsAsync((IEnumerable<Category>)(null));

            var result = await _categoryService.Search(searchedCategory.Name);

            Assert.Null(result);
        }

        [Fact]
        public async void Search_ShouldCallSearchFromRepository_OnlyOnce()
        {
            var categoryList = CreateCategoryList();
            var searchedCategory = CreateCategory();
            var categoryName = searchedCategory.Name;

            _categoryRepositoryMock.Setup(x => x.Search(x => x.Name.Contains(categoryName))).ReturnsAsync(categoryList);

            await _categoryService.Search(searchedCategory.Name);

            _categoryRepositoryMock.Verify(mock => mock.Search(x => x.Name.Contains(categoryName)), Times.Once);
        }

        #endregion

    }
}
