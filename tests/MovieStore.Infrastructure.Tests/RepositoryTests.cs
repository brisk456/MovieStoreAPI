using Microsoft.EntityFrameworkCore;
using MoviesStore.Domain.Models;
using MoviesStore.Infrastructure.Context;
using MoviesStore.Infrastructure.Repositories;
using Xunit;

namespace MovieStore.Infrastructure.Tests
{
    public class RepositoryTests
    {
        private readonly DbContextOptions<MovieStoreDbContext> _options;

        public RepositoryTests()
        {
            _options = MovieStoreHelperTests.MovieStoreDbContextOptionsSQLiteInMemory();
            MovieStoreHelperTests.CreateDataBaseSQLiteInMemory(_options);
        }

        #region Test Data


        private Category CreateCategory()
        {
            return new Category()
            {
                Id = 4,
                Name = "Category Test 4",
            };
        }

        private List<Category> CreateCategoryList()
        {
            return new List<Category>()
            {
                new Category { Id = 1, Name = "Category Test 1" },
                new Category { Id = 2, Name = "Category Test 2" },
                new Category { Id = 3, Name = "Category Test 3" }
            };
        }

        #endregion

        #region GetAll

        [Fact]
        public async void GetAll_ShouldReturnAListOfCategory_WhenCategoriesExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var repository = new RepositoryConcreteClass(context);

                var categories = await repository.GetAll();

                Assert.NotNull(categories);
                Assert.IsType<List<Category>>(categories);
            }
        }

        [Fact]
        public async void GetAll_ShouldReturnAnEmptyList_WhenCategoriesDoNotExist()
        {
            await MovieStoreHelperTests.CleanDataBase(_options);

            await using (var context = new MovieStoreDbContext(_options))
            {
                var repository = new RepositoryConcreteClass(context);
                var categories = await repository.GetAll();

                Assert.NotNull(categories);
                Assert.Empty(categories);
                Assert.IsType<List<Category>>(categories);
            }
        }

        [Fact]
        public async void GetAll_ShouldReturnAListOfCategoryWithCorrectValues_WhenCategoriesExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var expectedCategories = CreateCategoryList();
                var repository = new RepositoryConcreteClass(context);
                var categoryList = await repository.GetAll();

                Assert.Equal(3, categoryList.Count);
                Assert.Equal(expectedCategories[0].Id, categoryList[0].Id);
                Assert.Equal(expectedCategories[0].Name, categoryList[0].Name);
                Assert.Equal(expectedCategories[1].Id, categoryList[1].Id);
                Assert.Equal(expectedCategories[1].Name, categoryList[1].Name);
                Assert.Equal(expectedCategories[2].Id, categoryList[2].Id);
                Assert.Equal(expectedCategories[2].Name, categoryList[2].Name);
            }
        }

        #endregion

        #region GetById

        [Fact]
        public async void GetById_ShouldReturnCategoryWithSearchedId_WhenCategoryWithSearchedIdExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var repository = new RepositoryConcreteClass(context);
                var category = await repository.GetById(2);

                Assert.NotNull(category);
                Assert.IsType<Category>(category);
            }
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenCategoryWithSearchedIdDoesNotExist()
        {
            await MovieStoreHelperTests.CleanDataBase(_options);

            await using (var context = new MovieStoreDbContext(_options))
            {
                var repository = new RepositoryConcreteClass(context);
                var category = await repository.GetById(1);

                Assert.Null(category);
            }
        }

        [Fact]
        public async void GetById_ShouldReturnCategoryWithCorrectValues_WhenCategoryExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var repository = new RepositoryConcreteClass(context);

                var expectedCategories = CreateCategoryList();
                var category = await repository.GetById(2);

                Assert.Equal(expectedCategories[1].Id, category.Id);
                Assert.Equal(expectedCategories[1].Name, category.Name);
            }
        }

        #endregion

        #region Category

        [Fact]
        public async void AddCategory_ShouldAddCategoryWithCorrectValues_WhenCategoryIsValid()
        {
            Category categoryToAdd = new Category();

            await using (var context = new MovieStoreDbContext(_options))
            {
                var repository = new RepositoryConcreteClass(context);
                categoryToAdd = CreateCategory();

                await repository.Add(categoryToAdd);
            }

            await using (var context = new MovieStoreDbContext(_options))
            {
                var categoryResult = await context.Categories.Where(b => b.Id == 4).FirstOrDefaultAsync();

                Assert.NotNull(categoryResult);
                Assert.IsType<Category>(categoryToAdd);
                Assert.Equal(categoryToAdd.Id, categoryResult.Id);
                Assert.Equal(categoryToAdd.Name, categoryResult.Name);
            }
        }

        [Fact]
        public async void UpdateCategory_ShouldUpdateCategoryWithCorrectValues_WhenCategoryIsValid()
        {
            Category categoryToUpdate = new Category();
            await using (var context = new MovieStoreDbContext(_options))
            {
                categoryToUpdate = await context.Categories.Where(b => b.Id == 1).FirstOrDefaultAsync();
                categoryToUpdate.Name = "Updated Name";
            }

            await using (var context = new MovieStoreDbContext(_options))
            {
                var repository = new RepositoryConcreteClass(context);
                await repository.Update(categoryToUpdate);
            }

            await using (var context = new MovieStoreDbContext(_options))
            {
                var updatedCategory = await context.Categories.Where(b => b.Id == 1).FirstOrDefaultAsync();

                Assert.NotNull(updatedCategory);
                Assert.IsType<Category>(updatedCategory);
                Assert.Equal(categoryToUpdate.Id, updatedCategory.Id);
                Assert.Equal(categoryToUpdate.Name, updatedCategory.Name);
            }
        }

        [Fact]
        public async void Remove_ShouldRemoveCategory_WhenCategoryIsValid()
        {
            Category categoryToRemove = new Category();

            await using (var context = new MovieStoreDbContext(_options))
            {
                categoryToRemove = await context.Categories.Where(x => x.Id == 2).FirstOrDefaultAsync();
            }

            await using (var context = new MovieStoreDbContext(_options))
            {
                var repository = new RepositoryConcreteClass(context);

                await repository.Remove(categoryToRemove);
            }

            await using (var context = new MovieStoreDbContext(_options))
            {
                var categoryRemoved = await context.Categories.Where(x => x.Id == 2).FirstOrDefaultAsync();

                Assert.Null(categoryRemoved);
            }
        }

        #endregion

    }
}

internal class RepositoryConcreteClass : Repository<Category>
{
    internal RepositoryConcreteClass(MovieStoreDbContext movieStoreDbContext) : base(movieStoreDbContext)
    {

    }
}