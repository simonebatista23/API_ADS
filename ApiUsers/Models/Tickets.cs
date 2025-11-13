using System.Text.Json.Serialization;

namespace ApiUsers.Models
{
    public class Tickets
    {
        public int Id { get; set; }

        public int? Id_user { get; set; }
        public int? Id_dept_target { get; set; }
        public int? Id_category { get; set; }
        public int? Id_status { get; set; }

        public string Desc { get; set; } = null!;
        public DateTime Open_datetime { get; set; } 
        public int Priority_level { get; set; }

        [JsonIgnore]
        public virtual Dept? IdDeptNavigation { get; set; }

        [JsonIgnore]
        public virtual Category? IdCategoryNavigation { get; set; }

        [JsonIgnore]
        public virtual StatusTickets? IdStatusTicketsNavigation { get; set; }

        [JsonIgnore]
        public virtual User? IdUserNavigation { get; set; }

        public virtual ICollection<TicketsTransction> TicketsTransctions { get; set; } = new List<TicketsTransction>();
    }
}
