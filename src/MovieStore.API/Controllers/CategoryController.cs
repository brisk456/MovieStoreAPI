using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesStore.Domain.Interfaces;
using MoviesStore.Domain.Models;
using MovieStore.API.Dtos.Category;

namespace MovieStore.API.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : MainController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(IMapper mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var categoriec = await _categoryService.GetAll();

            return Ok(_mapper.Map<IEnumerable<CategoryResultDto>>(categoriec));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CategoryResultDto>(category));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var categpry = _mapper.Map<Category>(categoryAddDto);
            var categpryResult = await _categoryService.Add(categpry);

            if (categpryResult == null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<CategoryResultDto>(categpryResult));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, CategoryEditDto categoryEditDto)
        {
            if (id != categoryEditDto.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            await _categoryService.Update(_mapper.Map<Category>(categoryEditDto));

            return Ok(categoryEditDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remove(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            var result = await _categoryService.Remove(category);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet]
        [Route("search/{category}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Category>>> Search(string category)
        {
            var categories = _mapper.Map<List<Category>>(await _categoryService.Search(category));
            if (categories == null || categories.Count == 0)
            {
                return NotFound("None category was founded");
            }

            return Ok(categories);
        }

    }
}
