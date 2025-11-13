namespace ApiUsers.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string Desc { get; set; } = null!;
        public int? IdDept { get; set; }

    }
}
