namespace ApiUsers.Dtos
{
    public class TicketTransctionDto
    {
        public Guid Id { get; set; }


        public int? Id_user_source { get; set; }
        public int? Id_user_target { get; set; }


        public int? Id_ticket { get; set; }


        public string Body { get; set; } = null!;
        public DateTime Created_at { get; set; }
        public string? Attach_url { get; set; }
    }
}
