namespace ApiUsers.Dtos
{
  
    public class UserCreateDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? IdDept { get; set; }
        
        public int? IdProfile { get; set; }
      
    }
}
