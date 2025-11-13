using Microsoft.AspNetCore.Mvc;
using ApiUsers.Models;
using ApiUsers.Repositories;
using ApiUsers.Dtos;

namespace ApiUsers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketTransctionController : ControllerBase
    {
        private readonly ITicketTransctionRepository _repository;

        public TicketTransctionController(ITicketTransctionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketTransctionDto>>> GetAll()
        {
            try
            {
                var transactions = await _repository.GetAllAsync();

                var dtoList = transactions.Select(t => new TicketTransctionDto
                {
                    Id = t.Id,
                    Id_user_source = t.Id_user_source,
                    Id_user_target = t.Id_user_target,
                    Id_ticket = t.Id_ticket,
                    Body = t.Body,
                    Created_at = t.Created_at,
                    Attach_url = t.Attach_url
                });

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
        public async Task<ActionResult<TicketTransctionDto>> GetById(Guid id)
        {
            try
            {
                var transaction = await _repository.GetByIdAsync(id);
                if (transaction == null) return NotFound();

                var dto = new TicketTransctionDto
                {
                    Id = transaction.Id,
                    Id_user_source = transaction.Id_user_source,
                    Id_user_target = transaction.Id_user_target,
                    Id_ticket = transaction.Id_ticket,
                    Body = transaction.Body,
                    Created_at = transaction.Created_at,
                    Attach_url = transaction.Attach_url
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

        [HttpPost]
        public async Task<ActionResult<TicketTransctionDto>> Create([FromForm] TicketTransctionCreateDto dto)
        {
            try
            {
                string? attachUrl = null;

                if (dto.File != null && dto.File.Length > 0)
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Console.WriteLine($"Caminho completo do upload: {uploadFolder}");
                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    var extension = Path.GetExtension(dto.File.FileName);

                    var nameWithoutExt = Path.GetFileNameWithoutExtension(dto.File.FileName)
                                          .Replace(" ", "_")            
                                          .Replace("(", "")            
                                          .Replace(")", "")            
                                          .Replace("[", "")               
                                          .Replace("]", "")               
                                          .Replace(",", "")               
                                          .Replace("'", "");             

                   
                    var fileName = $"{Guid.NewGuid()}_{nameWithoutExt}{extension}";
                    var filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.File.CopyToAsync(stream);
                    }

              
                    attachUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
                }


                var transaction = new TicketsTransction
                {
                    Id = Guid.NewGuid(),
                    Id_user_source = dto.Id_user_source,
                    Id_user_target = dto.Id_user_target,
                    Id_ticket = dto.Id_ticket,
                    Body = dto.Body,
                    Created_at = DateTime.UtcNow,
                    Attach_url = attachUrl
                };

                var created = await _repository.CreateAsync(transaction);

                var createdDto = new TicketTransctionDto
                {
                    Id = created.Id,
                    Id_user_source = created.Id_user_source,
                    Id_user_target = created.Id_user_target,
                    Id_ticket = created.Id_ticket,
                    Body = created.Body,
                    Created_at = created.Created_at,
                    Attach_url = created.Attach_url
                };

                return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    message = "Ocorreu um erro ao criar contatar administrador.",
                    error = ex.Message
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, TicketTransctionDto dto)
        {
            try
            {
                if (id != dto.Id) return BadRequest("ID não confere");

                var transaction = new TicketsTransction
                {
                    Id = id,
                    Id_user_source = dto.Id_user_source,
                    Id_user_target = dto.Id_user_target,
                    Id_ticket = dto.Id_ticket,
                    Body = dto.Body,
                    Created_at = dto.Created_at,
                    Attach_url = dto.Attach_url
                };

                await _repository.UpdateAsync(transaction);
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
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
