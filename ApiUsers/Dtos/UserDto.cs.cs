
namespace ApiUsers.Dtos
{

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? IdDept { get; set; }
        public int? IdStatus { get; set; }
        public int? IdProfile { get; set; }
        public bool Blocked { get; set; }
    }




}
