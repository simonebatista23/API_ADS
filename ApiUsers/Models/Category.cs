using System.Text.Json.Serialization;

namespace ApiUsers.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Desc { get; set; } = null!;
        public int? IdDept { get; set; }

        [JsonIgnore]
        public virtual Dept? IdDeptNavigation { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; } = new List<Tickets>();
    }
}
