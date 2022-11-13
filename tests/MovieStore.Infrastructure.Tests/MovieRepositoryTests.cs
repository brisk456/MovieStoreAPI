using Microsoft.EntityFrameworkCore;
using MoviesStore.Domain.Models;
using MoviesStore.Infrastructure.Context;
using MoviesStore.Infrastructure.Repositories;
using Xunit;

namespace MovieStore.Infrastructure.Tests
{
    public class MovieRepositoryTests
    {
        private readonly DbContextOptions<MovieStoreDbContext> _options;

        public MovieRepositoryTests()
        {
            // Use this when using a SQLite InMemory database
            _options = MovieStoreHelperTests.MovieStoreDbContextOptionsSQLiteInMemory();
            MovieStoreHelperTests.CreateDataBaseSQLiteInMemory(_options);

            // Use this when using a EF Core InMemory database
            //_options = MovieStoreHelperTests.MovieStoreDbContextOptionsEfCoreInMemory();
            //MovieStoreHelperTests.CreateDataBaseEfCoreInMemory(_options);
        }

        #region Test Data

        private List<Movie> CreateMovieList()
        {
            return new List<Movie>()
            {
                new Movie()
                {
                    Id = 1,
                    Title = "Movie Test 1",
                    Author = "Author Test 1",
                    Description = "Description Test 1",
                    CategoryId = 1,
                    ReleaseDate = new DateTime(2020, 1, 1, 0, 0, 0, 0),
                    Category = new Category()
                    {
                        Id = 1,
                        Name = "Category Test 1"
                    }
                },
                new Movie()
                {
                    Id = 2,
                    Title = "Movie Test 2",
                    Author = "Author Test 2",
                    Description = "Description Test 2",
                    CategoryId = 1,
                    ReleaseDate = new DateTime(2020, 2, 2, 0, 0, 0, 0),
                    Category = new Category()
                    {
                        Id = 1,
                        Name = "Category Test 1"
                    }
                },
                new Movie()
                {
                    Id = 3,
                    Title = "Movie Test 3",
                    Author = "Author Test 3",
                    Description = "Description Test 3",
                    CategoryId = 3,
                    ReleaseDate = new DateTime(2020, 3, 3, 0, 0, 0, 0),
                    Category = new Category()
                    {
                        Id = 3,
                        Name = "Category Test 3"
                    }
                }
            };
        }

        #endregion

        #region GetAll

        [Fact]
        public async void GetAll_ShouldReturnAListOfMovie_WhenMoviesExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var movieRepository = new MovieRepository(context);
                var movies = await movieRepository.GetAll();

                Assert.NotNull(movies);
                Assert.IsType<List<Movie>>(movies);
            }
        }

        [Fact]
        public async void GetAll_ShouldReturnAnEmptyList_WhenMoviesDoNotExist()
        {
            await MovieStoreHelperTests.CleanDataBase(_options);

            await using (var context = new MovieStoreDbContext(_options))
            {
                var movieRepository = new MovieRepository(context);
                var movies = await movieRepository.GetAll();

                Assert.NotNull(movies);
                Assert.Empty(movies);
                Assert.IsType<List<Movie>>(movies);
            }
        }

        [Fact]
        public async void GetAll_ShouldReturnAListOfMovieWithCorrectValues_WhenMoviesExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var movieRepository = new MovieRepository(context);

                var expectedMovies = CreateMovieList();
                var movieList = await movieRepository.GetAll();

                Assert.Equal(3, movieList.Count);
                Assert.Equal(expectedMovies[0].Id, movieList[0].Id);
                Assert.Equal(expectedMovies[0].Title, movieList[0].Title);
                Assert.Equal(expectedMovies[0].Description, movieList[0].Description);
                Assert.Equal(expectedMovies[0].CategoryId, movieList[0].CategoryId);
                Assert.Equal(expectedMovies[0].ReleaseDate, movieList[0].ReleaseDate);
                Assert.Equal(expectedMovies[0].Category.Id, movieList[0].Category.Id);
                Assert.Equal(expectedMovies[0].Category.Name, movieList[0].Category.Name);

                Assert.Equal(expectedMovies[1].Id, movieList[1].Id);
                Assert.Equal(expectedMovies[1].Title, movieList[1].Title);
                Assert.Equal(expectedMovies[1].Description, movieList[1].Description);
                Assert.Equal(expectedMovies[1].CategoryId, movieList[1].CategoryId);
                Assert.Equal(expectedMovies[1].ReleaseDate, movieList[1].ReleaseDate);
                Assert.Equal(expectedMovies[1].Category.Id, movieList[1].Category.Id);
                Assert.Equal(expectedMovies[1].Category.Name, movieList[1].Category.Name);

                Assert.Equal(expectedMovies[2].Id, movieList[2].Id);
                Assert.Equal(expectedMovies[2].Title, movieList[2].Title);
                Assert.Equal(expectedMovies[2].Description, movieList[2].Description);
                Assert.Equal(expectedMovies[2].CategoryId, movieList[2].CategoryId);
                Assert.Equal(expectedMovies[2].ReleaseDate, movieList[2].ReleaseDate);
                Assert.Equal(expectedMovies[2].Category.Id, movieList[2].Category.Id);
                Assert.Equal(expectedMovies[2].Category.Name, movieList[2].Category.Name);
            }
        }

        #endregion

        #region GetById

        [Fact]
        public async void GetById_ShouldReturnMovieWithSearchedId_WhenMovieWithSearchedIdExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var movieRepository = new MovieRepository(context);

                var movie = await movieRepository.GetById(2);

                Assert.NotNull(movie);
                Assert.IsType<Movie>(movie);
            }
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenMovieWithSearchedIdDoesNotExist()
        {
            await MovieStoreHelperTests.CleanDataBase(_options);

            await using (var context = new MovieStoreDbContext(_options))
            {
                var movieRepository = new MovieRepository(context);
                var movie = await movieRepository.GetById(1);

                Assert.Null(movie);
            }
        }

        [Fact]
        public async void GetById_ShouldReturnMovieWithCorrectValues_WhenMovieExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var movieRepository = new MovieRepository(context);

                var expectedMovies = CreateMovieList();
                var movie = await movieRepository.GetById(2);

                Assert.Equal(expectedMovies[1].Id, movie.Id);
                Assert.Equal(expectedMovies[1].Title, movie.Title);
                Assert.Equal(expectedMovies[1].Description, movie.Description);
                Assert.Equal(expectedMovies[1].CategoryId, movie.CategoryId);
                Assert.Equal(expectedMovies[1].ReleaseDate, movie.ReleaseDate);
                Assert.Equal(expectedMovies[1].Category.Id, movie.Category.Id);
                Assert.Equal(expectedMovies[1].Category.Name, movie.Category.Name);
            }
        }

        #endregion

        #region GetMovieByCategory

        [Fact]
        public async void GetMoviesByCategory_ShouldReturnAListOfMovie_WhenMoviesWithSearchedCategoryExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var movieRepository = new MovieRepository(context);

                var movies = await movieRepository.GetMovieByCategory(1);

                Assert.NotNull(movies);
                Assert.IsType<List<Movie>>(movies);
            }
        }

        [Fact]
        public async void GetMoviesByCategory_ShouldReturnAnEmptyList_WhenNoMoviesWithSearchedCategoryExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var movieRepository = new MovieRepository(context);
                var movies = await movieRepository.GetMovieByCategory(4);
                var movieList = movies as List<Movie>;

                Assert.NotNull(movieList);
                Assert.Empty(movieList);
                Assert.IsType<List<Movie>>(movieList);
            }
        }

        [Fact]
        public async void GetMoviesByCategory_ShouldReturnAListOfMovieWithSearchedCategory_WhenMoviesWithSearchedCategoryExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var movieRepository = new MovieRepository(context);

                var expectedMovies = CreateMovieList();
                var movies = await movieRepository.GetMovieByCategory(1);
                var movieList = movies as List<Movie>;

                Assert.NotNull(movieList);
                Assert.Equal(2, movieList.Count);

                Assert.Equal(expectedMovies[0].Id, movieList[0].Id);
                Assert.Equal(expectedMovies[0].Title, movieList[0].Title);
                Assert.Equal(expectedMovies[0].Description, movieList[0].Description);
                Assert.Equal(expectedMovies[0].CategoryId, movieList[0].CategoryId);
                Assert.Equal(expectedMovies[0].ReleaseDate, movieList[0].ReleaseDate);
                Assert.Equal(expectedMovies[1].Id, movieList[1].Id);
                Assert.Equal(expectedMovies[1].Title, movieList[1].Title);
                Assert.Equal(expectedMovies[1].Description, movieList[1].Description);
                Assert.Equal(expectedMovies[1].CategoryId, movieList[1].CategoryId);
                Assert.Equal(expectedMovies[1].ReleaseDate, movieList[1].ReleaseDate);
            }
        }

        #endregion

        #region SearchMovieWithCategory

        [Fact]
        public async void SearchMovieWithCategory_ShouldReturnOneMovie_WhenOneMovieWithSearchedValueExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var movieRepository = new MovieRepository(context);
                var expectedMovie = CreateMovieList();
                var movies = await movieRepository.SearchMovieWithCategory(expectedMovie[1].Title);
                var movieList = movies as List<Movie>;

                Assert.NotNull(movieList);
                Assert.IsType<List<Movie>>(movieList);
                Assert.Single(movieList);
                Assert.Equal(expectedMovie[1].Id, movieList[0].Id);
                Assert.Equal(expectedMovie[1].Title, movieList[0].Title);
                Assert.Equal(expectedMovie[1].Author, movieList[0].Author);
                Assert.Equal(expectedMovie[1].Description, movieList[0].Description);
                Assert.Equal(expectedMovie[1].CategoryId, movieList[0].CategoryId);
                Assert.Equal(expectedMovie[1].ReleaseDate, movieList[0].ReleaseDate);
            }
        }

        [Fact]
        public async void SearchMovieWithCategory_ShouldReturnAListOfMovie_WhenMovieWithSearchedValueExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var movieRepository = new MovieRepository(context);
                var expectedMovies = CreateMovieList();
                var movies = await movieRepository.SearchMovieWithCategory("Movie Test");
                var movieList = movies as List<Movie>;

                Assert.NotNull(movieList);
                Assert.IsType<List<Movie>>(movieList);
                Assert.Equal(expectedMovies.Count, movieList.Count);
                Assert.Equal(expectedMovies[0].Id, movieList[0].Id);
                Assert.Equal(expectedMovies[0].Title, movieList[0].Title);
                Assert.Equal(expectedMovies[0].Description, movieList[0].Description);
                Assert.Equal(expectedMovies[0].CategoryId, movieList[0].CategoryId);
                Assert.Equal(expectedMovies[0].ReleaseDate, movieList[0].ReleaseDate);
                Assert.Equal(expectedMovies[0].Category.Id, movieList[0].Category.Id);
                Assert.Equal(expectedMovies[0].Category.Name, movieList[0].Category.Name);

                Assert.Equal(expectedMovies[1].Id, movieList[1].Id);
                Assert.Equal(expectedMovies[1].Title, movieList[1].Title);
                Assert.Equal(expectedMovies[1].Description, movieList[1].Description);
                Assert.Equal(expectedMovies[1].CategoryId, movieList[1].CategoryId);
                Assert.Equal(expectedMovies[1].ReleaseDate, movieList[1].ReleaseDate);
                Assert.Equal(expectedMovies[1].Category.Id, movieList[1].Category.Id);
                Assert.Equal(expectedMovies[1].Category.Name, movieList[1].Category.Name);

                Assert.Equal(expectedMovies[2].Id, movieList[2].Id);
                Assert.Equal(expectedMovies[2].Title, movieList[2].Title);
                Assert.Equal(expectedMovies[2].Description, movieList[2].Description);
                Assert.Equal(expectedMovies[2].CategoryId, movieList[2].CategoryId);
                Assert.Equal(expectedMovies[2].ReleaseDate, movieList[2].ReleaseDate);
                Assert.Equal(expectedMovies[2].Category.Id, movieList[2].Category.Id);
                Assert.Equal(expectedMovies[2].Category.Name, movieList[2].Category.Name);
            }
        }

        [Fact]
        public async void SearchMovieWithCategory_ShouldReturnAnEmptyList_WhenNoMoviesWithSearchedValueExist()
        {
            await using (var context = new MovieStoreDbContext(_options))
            {
                var movieRepository = new MovieRepository(context);
                var movies = await movieRepository.SearchMovieWithCategory("Testt");
                var movieList = movies as List<Movie>;

                Assert.NotNull(movieList);
                Assert.Empty(movieList);
                Assert.IsType<List<Movie>>(movieList);
            }
        }

        #endregion

    }
}
