namespace ApiUsers.Dtos
{
    public class CloseTicketDto
    {
        public int TicketId { get; set; }
        public int UserClosingId { get; set; }
        public string Message { get; set; } = string.Empty; 
    }

}
