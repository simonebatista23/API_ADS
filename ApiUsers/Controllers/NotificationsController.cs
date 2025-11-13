using ApiUsers.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationService _notificationService;

    //  Injeção via construtor
    public NotificationsController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("{userId}")]
    public IActionResult Get(int userId)
    {
        var notifications = _notificationService.GetNotifications(userId);
        return Ok(notifications);
    }
    [HttpDelete("{userId}")]
    public IActionResult ClearNotifications(int userId)
    {
        _notificationService.ClearNotifications(userId);
        return Ok();
    }

}
