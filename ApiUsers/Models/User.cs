using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiUsers.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Pwd { get; set; } = null!;

    public int? IdDept { get; set; }

    public int? IdStatus { get; set; }

    public int? IdProfile { get; set; }

    public bool Blocked { get; set; }

    [JsonIgnore]
    public virtual Dept? IdDeptNavigation { get; set; }


    [JsonIgnore]
    public virtual Profile? IdProfileNavigation { get; set; }

    [JsonIgnore]
    public virtual Statususer? IdStatusNavigation { get; set; }

    public virtual ICollection<Tickets> Tickets { get; set; } = new List<Tickets>();
    public virtual ICollection<TicketsTransction> TicketsSource { get; set; } = new List<TicketsTransction>();


    public virtual ICollection<TicketsTransction> TicketsTarget { get; set; } = new List<TicketsTransction>();
}
