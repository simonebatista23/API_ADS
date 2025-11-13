namespace ApiUsers.Dtos
{
    public class TicketDto
    {

        public int Id { get; set; }

        public int? Id_user { get; set; }
        public int? Id_dept_target { get; set; }
        public int? Id_category { get; set; }
        public int? Id_status { get; set; }

        public string Desc { get; set; } = null!;
        public DateTime Open_datetime { get; set; }
        public int Priority_level { get; set; }
        public List<TicketTransctionDto> TicketsTransctions { get; set; }

    }
}
