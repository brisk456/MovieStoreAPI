using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MoviesStore.Domain.Interfaces;
using MoviesStore.Domain.Models;
using MovieStore.API.Controllers;
using MovieStore.API.Dtos.Movie;
using Xunit;

namespace MovieStore.API.Tests
{
    public class MovieControllerTests
    {
        private readonly MovieController _moviesController;
        private readonly Mock<IMovieService> _movieServiceMock;
        private readonly Mock<IMapper> _mapperMock;

        public MovieControllerTests()
        {
            _movieServiceMock = new Mock<IMovieService>();
            _mapperMock = new Mock<IMapper>();
            _moviesController = new MovieController(_mapperMock.Object, _movieServiceMock.Object);
        }

        #region Test Data

        private Movie CreateMovie()
        {
            return new Movie()
            {
                Id = 2,
                Title = "Movie Test",
                Author = "Author Test",
                Description = "Description Test",
                CategoryId = 1,
                ReleaseDate = DateTime.MinValue.AddYears(40),
                Category = new Category()
                {
                    Id = 1,
                    Name = "Category Test"
                }
            };
        }

        private MovieResultDto MapModelToMovieResultDto(Movie movie)
        {
            var movieDto = new MovieResultDto()
            {
                Id = movie.Id,
                Title = movie.Title,
                Author = movie.Author,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                CategoryId = movie.CategoryId
            };
            return movieDto;
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

        private List<MovieResultDto> MapModelToMovieResultListDto(List<Movie> movies)
        {
            var listMovies = new List<MovieResultDto>();

            foreach (var item in movies)
            {
                var movie = new MovieResultDto()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Author = item.Author,
                    Description = item.Description,
                    ReleaseDate = item.ReleaseDate,
                    CategoryId = item.CategoryId
                };
                listMovies.Add(movie);
            }
            return listMovies;
        }

        #endregion

        #region GetAll

        [Fact]
        public async void GetAll_ShouldReturnOk_WhenExistMovies()
        {
            var movies = CreateMovieList();
            var dtoExpected = MapModelToMovieResultListDto(movies);

            _movieServiceMock.Setup(x => x.GetAll()).ReturnsAsync(movies);
            _mapperMock.Setup(x => x.Map<IEnumerable<MovieResultDto>>(It.IsAny<List<Movie>>())).Returns(dtoExpected);

            var result = await _moviesController.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnOk_WhenDoesNotExistAnyMovie()
        {
            var movies = new List<Movie>();
            var dtoExpected = MapModelToMovieResultListDto(movies);

            _movieServiceMock.Setup(x => x.GetAll()).ReturnsAsync(movies);
            _mapperMock.Setup(x => x.Map<IEnumerable<MovieResultDto>>(It.IsAny<List<Movie>>())).Returns(dtoExpected);

            var result = await _moviesController.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAll_ShouldCallGetAllFromService_OnlyOnce()
        {
            var movies = CreateMovieList();
            var dtoExpected = MapModelToMovieResultListDto(movies);

            _movieServiceMock.Setup(x => x.GetAll()).ReturnsAsync(movies);
            _mapperMock.Setup(x => x.Map<IEnumerable<MovieResultDto>>(It.IsAny<List<Movie>>())).Returns(dtoExpected);

            await _moviesController.GetAll();

            _movieServiceMock.Verify(mock => mock.GetAll(), Times.Once);
        }

        #endregion

        #region GetById

        [Fact]
        public async void GetById_ShouldReturnOk_WhenMovieExist()
        {
            var movie = CreateMovie();
            var dtoExpected = MapModelToMovieResultDto(movie);

            _movieServiceMock.Setup(x => x.GetById(2)).ReturnsAsync(movie);
            _mapperMock.Setup(x => x.Map<MovieResultDto>(It.IsAny<Movie>())).Returns(dtoExpected);

            var result = await _moviesController.GetById(2);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnNotFound_WhenMovieDoesNotExist()
        {
            _movieServiceMock.Setup(x => x.GetById(2)).ReturnsAsync((Movie)null);

            var result = await _moviesController.GetById(2);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetById_ShouldCallGetByIdFromService_OnlyOnce()
        {
            var movie = CreateMovie();
            var dtoExpected = MapModelToMovieResultDto(movie);

            _movieServiceMock.Setup(x => x.GetById(2)).ReturnsAsync(movie);
            _mapperMock.Setup(x => x.Map<MovieResultDto>(It.IsAny<Movie>())).Returns(dtoExpected);

            await _moviesController.GetById(2);

            _movieServiceMock.Verify(mock => mock.GetById(2), Times.Once);
        }

        #endregion

        #region GetMoviesByCategory

        [Fact]
        public async void GetMoviesByCategory_ShouldReturnOk_WhenMovieWithSearchedCategoryExist()
        {
            var movieList = CreateMovieList();
            var movie = CreateMovie();
            var dtoExpected = MapModelToMovieResultListDto(movieList);

            _movieServiceMock.Setup(x => x.GetMovieByCategory(movie.CategoryId)).ReturnsAsync(movieList);
            _mapperMock.Setup(x => x.Map<IEnumerable<MovieResultDto>>(It.IsAny<IEnumerable<Movie>>())).Returns(dtoExpected);

            var result = await _moviesController.GetMoviesByCategory(movie.CategoryId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetMoviesByCategory_ShouldReturnNotFound_WhenMovieWithSearchedCategoryDoesNotExist()
        {
            var movie = CreateMovie();
            var dtoExpected = MapModelToMovieResultDto(movie);

            _movieServiceMock.Setup(x => x.GetMovieByCategory(movie.CategoryId)).ReturnsAsync(new List<Movie>());
            _mapperMock.Setup(x => x.Map<MovieResultDto>(It.IsAny<Movie>())).Returns(dtoExpected);

            var result = await _moviesController.GetMoviesByCategory(movie.CategoryId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetMoviesByCategory_ShouldCallGetMoviesByCategoryFromService_OnlyOnce()
        {
            var movieList = CreateMovieList();
            var movie = CreateMovie();
            var dtoExpected = MapModelToMovieResultListDto(movieList);

            _movieServiceMock.Setup(x => x.GetMovieByCategory(movie.CategoryId)).ReturnsAsync(movieList);
            _mapperMock.Setup(x => x.Map<IEnumerable<MovieResultDto>>(It.IsAny<IEnumerable<Movie>>())).Returns(dtoExpected);

            await _moviesController.GetMoviesByCategory(movie.CategoryId);

            _movieServiceMock.Verify(mock => mock.GetMovieByCategory(movie.CategoryId), Times.Once);
        }

        #endregion

        #region Add

        [Fact]
        public async void Add_ShouldReturnOk_WhenMovieIsAdded()
        {
            var movie = CreateMovie();
            var movieAddDto = new MovieAddDto() { Title = movie.Title };
            var movieResultDto = MapModelToMovieResultDto(movie);

            _mapperMock.Setup(x => x.Map<Movie>(It.IsAny<MovieAddDto>())).Returns(movie);
            _movieServiceMock.Setup(x => x.Add(movie)).ReturnsAsync(movie);
            _mapperMock.Setup(x => x.Map<MovieResultDto>(It.IsAny<Movie>())).Returns(movieResultDto);

            var result = await _moviesController.Add(movieAddDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Add_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            var movieAddDto = new MovieAddDto();
            _moviesController.ModelState.AddModelError("Title", "The field Title is required");

            var result = await _moviesController.Add(movieAddDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Add_ShouldReturnBadRequest_WhenMovieResultIsNull()
        {
            var movie = CreateMovie();
            var movieAddDto = new MovieAddDto() { Title = movie.Title };

            _mapperMock.Setup(x => x.Map<Movie>(It.IsAny<MovieAddDto>())).Returns(movie);
            _movieServiceMock.Setup(x => x.Add(movie)).ReturnsAsync((Movie)null);

            var result = await _moviesController.Add(movieAddDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Add_ShouldCallAddFromService_OnlyOnce()
        {
            var movie = CreateMovie();
            var movieAddDto = new MovieAddDto() { Title = movie.Title };

            _mapperMock.Setup(x => x.Map<Movie>(It.IsAny<MovieAddDto>())).Returns(movie);
            _movieServiceMock.Setup(x => x.Add(movie)).ReturnsAsync(movie);

            await _moviesController.Add(movieAddDto);

            _movieServiceMock.Verify(mock => mock.Add(movie), Times.Once);
        }

        #endregion

        #region Update

        [Fact]
        public async void Update_ShouldReturnOk_WhenMovieIsUpdatedCorrectly()
        {
            var movie = CreateMovie();
            var movieEditDto = new MovieEditDto() { Id = movie.Id, Title = "Test" };

            _mapperMock.Setup(x => x.Map<Movie>(It.IsAny<MovieEditDto>())).Returns(movie);
            _movieServiceMock.Setup(x => x.GetById(movie.Id)).ReturnsAsync(movie);
            _movieServiceMock.Setup(x => x.Update(movie)).ReturnsAsync(movie);

            var result = await _moviesController.Update(movieEditDto.Id, movieEditDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Update_ShouldReturnBadRequest_WhenMovieIdIsDifferentThenParameterId()
        {
            var movieEditDto = new MovieEditDto() { Id = 1, Title = "Test" };

            var result = await _moviesController.Update(2, movieEditDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Update_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            var movieEditDto = new MovieEditDto() { Id = 1 };
            _moviesController.ModelState.AddModelError("Title", "The field Title is required");

            var result = await _moviesController.Update(1, movieEditDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void Update_ShouldCallUpdateFromService_OnlyOnce()
        {
            var movie = CreateMovie();
            var movieEditDto = new MovieEditDto() { Id = movie.Id, Title = "Test" };

            _mapperMock.Setup(x => x.Map<Movie>(It.IsAny<MovieEditDto>())).Returns(movie);
            _movieServiceMock.Setup(x => x.GetById(movie.Id)).ReturnsAsync(movie);
            _movieServiceMock.Setup(x => x.Update(movie)).ReturnsAsync(movie);

            await _moviesController.Update(movieEditDto.Id, movieEditDto);

            _movieServiceMock.Verify(mock => mock.Update(movie), Times.Once);
        }

        #endregion

        #region Remove

        [Fact]
        public async void Remove_ShouldReturnOk_WhenMovieIsRemoved()
        {
            var movie = CreateMovie();
            _movieServiceMock.Setup(x => x.GetById(movie.Id)).ReturnsAsync(movie);
            _movieServiceMock.Setup(x => x.Remove(movie)).ReturnsAsync(true);

            var result = await _moviesController.Remove(movie.Id);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void Remove_ShouldReturnNotFound_WhenMovieDoesNotExist()
        {
            var movie = CreateMovie();
            _movieServiceMock.Setup(x => x.GetById(movie.Id)).ReturnsAsync((Movie)null);

            var result = await _moviesController.Remove(movie.Id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Remove_ShouldCallRemoveFromService_OnlyOnce()
        {
            var movie = CreateMovie();
            _movieServiceMock.Setup(x => x.GetById(movie.Id)).ReturnsAsync(movie);
            _movieServiceMock.Setup(x => x.Remove(movie)).ReturnsAsync(true);

            await _moviesController.Remove(movie.Id);

            _movieServiceMock.Verify(mock => mock.Remove(movie), Times.Once);
        }

        #endregion

        #region Search

        [Fact]
        public async void Search_ShouldReturnOk_WhenMovieWithSearchedNameExist()
        {
            var movieList = CreateMovieList();
            var movie = CreateMovie();

            _movieServiceMock.Setup(x => x.Search(movie.Title)).ReturnsAsync(movieList);
            _mapperMock.Setup(x => x.Map<List<Movie>>(It.IsAny<IEnumerable<Movie>>())).Returns(movieList);

            var result = await _moviesController.Search(movie.Title);
            var actual = (OkObjectResult)result.Result;

            Assert.NotNull(actual);
            Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async void Search_ShouldReturnNotFound_WhenMovieWithSearchedNameDoesNotExist()
        {
            var movie = CreateMovie();
            var movieList = new List<Movie>();

            var dtoExpected = MapModelToMovieResultDto(movie);
            movie.Title = dtoExpected.Title;

            _movieServiceMock.Setup(x => x.Search(movie.Title)).ReturnsAsync(movieList);
            _mapperMock.Setup(x => x.Map<IEnumerable<Movie>>(It.IsAny<Movie>())).Returns(movieList);

            var result = await _moviesController.Search(movie.Title);
            var actual = (NotFoundObjectResult)result.Result;

            Assert.NotNull(actual);
            Assert.IsType<NotFoundObjectResult>(actual);
        }

        [Fact]
        public async void Search_ShouldCallSearchFromService_OnlyOnce()
        {
            var movieList = CreateMovieList();
            var movie = CreateMovie();

            _movieServiceMock.Setup(x => x.Search(movie.Title)).ReturnsAsync(movieList);
            _mapperMock.Setup(x => x.Map<List<Movie>>(It.IsAny<IEnumerable<Movie>>())).Returns(movieList);

            await _moviesController.Search(movie.Title);

            _movieServiceMock.Verify(mock => mock.Search(movie.Title), Times.Once);
        }

        #endregion

        #region SearchMovieWithCategory

        [Fact]
        public async void SearchMovieWithCategory_ShouldReturnOk_WhenMovieWithSearchedValueExist()
        {
            var movieList = CreateMovieList();
            var movie = CreateMovie();
            var movieResultList = MapModelToMovieResultListDto(movieList);

            _movieServiceMock.Setup(x => x.SearchMovieWithCategory(movie.Title)).ReturnsAsync(movieList);
            _mapperMock.Setup(x => x.Map<IEnumerable<Movie>>(It.IsAny<List<Movie>>())).Returns(movieList);
            _mapperMock.Setup(x => x.Map<IEnumerable<MovieResultDto>>(It.IsAny<List<Movie>>())).Returns(movieResultList);

            var result = await _moviesController.SearchMovieWithCategory(movie.Title);
            var actual = (OkObjectResult)result.Result;

            Assert.NotNull(actual);
            Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async void SearchMovieWithCategory_ShouldReturnNotFound_WhenMovieWithSearchedValueDoesNotExist()
        {
            var movie = CreateMovie();
            var movieList = new List<Movie>();

            _movieServiceMock.Setup(x => x.SearchMovieWithCategory(movie.Title)).ReturnsAsync(movieList);
            _mapperMock.Setup(x => x.Map<IEnumerable<Movie>>(It.IsAny<List<Movie>>())).Returns(movieList);

            var result = await _moviesController.SearchMovieWithCategory(movie.Title);
            var actual = (NotFoundObjectResult)result.Result;

            Assert.Equal("None movie was founded", actual.Value);
            Assert.IsType<NotFoundObjectResult>(actual);
        }

        [Fact]
        public async void SearchMovieWithCategory_ShouldCallSearchMovieWithCategoryFromService_OnlyOnce()
        {
            var movieList = CreateMovieList();
            var movie = CreateMovie();
            var movieResultList = MapModelToMovieResultListDto(movieList);

            _movieServiceMock.Setup(x => x.SearchMovieWithCategory(movie.Title)).ReturnsAsync(movieList);
            _mapperMock.Setup(x => x.Map<IEnumerable<Movie>>(It.IsAny<List<Movie>>())).Returns(movieList);
            _mapperMock.Setup(x => x.Map<IEnumerable<MovieResultDto>>(It.IsAny<List<Movie>>())).Returns(movieResultList);

            await _moviesController.SearchMovieWithCategory(movie.Title);

            _movieServiceMock.Verify(mock => mock.SearchMovieWithCategory(movie.Title), Times.Once);
        }

        #endregion

    }
}
