using System.ComponentModel.DataAnnotations;

namespace HMS.Dto.Auth
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
