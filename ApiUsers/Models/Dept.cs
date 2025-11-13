using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiUsers.Models;

public partial class Dept
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool AcceptTicket { get; set; }


    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<Category> Category { get; set; } = new List<Category>();

    public virtual ICollection<Tickets> Tickets { get; set; } = new List<Tickets>();
}
