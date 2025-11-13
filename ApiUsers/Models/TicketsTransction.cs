using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiUsers.Models
{
    public class TicketsTransction
    {

        [Key]
        public Guid Id { get; set; }

      
        public int? Id_user_source { get; set; }
        public int? Id_user_target { get; set; }

   
        public int? Id_ticket { get; set; }


        public string Body { get; set; } = null!; 
        public DateTime Created_at { get; set; } 
        public string? Attach_url { get; set; }


        [JsonIgnore]
        public virtual User? UserSource { get; set; } = null!;

        [JsonIgnore]
        public virtual User? UserTarget { get; set; } = null!;

        [JsonIgnore]
        public virtual Tickets? TicketNavigation { get; set; } = null!;
    }
}
