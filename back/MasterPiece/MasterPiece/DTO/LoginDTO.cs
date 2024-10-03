using System.ComponentModel.DataAnnotations;

namespace MasterPiece.DTO
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]

        public string Password { get; set; } = null!;
    }
}
