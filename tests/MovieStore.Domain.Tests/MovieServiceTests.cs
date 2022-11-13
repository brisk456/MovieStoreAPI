using Moq;
using MoviesStore.Domain.Interfaces;
using MoviesStore.Domain.Models;
using MoviesStore.Domain.Services;
using Xunit;

namespace MovieStore.Domain.Tests
{
    public class MovieServiceTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _movieService = new MovieService(_movieRepositoryMock.Object);
        }


        #region Test Data

        private Movie CreateMovie()
        {
            return new Movie()
            {
                Id = 1,
                Title = "Movie Test",
                Author = "Author Test",
                Description = "Description Test",
                CategoryId = 1,
                ReleaseDate = DateTime.MinValue.AddYears(40)
            };
        }

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
                    CategoryId = 1
                },
                new Movie()
                {
                    Id = 2,
                    Title = "Movie Test 2",
                    Author = "Author Test 2",
                    Description = "Description Test 2",
                    CategoryId = 1
                },
                new Movie()
                {
                    Id = 3,
                    Title = "Movie Test 3",
                    Author = "Author Test 3",
                    Description = "Description Test 3",
                    CategoryId = 2
                }
            };
        }

        #endregion

        #region GetAll

        [Fact]
        public async void GetAll_ShouldReturnAListOfMovie_WhenMoviesExist()
        {
            var movies = CreateMovieList();

            _movieRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(movies);

            var result = await _movieService.GetAll();

            Assert.NotNull(result);
            Assert.IsType<List<Movie>>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnNull_WhenMoviesDoNotExist()
        {
            _movieRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync((List<Movie>)null);

            var result = await _movieService.GetAll();

            Assert.Null(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromRepository_OnlyOnce()
        {
            _movieRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Movie>());

            await _movieService.GetAll();

            _movieRepositoryMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        #endregion

        #region GetById

        [Fact]
        public async void GetById_ShouldReturnMovie_WhenMovieExist()
        {
            var movie = CreateMovie();

            _movieRepositoryMock.Setup(x => x.GetById(movie.Id)).ReturnsAsync(movie);

            var result = await _movieService.GetById(movie.Id);

            Assert.NotNull(result);
            Assert.IsType<Movie>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenMovieDoesNotExist()
        {
            _movieRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync((Movie)null);

            var result = await _movieService.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromRepository_OnlyOnce()
        {
            _movieRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(new Movie());

            await _movieService.GetById(1);

            _movieRepositoryMock.Verify(mock => mock.GetById(1), Times.Once);
        }

        #endregion

        #region GetMovieByCategory

        [Fact]
        public async void GetMoviesByCategory_ShouldReturnAListOfMovie_WhenMoviesWithSearchedCategoryExist()
        {
            var movieList = CreateMovieList();

            _movieRepositoryMock.Setup(x => x.GetMovieByCategory(2)).ReturnsAsync(movieList);

            var result = await _movieService.GetMovieByCategory(2);

            Assert.NotNull(result);
            Assert.IsType<List<Movie>>(result);
        }

        [Fact]
        public async void GetMoviesByCategory_ShouldReturnNull_WhenMoviesWithSearchedCategoryDoNotExist()
        {
            _movieRepositoryMock.Setup(x => x.GetMovieByCategory(2)).ReturnsAsync((IEnumerable<Movie>)null);

            var result = await _movieService.GetById(1);

            Assert.Null(result);
        }

        [Fact]
        public async void GetMoviesByCategory_ShouldCallGetMoviesByCategoryFromRepository_OnlyOnce()
        {
            var movieList = CreateMovieList();

            _movieRepositoryMock.Setup(x => x.GetMovieByCategory(2)).ReturnsAsync(movieList);

            await _movieService.GetMovieByCategory(2);

            _movieRepositoryMock.Verify(mock => mock.GetMovieByCategory(2), Times.Once);
        }

        #endregion

        #region Search 

        [Fact]
        public async void Search_ShouldReturnAListOfMovie_WhenMoviesWithSearchedNameExist()
        {
            var movieList = CreateMovieList();
            var searchedMovie = CreateMovie();
            var movieTitle = searchedMovie.Title;

            _movieRepositoryMock.Setup(x => x.Search(x => x.Title.Contains(movieTitle))).ReturnsAsync(movieList);

            var result = await _movieService.Search(searchedMovie.Title);

            Assert.NotNull(result);
            Assert.IsType<List<Movie>>(result);
        }

        [Fact]
        public async void Search_ShouldReturnNull_WhenMoviesWithSearchedNameDoNotExist()
        {
            var searchedMovie = CreateMovie();
            var movieTitle = searchedMovie.Title;

            _movieRepositoryMock.Setup(x => x.Search(x => x.Title.Contains(movieTitle))).ReturnsAsync((IEnumerable<Movie>)(null));

            var result = await _movieService.Search(searchedMovie.Title);

            Assert.Null(result);
        }

        [Fact]
        public async void Search_ShouldCallSearchFromRepository_OnlyOnce()
        {
            var movieList = CreateMovieList();
            var searchedMovie = CreateMovie();
            var movieTitle = searchedMovie.Title;

            _movieRepositoryMock.Setup(x => x.Search(x => x.Title.Contains(movieTitle))).ReturnsAsync(movieList);

            await _movieService.Search(searchedMovie.Title);

            _movieRepositoryMock.Verify(mock => mock.Search(x => x.Title.Contains(movieTitle)), Times.Once);
        }

        #endregion

        #region SearchMovieWithCategory

        [Fact]
        public async void SearchMovieWithCategory_ShouldReturnAListOfMovie_WhenMoviesWithSearchedCategoryExist()
        {
            var movieList = CreateMovieList();
            var searchedMovie = CreateMovie();

            _movieRepositoryMock.Setup(x => x.SearchMovieWithCategory(searchedMovie.Title)).ReturnsAsync(movieList);

            var result = await _movieService.SearchMovieWithCategory(searchedMovie.Title);

            Assert.NotNull(result);
            Assert.IsType<List<Movie>>(result);
        }

        [Fact]
        public async void SearchMovieWithCategory_ShouldReturnNull_WhenMoviesWithSearchedCategoryDoNotExist()
        {
            var searchedMovie = CreateMovie();

            _movieRepositoryMock.Setup(x => x.SearchMovieWithCategory(searchedMovie.Title)).ReturnsAsync((IEnumerable<Movie>)null);

            var result = await _movieService.SearchMovieWithCategory(searchedMovie.Title);

            Assert.Null(result);
        }

        [Fact]
        public async void SearchMovieWithCategory_ShouldCallSearchMovieWithCategoryFromRepository_OnlyOnce()
        {
            var movieList = CreateMovieList();
            var searchedMovie = CreateMovie();

            _movieRepositoryMock.Setup(x => x.SearchMovieWithCategory(searchedMovie.Title)).ReturnsAsync(movieList);

            await _movieService.SearchMovieWithCategory(searchedMovie.Title);

            _movieRepositoryMock.Verify(mock => mock.SearchMovieWithCategory(searchedMovie.Title), Times.Once);
        }

        #endregion

        #region Add

        [Fact]
        public async void Add_ShouldAddMovie_WhenMovieNameDoesNotExist()
        {
            var movie = CreateMovie();

            _movieRepositoryMock.Setup(x => x.Search(x => x.Title == movie.Title)).ReturnsAsync(new List<Movie>());
            _movieRepositoryMock.Setup(x => x.Add(movie));

            var result = await _movieService.Add(movie);

            Assert.NotNull(result);
            Assert.IsType<Movie>(result);
        }

        [Fact]
        public async void Add_ShouldNotAddMovie_WhenMovieNameAlreadyExist()
        {
            var movie = CreateMovie();
            var movieList = new List<Movie>() { movie };

            _movieRepositoryMock.Setup(x => x.Search(x => x.Title == movie.Title)).ReturnsAsync(movieList);

            var result = await _movieService.Add(movie);

            Assert.Null(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromRepository_OnlyOnce()
        {
            var movie = CreateMovie();

            _movieRepositoryMock.Setup(x => x.Search(x => x.Title == movie.Title)).ReturnsAsync(new List<Movie>());
            _movieRepositoryMock.Setup(x => x.Add(movie));

            await _movieService.Add(movie);

            _movieRepositoryMock.Verify(mock => mock.Add(movie), Times.Once);
        }

        #endregion

        #region Update

        [Fact]
        public async void Update_ShouldUpdateMovie_WhenMovieNameDoesNotExist()
        {
            var movie = CreateMovie();

            _movieRepositoryMock.Setup(x => x.Search(x => x.Title == movie.Title && x.Id != movie.Id)).ReturnsAsync(new List<Movie>());
            _movieRepositoryMock.Setup(x => x.Update(movie));

            var result = await _movieService.Update(movie);

            Assert.NotNull(result);
            Assert.IsType<Movie>(result);
        }

        [Fact]
        public async void Update_ShouldNotUpdateMovie_WhenMovieDoesNotExist()
        {
            var movie = CreateMovie();
            var movieList = new List<Movie>()
            {
                new Movie()
                {
                    Id = 2,
                    Title = "Movie Test 2",
                    Author = "Author Test 2"
                }
            };

            _movieRepositoryMock.Setup(x => x.Search(x => x.Title == movie.Title && x.Id != movie.Id)).ReturnsAsync(movieList);

            var result = await _movieService.Update(movie);

            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldCallAddFromRepository_OnlyOnce()
        {
            var movie = CreateMovie();

            _movieRepositoryMock.Setup(x => x.Search(x => x.Title == movie.Title && x.Id != movie.Id)).ReturnsAsync(new List<Movie>());

            await _movieService.Update(movie);

            _movieRepositoryMock.Verify(mock => mock.Update(movie), Times.Once);
        }

        #endregion

        #region Remove

        [Fact]
        public async void Remove_ShouldReturnTrue_WhenMovieCanBeRemoved()
        {
            var movie = CreateMovie();

            var result = await _movieService.Remove(movie);

            Assert.True(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromRepository_OnlyOnce()
        {
            var movie = CreateMovie();

            await _movieService.Remove(movie);

            _movieRepositoryMock.Verify(mock => mock.Remove(movie), Times.Once);
        }

        #endregion

    }
}
