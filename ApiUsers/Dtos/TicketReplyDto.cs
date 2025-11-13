namespace ApiUsers.Dtos
{
    public class TicketReplyDto
    {
        public int Id_ticket { get; set; }
        public int Id_user_source { get; set; }  
        public string Body { get; set; } = string.Empty;
        public IFormFile? File { get; set; }     
    }
}
