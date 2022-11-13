using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesStore.Domain.Interfaces;
using MoviesStore.Domain.Models;
using MovieStore.API.Dtos.Movie;

namespace MovieStore.API.Controllers
{
    [Route("api/[controller]")]
    public class MovieController : MainController
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;

        public MovieController(IMapper mapper, IMovieService movieService)
        {
            _mapper = mapper;
            _movieService = movieService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var movie = await _movieService.GetAll();

            return Ok(_mapper.Map<IEnumerable<MovieResultDto>>(movie));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _movieService.GetById(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MovieResultDto>(movie));
        }

        [HttpGet]
        [Route("get-movies-by-category/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMoviesByCategory(int categoryId)
        {
            var movies = await _movieService.GetMovieByCategory(categoryId);

            if (!movies.Any())
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<MovieResultDto>>(movies));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(MovieAddDto movieAddDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var movie = _mapper.Map<Movie>(movieAddDto);
            var movieResultt = await _movieService.Add(movie);

            if (movieResultt == null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<MovieResultDto>(movieResultt));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, MovieEditDto movieEditDto)
        {
            if (movieEditDto.Id != id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            await _movieService.Update(_mapper.Map<Movie>(movieEditDto));

            return Ok(movieEditDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Remove(int id)
        {
            var movie = await _movieService.GetById(id);

            if (movie == null)
            {
                return NotFound();
            }

            await _movieService.Remove(movie);

            return Ok();
        }

        [HttpGet]
        [Route("search/{movieTitle}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Movie>>> Search(string movieTitle)
        {
            var movies = _mapper.Map<List<Movie>>(await _movieService.Search(movieTitle));

            if (movies == null || movies.Count == 0)
            {
                return NotFound("None movie was founded");
            }

            return Ok(movies);
        }

        [HttpGet]
        [Route("search-movies-with-category/{searchedValue}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Movie>>> SearchMovieWithCategory(string searchedValue)
        {
            var movies = _mapper.Map<List<Movie>>(await _movieService.SearchMovieWithCategory(searchedValue));

            if (!movies.Any())
            {
                return NotFound("None movie was founded");
            }

            return Ok(_mapper.Map<IEnumerable<MovieResultDto>>(movies));
        }

    }
}
