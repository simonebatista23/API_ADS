using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiUsers.Models;

public partial class Statususer
{
    public int Id { get; set; }

    public string Desc { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
