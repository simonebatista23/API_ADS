using ApiUsers.Models; 
using ApiUsers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{

    private readonly GeminiChatService _chatService;

    public ChatController(GeminiChatService chatService)
    {
        _chatService = chatService;
    }


    [HttpPost] 
    public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
    {
  
        if (request == null || string.IsNullOrWhiteSpace(request.Message))
        {
          
            return BadRequest(new { error = "O corpo da requisição é inválido ou a mensagem está vazia." });
        }

    
        var responseText = await _chatService.SendMessageAsync(request.Message);

        if (responseText.StartsWith("ERRO da API:"))
        {
            
            return StatusCode(500, new { error = responseText });
        }

        return Ok(new { response = responseText });
    }
}