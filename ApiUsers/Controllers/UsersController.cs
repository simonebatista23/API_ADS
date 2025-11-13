using ApiUsers.Dtos;
using ApiUsers.Models;
using ApiUsers.Repositories;
using ApiUsers.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiUsers.Enums;

namespace ApiUsers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }


        // GET: api/Users
        [HttpGet]

        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _repository.GetAllAsync();
                var dtoList = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    IdDept = u.IdDept,
                    IdStatus = u.IdStatus,
                    IdProfile = u.IdProfile,
                    Blocked = u.Blocked
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

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id);
                if (user == null) return NotFound();

                var dto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    IdDept = user.IdDept,
                    IdStatus = user.IdStatus,
                    IdProfile = user.IdProfile,
                    Blocked = user.Blocked
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

        // POST: api/Users 
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserCreateDto userDto)
        {
            try
            {
                var users = await _repository.GetAllAsync();

                bool emailExists = users.Any(u => u.Email == userDto.Email);

                if (emailExists)
                {
                    return BadRequest("Já existe um usuário cadastrado com esse e-mail.");
                }


                var user = new User
                {
                    Name = userDto.Name,

                    Email = userDto.Email,
                    IdDept = userDto.IdDept,
                    IdProfile = userDto.IdProfile,
                    Blocked = false
                };

                var senha = "suportIa1234";

                var hasher = new PasswordHasher<User>();
                user.Pwd = hasher.HashPassword(user, senha);

                user.IdStatus = (int)UserStatus.Ativo;
                await _repository.AddAsync(user);

                var createdDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    IdDept = user.IdDept,
                    IdProfile = user.IdProfile,
                    Blocked = user.Blocked
                };

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.IdDept,
                    user.IdStatus,
                    user.IdProfile,
                    user.Blocked,
                });
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

        // PUT: api/Users/5 (Atualizar sem senha)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            try
            {
                if (id != userDto.Id) return BadRequest();

                var existingUser = await _repository.GetByIdAsync(id);
                if (existingUser == null) return NotFound();

                existingUser.Name = userDto.Name;
                existingUser.Email = userDto.Email;
                existingUser.IdDept = userDto.IdDept;
                existingUser.IdStatus = userDto.IdStatus;
                existingUser.IdProfile = userDto.IdProfile;
                existingUser.Blocked = userDto.Blocked;

                // Mantém a senha existente
                // existingUser.Pwd = existingUser.Pwd;

                await _repository.UpdateAsync(existingUser);
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


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        // POST: api/Users/login
        [HttpPost("login")]
       
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
                    return BadRequest(new { message = "Email e senha são obrigatórios." });

                var user = await _repository.FindByEmailAsync(loginDto.Email);

                if (user == null)
                    return Unauthorized(new { message = "Email ou senha incorretos." });

                if (user.Blocked)
                    return Unauthorized(new { message = "Usuário bloqueado. Entre em contato com o administrador." });

                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, user.Pwd, loginDto.Password);

                if (result != PasswordVerificationResult.Success)
                    return Unauthorized(new { message = "Email ou senha incorretos." });


                // 1. Defina a senha padrão da empresa
                const string SENHA_PADRAO_EMPRESA = "suportIa1234";

          
                var hasherPadrao = new PasswordHasher<User>();

                var isPwdPadrao = hasherPadrao.VerifyHashedPassword(user, user.Pwd, SENHA_PADRAO_EMPRESA);

            
                bool mustChangePassword = isPwdPadrao == PasswordVerificationResult.Success;

           

                var deptName = user.IdDeptNavigation?.Name?.Trim().ToLower() ?? "";
                var profileName = user.IdProfileNavigation?.Desc?.Trim().ToLower() ?? "";

                bool isAdmin = deptName == "rh" && profileName == "superadmin";

                var token = TokenServices.GenerateToken(user);

                return Ok(new
                {
                    message = isAdmin ? "Login de administrador bem-sucedido." : "Login realizado com sucesso",
                    user = new
                    {
                        user.Id,
                        user.Name,
                        user.Email,
                        IdDept = user.IdDeptNavigation?.Id,
                        Department = user.IdDeptNavigation?.Name,
                        Profile = user.IdProfileNavigation?.Desc,
                        IsAdmin = isAdmin,
                       
                        MustChangePassword = mustChangePassword
                    },
                    token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Ocorreu um erro ao logar, contate o administrador.",
                    error = ex.Message
                });
            }
        }

        //atualizar senha
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var user = await _repository.GetByIdAsync(dto.UserId);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            var hasher = new PasswordHasher<User>();

            var result = hasher.VerifyHashedPassword(user, user.Pwd, dto.CurrentPassword);

            if (result == PasswordVerificationResult.Failed)
                return BadRequest("Senha atual incorreta.");

            user.Pwd = hasher.HashPassword(user, dto.NewPassword);

            await _repository.UpdateAsync(user);

            return Ok("Senha alterada com sucesso!");
        }

        // PUT: api/Users/{id}/block?blocked=true
        [HttpPut("{id}/block")]
        public async Task<IActionResult> ToggleBlockUser(int id, [FromQuery] bool blocked)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id);
                if (user == null)
                    return NotFound(new { message = "Usuário não encontrado." });

                user.Blocked = blocked;
                await _repository.UpdateAsync(user);

                var statusMsg = blocked ? "Usuário bloqueado com sucesso." : "Usuário desbloqueado com sucesso.";

                return Ok(new
                {
                    message = statusMsg,
                    user = new
                    {
                        user.Id,
                        user.Name,
                        user.Email,
                        user.Blocked
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Erro ao atualizar status de bloqueio. Contate o administrador.",
                    error = ex.Message
                });
            }
        }


    }
}
