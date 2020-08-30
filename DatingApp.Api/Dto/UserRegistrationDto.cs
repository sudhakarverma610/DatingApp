using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Dto
{
    public class UserRegistrationDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8,MinimumLength=4,ErrorMessage="Please Provide  8 to 4 Character for Password  ")]
        public string Password { get; set; }
        
    }
}