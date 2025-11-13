using ApiUsers.Dtos;
using ApiUsers.Enums;
using ApiUsers.Models;
using ApiUsers.Repositories;
using ApiUsers.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace ApiUsers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicket _ticketRepository;
        private readonly ITicketTransctionRepository _transactionRepository;
        private readonly NotificationService _notificationService;

        public TicketController(
            ITicket ticketRepository,
            ITicketTransctionRepository transactionRepository,
            NotificationService notificationService) 
        {
            _ticketRepository = ticketRepository;
            _transactionRepository = transactionRepository;
            _notificationService = notificationService; 
        }


        private string? GetUserDepartmentFromToken()
        {
            return User.FindFirst("department")?.Value;
        }

        private string? GetUserProfileFromToken()
        {
            return User.FindFirst("profile")?.Value;
        }

        private int? GetUserDeptIdFromToken()
        {
            var claim = User.FindFirst("idDept")?.Value;
            return int.TryParse(claim, out int idDept) ? idDept : null;
        }


        // GET: api/ticket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
        {
            try
            {
                var tickets = await _ticketRepository.GetAllAsync();
                var ticketsDto = tickets.Select(t => new TicketDto
                {
                    Id = t.Id,
                    Id_user = t.Id_user,
                    Id_dept_target = t.Id_dept_target,
                    Id_category = t.Id_category,
                    Id_status = t.Id_status,
                    Desc = t.Desc,
                    Open_datetime = t.Open_datetime,
                    Priority_level = t.Priority_level
                }).ToList();

                return Ok(ticketsDto);
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

        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTicketsAtivos()
        {
            try
            {
                var tickets = await _ticketRepository.GetAllAsync();

                var ticketsFiltrados = tickets
                    .Where(t => t.Id_status != (int)TicketStatus.Cancelado)
                    .ToList();

                var ticketsDto = ticketsFiltrados.Select(t => new TicketDto
                {
                    Id = t.Id,
                    Id_user = t.Id_user,
                    Id_dept_target = t.Id_dept_target,
                    Id_category = t.Id_category,
                    Id_status = t.Id_status,
                    Desc = t.Desc,
                    Open_datetime = t.Open_datetime,
                    Priority_level = t.Priority_level
                }).ToList();

                return Ok(ticketsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Ocorreu um erro ao buscar informações. Contate o administrador.",
                    error = ex.Message
                });
            }
        }

        // GET: api/ticket/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicket(int id)
        {
            try
            {
                var ticket = await _ticketRepository.GetByIdAsync(id);
                if (ticket == null)
                    return NotFound();

                var ticketDto = new TicketDto
                {
                    Id = ticket.Id,
                    Id_user = ticket.Id_user,
                    Id_dept_target = ticket.Id_dept_target,
                    Id_category = ticket.Id_category,
                    Id_status = ticket.Id_status,
                    Desc = ticket.Desc,
                    Open_datetime = ticket.Open_datetime,
                    Priority_level = ticket.Priority_level,
                    TicketsTransctions = ticket.TicketsTransctions?
                    .Select(t => new TicketTransctionDto
                    {
                        Id = t.Id,
                        Id_user_source = t.Id_user_source,
                        Id_user_target = t.Id_user_target,
                        Body = t.Body,
                        Created_at = t.Created_at
                    })
                    .ToList()

                };


                return Ok(ticketDto);
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

        [HttpGet("opened/{userId}")]
        public async Task<ActionResult> GetTicketsOpenedByUser(int userId)
        {
            var tickets = await _ticketRepository.GetTicketsOpenedByUserAsync(userId);

            var ticketsFiltrados = tickets
                .Where(t => t.Id_status != (int)TicketStatus.Cancelado) 
                .ToList(); 

            if (!ticketsFiltrados.Any())
                return NotFound("Nenhum ticket aberto por este usuário ou nenhum ticket com status diferente de Cancelado."); // Ajustei a mensagem

            return Ok(ticketsFiltrados.Select(t => new
            {
                t.Id,
                t.Desc,
                t.Id_category,
                t.Id_status,
                t.Open_datetime,
                Transacoes = t.TicketsTransctions.Select(tr => new
                {
                    tr.Id,
                    tr.Id_user_source,
                    tr.Id_user_target,
                    tr.Body,
                    tr.Created_at
                })
            }));
        }



        [HttpGet("all-user-tickets/{userId}")]
        public async Task<ActionResult> GetAllUserTickets(int userId)
        {
            var tickets = await _ticketRepository.GetAllUserTicketsAsync(userId); 

            if (!tickets.Any())
                return NotFound("Nenhum ticket encontrado para este usuário.");

            return Ok(tickets.Select(t => new
            {
                t.Id,
                t.Desc,
                t.Id_category,
                t.Id_status,
                t.Open_datetime,
                t.Priority_level,
                t.Id_user, 
                Transacoes = t.TicketsTransctions.Select(tr => new
                {
                    tr.Id,
                    tr.Id_user_source,
                    tr.Id_user_target,
                    tr.Body,
                    tr.Created_at,
                    tr.Attach_url
                   
                })
            }));
        }

        [HttpGet("responded/{userId}")]
        public async Task<ActionResult> GetTicketsRespondedByUser(int userId)
        {
            var tickets = await _ticketRepository.GetTicketsRespondedByUserAsync(userId);
            tickets = tickets.Where(t => t.Id_status != 4).ToList();

            if (!tickets.Any())
                return NotFound("Nenhum ticket respondido por este usuário.");

            return Ok(tickets.Select(t => new
            {
                t.Id,
                t.Desc,
                t.Id_category,
                t.Id_status,
                t.Open_datetime,
                Transacoes = t.TicketsTransctions.Select(tr => new
                {
                    tr.Id,
                    tr.Id_user_source,
                    tr.Id_user_target,
                    tr.Body,
                    tr.Created_at,
                    tr.Attach_url
                })
            }));
        }

        [HttpGet("by-department/{idDept}")]
        public async Task<IActionResult> GetTicketsByDepartment(int idDept)
        {
            try
            {
                var userDept = GetUserDepartmentFromToken();
                var userProfile = GetUserProfileFromToken();
                var userDeptId = GetUserDeptIdFromToken();

                if (string.IsNullOrEmpty(userDept))
                    return Unauthorized("Departamento não encontrado no token.");

                if (string.IsNullOrEmpty(userProfile))
                    return Unauthorized("Perfil não encontrado no token.");

                var allowedProfiles = new[] { "suporte", "superadmin" };

                if (!allowedProfiles.Any(p => p.Equals(userProfile, StringComparison.OrdinalIgnoreCase)))
                {
                    return Unauthorized("Acesso permitido apenas para usuários com perfil 'suporte' ou 'superadmin'.");
                }

                if (userDeptId != idDept)
                    return Unauthorized("Você só pode acessar tickets do seu próprio departamento.");

                var tickets = await _ticketRepository.GetTicketsByDepartmentAsync(idDept);

                if (!tickets.Any())
                    return NotFound("Nenhum ticket encontrado para este departamento.");

                return Ok(tickets.Select(t => new
                {
                    t.Id,
                    t.Desc,
                    t.Id_category,
                    t.Id_status,
                    t.Priority_level,
                    t.Open_datetime,
                    t.Id_dept_target,
                    Transacoes = t.TicketsTransctions.Select(tr => new
                    {
                        tr.Id,
                        tr.Id_user_source,
                        tr.Id_user_target,
                        tr.Attach_url,
                        tr.Body,
                        tr.Created_at
                    })
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Erro ao buscar tickets por departamento e perfil.",
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        // POST: api/ticket
        [HttpPost]
        public async Task<ActionResult<TicketDto>> CreateTicket(TicketDto ticketDto)
        {
            try
            {
                var ticket = new Tickets
                {
                    Id_user = ticketDto.Id_user,
                    Id_dept_target = ticketDto.Id_dept_target,
                    Id_category = ticketDto.Id_category,
                    Id_status = ticketDto.Id_status,
                    Desc = ticketDto.Desc,
                    Open_datetime = DateTime.Now,
                    Priority_level = ticketDto.Priority_level
                };

                var createdTicket = await _ticketRepository.CreateAsync(ticket);

                var createdDto = new TicketDto
                {
                    Id = createdTicket.Id,
                    Id_user = createdTicket.Id_user,
                    Id_dept_target = createdTicket.Id_dept_target,
                    Id_category = createdTicket.Id_category,
                    Id_status = createdTicket.Id_status,
                    Desc = createdTicket.Desc,
                    Open_datetime = createdTicket.Open_datetime,
                    Priority_level = createdTicket.Priority_level
                };

                return CreatedAtAction(nameof(GetTicket), new { id = createdDto.Id }, createdDto);
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


        [HttpPost("create-with-transaction")]
        public async Task<ActionResult> CreateTicketWithTransaction([FromForm] TicketWithTransactionDto dto)
        {
            try
            {
                
                string? attachUrl = null;

                if (dto.File != null && dto.File.Length > 0)
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
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

                var ticket = new Tickets
                {
                    Id_user = dto.Id_user,
                    Id_dept_target = dto.Id_dept_target,
                    Id_status = dto.Id_status,
                    Desc = dto.Desc,
                    Open_datetime = DateTime.Now,
                };

                var createdTicket = await _ticketRepository.CreateAsync(ticket);


                var transaction = new TicketsTransction
                {
                    Id = Guid.NewGuid(),
                    Id_user_source = dto.Id_user,  
                    Id_ticket = createdTicket.Id,
                    Body = dto.Body,
                    Created_at = DateTime.UtcNow,
                    Attach_url = attachUrl
                };

                var createdTransaction = await _transactionRepository.CreateAsync(transaction);

                return Ok(new
                {
                    message = "Ticket e transação inicial criados com sucesso!",
                    ticket = new
                    {
                        createdTicket.Id,
                        createdTicket.Desc,
                        createdTicket.Open_datetime
                    },
                    transaction = new
                    {
                        createdTransaction.Id,
                        createdTransaction.Body,
                        createdTransaction.Attach_url,
                        createdTransaction.Created_at
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Erro ao criar o ticket e a transação inicial. Contate o administrador.",
                    error = ex.Message,
                    inner = ex.InnerException?.Message

                });
            }
        }

        [HttpPut("assign-target-category-priority")]
        public async Task<ActionResult> AssignTargetAndCategory(TicketTransctionIdTargetandCategory dto)
        {
            try
            {
   
                var transaction = await _transactionRepository.GetByIdAsync(dto.Id_transcition);
                if (transaction == null)
                    return NotFound("Transação não encontrada.");

                if (!transaction.Id_ticket.HasValue)
                    return BadRequest("Transação não possui ticket associado.");

                var ticket = await _ticketRepository.GetByIdAsync(transaction.Id_ticket.Value);
                if (ticket == null)
                    return NotFound("Ticket não encontrado.");

                ticket.Id_category = dto.Id_category;
                ticket.Priority_level = dto.Priority_level;
                ticket.Id_status = (int)TicketStatus.EmAndamento;

                await _ticketRepository.UpdateAsync(ticket);

                transaction.Id_user_target = dto.Id_user_target;
                await _transactionRepository.UpdateAsync(transaction);

                return Ok(new
                {
                    message = "Categoria, prioridade, atendente e status atualizados com sucesso!",
                    ticket = new
                    {
                        ticket.Id,
                        ticket.Id_category,
                        ticket.Priority_level,
                        ticket.Id_status,
                        ticket.Desc
                    },
                    transaction = new
                    {
                        transaction.Id,
                        transaction.Id_user_target,
                        transaction.Body
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Erro ao atualizar ticket, prioridade e transação.",
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        
        [HttpPost("add-transaction")]
        public async Task<ActionResult> AddTransactionToTicket([FromForm] AddTransactionDto dto)
        {
            try
            {
               
                var ticket = await _ticketRepository.GetByIdAsync(dto.Id_ticket);
                if (ticket == null)
                {
                    return NotFound("Ticket não encontrado.");
                }

                var lastTransaction = await _ticketRepository.GetLastTransactionByTicketIdAsync(dto.Id_ticket);
              
                int newSourceId = dto.SenderUserId;
                int newTargetId;

                if (lastTransaction == null)
                {
                    
                    if (ticket.Id_user == null)
                    {
                        return BadRequest("O criador do ticket (Id_user) não pode ser nulo para definir o primeiro alvo.");
                    }
                    newTargetId = ticket.Id_user.Value; 
                }
                else
                {
                    

                    if (newSourceId == ticket.Id_user)
                    {
                       
                        if (lastTransaction.Id_user_source == null)
                        {
                            return BadRequest("O remetente da última transação (Id_user_source) não pode ser nulo para definir o próximo alvo.");
                        }
                        newTargetId = lastTransaction.Id_user_source.Value; 
                    }
                   
                    else
                    {
                        
                        if (ticket.Id_user == null)
                        {
                            return BadRequest("O criador do ticket (Id_user) não pode ser nulo para definir o próximo alvo.");
                        }
                        newTargetId = ticket.Id_user.Value; 
                    }
                }

                string? attachUrl = null;
                if (dto.File != null && dto.File.Length > 0)
                {
              
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    var extension = Path.GetExtension(dto.File.FileName);
                    var nameWithoutExt = Path.GetFileNameWithoutExtension(dto.File.FileName)
                        .Replace(" ", "_").Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace(",", "").Replace("'", "");

                    var fileName = $"{Guid.NewGuid()}_{nameWithoutExt}{extension}";
                    var filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.File.CopyToAsync(stream);
                    }

                    attachUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
                }
                var newTransaction = new TicketsTransction
                {
                    Id = Guid.NewGuid(),
                    Id_user_source = newSourceId, 
                    Id_user_target = newTargetId, 
                    Id_ticket = dto.Id_ticket,
                    Body = dto.Body,
                    Created_at = DateTime.UtcNow,
                    Attach_url = attachUrl
                };

                var createdTransaction = await _transactionRepository.CreateAsync(newTransaction);

              
                if (ticket.Id_status != (int)TicketStatus.EmAndamento)
                {
                    ticket.Id_status = (int)TicketStatus.EmAndamento;
                    await _ticketRepository.UpdateAsync(ticket);
                }

             
               

                return Ok(new
                {
                    message = "Resposta adicionada com sucesso. Remetente e Destinatário invertidos.",
                    transaction = createdTransaction
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Erro ao adicionar transação e inverter papéis.",
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        [HttpPut("close-ticket")]
        public async Task<IActionResult> CloseTicket(CloseTicketDto dto)
        {
            try
            {
                
                var ticket = await _ticketRepository.GetByIdAsync(dto.TicketId);
                if (ticket == null)
                    return NotFound("Ticket não encontrado.");

               
                ticket.Id_status = (int)TicketStatus.Concluido;
                await _ticketRepository.UpdateAsync(ticket);

                
                var finalTransaction = new TicketsTransction
                {
                    Id = Guid.NewGuid(),
                    Id_ticket = ticket.Id,
                    Id_user_source = dto.UserClosingId,          
                    Id_user_target = ticket.Id_user,             
                    Body = dto.Message,
                    Created_at = DateTime.UtcNow
                };

                await _transactionRepository.CreateAsync(finalTransaction);

                return Ok(new
                {
                    message = "Ticket fechado e usuário notificado com sucesso.",
                    ticketId = ticket.Id,
                    transactionId = finalTransaction.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Erro ao fechar ticket.",
                    error = ex.Message
                });
            }
        }


        [HttpPut("cancel-ticket")]
        public async Task<IActionResult> CancelTicket(CancelTicket dto)
        {
            try
            {

                var ticket = await _ticketRepository.GetByIdAsync(dto.TicketId);
                if (ticket == null)
                    return NotFound("Ticket não encontrado.");


                ticket.Id_status = (int)TicketStatus.Cancelado;
                await _ticketRepository.UpdateAsync(ticket);



                return Ok(new
                {
                    message = "Ticket cancelado e usuário notificado com sucesso.",
                    ticketId = ticket.Id,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Erro ao fechar ticket.",
                    error = ex.Message
                });
            }
        }

        // PUT: api/ticket/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, TicketDto ticketDto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                var ticket = new Tickets
                {
                    Id = id,
                    Id_user = ticketDto.Id_user,
                    Id_dept_target = ticketDto.Id_dept_target,
                    Id_category = ticketDto.Id_category,
                    Id_status = ticketDto.Id_status,
                    Desc = ticketDto.Desc,
                    Open_datetime = ticketDto.Open_datetime,
                    Priority_level = ticketDto.Priority_level
                };

                await _ticketRepository.UpdateAsync(ticket);
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

        // DELETE: api/ticket/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            await _ticketRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
