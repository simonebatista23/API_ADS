using Microsoft.AspNetCore.Mvc;
using ApiUsers.Models;
using ApiUsers.Repositories;
using ApiUsers.Dtos;

namespace ApiUsers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoty _categoryRepository;

        public CategoryController(ICategoty categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();

                var categoriesDto = categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Desc = c.Desc,
                    IdDept = c.IdDept
                }).ToList();

                return Ok(categoriesDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    message = "Ocorreu um erro ao buscar informacoes contatar administrador.",
                    error = ex.Message
                });
            }
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null) return NotFound();

                var categoryDto = new CategoryDto
                {
                    Id = category.Id,
                    Desc = category.Desc,
                    IdDept = category.IdDept
                };

                return Ok(categoryDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    message = "Ocorreu um erro ao buscar informacoes contatar administrador.",
                    error = ex.Message
                });
            }
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryDto categoryDto)
        {
            try
            {
                var category = new Category
                {
                    Desc = categoryDto.Desc,
                    IdDept = categoryDto.IdDept
                };

                var createdCategory = await _categoryRepository.CreateAsync(category);

                var createdDto = new CategoryDto
                {
                    Id = createdCategory.Id,
                    Desc = createdCategory.Desc,
                    IdDept = createdCategory.IdDept
                };

                return CreatedAtAction(nameof(GetCategory), new { id = createdDto.Id }, createdDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    message = "Ocorreu um erro ao cadastrar contatar administrador.",
                    error = ex.Message
                });
            }
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDto categoryDto)
        {
            try
            {
                if (id <= 0) return BadRequest();

                var category = new Category
                {
                    Id = id,
                    Desc = categoryDto.Desc,
                    IdDept = categoryDto.IdDept
                };

                await _categoryRepository.UpdateAsync(category);
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    message = "Ocorreu um erro ao atualizar contatar administrador.",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
