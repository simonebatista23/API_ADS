using Microsoft.AspNetCore.Mvc;
using ApiUsers.Models;
using ApiUsers.Repositories;
using ApiUsers.Dtos;

namespace ApiUsers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfileDto>>> GetProfiles()
        {
            try
            {
                var profiles = await _profileRepository.GetAllAsync();
                var profilesDto = profiles.Select(p => new ProfileDto
                {
                    Id = p.Id,
                    Desc = p.Desc
                }).ToList();

                return Ok(profilesDto);
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
        public async Task<ActionResult<ProfileDto>> GetProfile(int id)
        {
            try
            {
                var profile = await _profileRepository.GetByIdAsync(id);
                if (profile == null) return NotFound();

                var profileDto = new ProfileDto
                {
                    Id = profile.Id,
                    Desc = profile.Desc
                };

                return Ok(profileDto);
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
        public async Task<ActionResult<ProfileDto>> CreateProfile(ProfileDto profileDto)
        {
            try
            {
                var profile = new Profile
                {
                    Desc = profileDto.Desc
                };

                var createdProfile = await _profileRepository.CreateAsync(profile);

                var createdDto = new ProfileDto
                {
                    Id = createdProfile.Id,
                    Desc = createdProfile.Desc
                };

                return CreatedAtAction(nameof(GetProfile), new { id = createdDto.Id }, createdDto);
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
        public async Task<IActionResult> UpdateProfile(int id, ProfileDto profileDto)
        {
            try
            {
                if (id <= 0) return BadRequest();

                var profile = new Profile
                {
                    Id = id,
                    Desc = profileDto.Desc
                };

                await _profileRepository.UpdateAsync(profile);
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
        public async Task<IActionResult> DeleteProfile(int id)
        {
            await _profileRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
