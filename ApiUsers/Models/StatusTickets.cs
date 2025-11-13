namespace ApiUsers.Models
{
    public class StatusTickets
    {
        public int Id { get; set; }

        public string Desc { get; set; } = null!;
        public virtual ICollection<Tickets> Tickets { get; set; } = new List<Tickets>();
    }
}
