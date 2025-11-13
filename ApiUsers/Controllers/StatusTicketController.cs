using Microsoft.AspNetCore.Mvc;
using ApiUsers.Models;
using ApiUsers.Repositories;
using ApiUsers.Dtos;

namespace ApiUsers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusTicketController : Controller
    {
        private readonly IStatusTicketRepository _statusRepository;

        public StatusTicketController(IStatusTicketRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusTicketDto>>> GetStatusTicksts()
        {
            try
            {
                var statusList = await _statusRepository.GetAllAsync();
                var dtoList = statusList.Select(s => new StatusTicketDto
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
        public async Task<ActionResult<StatusTicketDto>> GetStatusTicksts(int id)
        {
            try
            {
                var status = await _statusRepository.GetByIdAsync(id);
                if (status == null) return NotFound();

                var dto = new StatusTicketDto
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
        public async Task<ActionResult<IEnumerable<StatusTicketDto>>> GetActiveStatusTicksts()
        {
            try
            {
                var activeList = await _statusRepository.GetActiveAsync();
                var dtoList = activeList.Select(s => new StatusTicketDto
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
        public async Task<ActionResult<StatusTicketDto>> CreateStatusTicksts(StatusTicketDto statusDto)
        {
            try
            {
                var status = new StatusTickets
                {
                    Desc = statusDto.Desc
                };

                var createdStatus = await _statusRepository.CreateAsync(status);

                var createdDto = new StatusTicketDto
                {
                    Id = createdStatus.Id,
                    Desc = createdStatus.Desc
                };

                return CreatedAtAction(nameof(GetStatusTicksts), new { id = createdDto.Id }, createdDto);
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
        public async Task<IActionResult> UpdateStatusTicksts(int id, StatusTicketDto statusDto)
        {
            try
            {
                if (id <= 0) return BadRequest();

                var status = new StatusTickets
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
        public async Task<IActionResult> DeleteStatusTicksts(int id)
        {

            await _statusRepository.DeleteAsync(id);
            return NoContent();
        }
    }

}
