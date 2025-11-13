namespace ApiUsers.Dtos
{
    public class TicketWithTransactionDto
    {

        public int Id_user { get; set; }
        public int Id_dept_target { get; set; }
        //public int Id_category { get; set; }
        public int Id_status { get; set; }
        public string Desc { get; set; } = string.Empty;
        //public int Priority_level { get; set; }

        public string Body { get; set; } = string.Empty;
        public IFormFile File { get; set; } = null!;
    }

}
