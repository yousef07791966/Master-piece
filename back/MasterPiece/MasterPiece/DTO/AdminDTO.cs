namespace MasterPiece.DTO
{
    public class AdminDTO
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public IFormFile? Img { get; set; }

        public string Password { get; set; } = null!;
    }
}
