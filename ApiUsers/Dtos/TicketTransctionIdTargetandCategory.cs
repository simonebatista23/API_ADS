namespace ApiUsers.Dtos
{
    public class TicketTransctionIdTargetandCategory
    {
        //public int Id_tickt { get; set; }
        public Guid Id_transcition { get; set; }
        public int Id_user_target { get; set; }
        public int Id_category { get; set; }
        public int Priority_level { get; set; }

    }
}
