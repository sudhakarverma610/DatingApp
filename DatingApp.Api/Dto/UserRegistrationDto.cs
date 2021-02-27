using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Dto
{
    public class UserRegistrationDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string KnowAs { get; set; }
        [Required]
         public DateTime DateOfBirth { get; set; }
         public string City { get; set; }
         public string Country { get; set; }
         public DateTime Created { get; set; }
         public DateTime LastActive { get; set; }
        [Required]
        [StringLength(14,MinimumLength=4,ErrorMessage="Please Provide  14 to 4 Character for Password  ")]
        public string Password { get; set; }
        public UserRegistrationDto()
        {
            Created=DateTime.Now;
            LastActive=DateTime.Now;
        }
    }
}