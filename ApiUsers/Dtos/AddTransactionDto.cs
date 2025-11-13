namespace ApiUsers.Dtos
{
    public class AddTransactionDto
    {
        public int Id_ticket { get; set; }

        public string Body { get; set; }

        public int SenderUserId { get; set; }

        // Arquivo (opcional)
        public IFormFile? File { get; set; }
    }
}
