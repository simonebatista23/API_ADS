using Microsoft.AspNetCore.Mvc;
using ApiUsers.Models;
using ApiUsers.Repositories;
using ApiUsers.Dtos;

namespace ApiUsers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeptsController : ControllerBase
    {
        private readonly IDeptRepository _deptRepository;

        public DeptsController(IDeptRepository deptRepository)
        {
            _deptRepository = deptRepository;
        }

        // GET: api/depts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeptDto>>> GetDepts()
        {
            var depts = await _deptRepository.GetAllAsync();
            var deptsDto = depts.Select(d => new DeptDto
            {
                Id = d.Id,
                Name = d.Name,
                AcceptTicket = d.AcceptTicket
            }).ToList();

            return Ok(deptsDto);
        }

        // GET: api/depts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeptDto>> GetDept(int id)
        {
            try
            {
                var dept = await _deptRepository.GetByIdAsync(id);
                if (dept == null) return NotFound();

                var deptDto = new DeptDto
                {
                    Id = dept.Id,
                    Name = dept.Name,
                    AcceptTicket = dept.AcceptTicket
                };

                return Ok(deptDto);
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

        // GET: api/depts/ativos
        [HttpGet("ativos")]
        public async Task<ActionResult<IEnumerable<DeptDto>>> GetActiveDepts()
        {
            try
            {
                var depts = await _deptRepository.GetActiveDeptsAsync();
                var deptsDto = depts.Select(d => new DeptDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    AcceptTicket = d.AcceptTicket
                }).ToList();

                return Ok(deptsDto);
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

        // POST: api/depts
        [HttpPost]
        public async Task<ActionResult<DeptDto>> CreateDept(DeptDto deptDto)
        {
            try
            {
                var dept = new Dept
                {
                    Name = deptDto.Name,
                    AcceptTicket = deptDto.AcceptTicket
                };

                var createdDept = await _deptRepository.CreateAsync(dept);

                var createdDto = new DeptDto
                {
                    Id = createdDept.Id,
                    Name = createdDept.Name,
                    AcceptTicket = createdDept.AcceptTicket
                };

                return CreatedAtAction(nameof(GetDept), new { id = createdDto.Id }, createdDto);
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

        // PUT: api/depts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDept(int id, DeptDto deptDto)
        {
            try
            {
                if (id <= 0) return BadRequest();

                var dept = new Dept
                {
                    Id = id,
                    Name = deptDto.Name,
                    AcceptTicket = deptDto.AcceptTicket
                };

                await _deptRepository.UpdateAsync(dept);
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

        // DELETE: api/depts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDept(int id)
        {
            await _deptRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
