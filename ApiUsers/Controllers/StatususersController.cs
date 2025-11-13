using Microsoft.AspNetCore.Mvc;
using ApiUsers.Models;
using ApiUsers.Repositories;
using ApiUsers.Dtos;

namespace ApiUsers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatususersController : ControllerBase
    {
        private readonly IStatususerRepository _statusRepository;

        public StatususersController(IStatususerRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatususerDto>>> GetStatususers()
        {
            try
            {
                var statusList = await _statusRepository.GetAllAsync();
                var dtoList = statusList.Select(s => new StatususerDto
                {
                    Id = s.Id,
                    Desc = s.Desc
                }).ToList();

                return Ok(dtoList);
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

        [HttpGet("{id}")]
        public async Task<ActionResult<StatususerDto>> GetStatususer(int id)
        {
            try
            {
                var status = await _statusRepository.GetByIdAsync(id);
                if (status == null) return NotFound();

                var dto = new StatususerDto
                {
                    Id = status.Id,
                    Desc = status.Desc
                };

                return Ok(dto);
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

        [HttpGet("ativos")]
        public async Task<ActionResult<IEnumerable<StatususerDto>>> GetActiveStatususers()
        {
            try
            {
                var activeList = await _statusRepository.GetActiveAsync();
                var dtoList = activeList.Select(s => new StatususerDto
                {
                    Id = s.Id,
                    Desc = s.Desc
                }).ToList();

                return Ok(dtoList);
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

        [HttpPost]
        public async Task<ActionResult<StatususerDto>> CreateStatususer(StatususerDto statusDto)
        {
            try
            {
                var status = new Statususer
                {
                    Desc = statusDto.Desc
                };

                var createdStatus = await _statusRepository.CreateAsync(status);

                var createdDto = new StatususerDto
                {
                    Id = createdStatus.Id,
                    Desc = createdStatus.Desc
                };

                return CreatedAtAction(nameof(GetStatususer), new { id = createdDto.Id }, createdDto);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatususer(int id, StatususerDto statusDto)
        {
            try
            {
                if (id <= 0) return BadRequest();

                var status = new Statususer
                {
                    Id = id,
                    Desc = statusDto.Desc
                };

                await _statusRepository.UpdateAsync(status);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatususer(int id)
        {
            await _statusRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
