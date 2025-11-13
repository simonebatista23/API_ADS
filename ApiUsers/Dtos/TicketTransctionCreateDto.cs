namespace ApiUsers.Dtos
{
    public class TicketTransctionCreateDto
    {
        public int? Id_user_source { get; set; }
        public int? Id_user_target { get; set; }
        public int? Id_ticket { get; set; }
        public string Body { get; set; } = null!;
        public IFormFile? File { get; set; }
    }
}
