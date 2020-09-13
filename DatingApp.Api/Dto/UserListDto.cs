using System;
using System.Collections.Generic;

namespace DatingApp.Api.Dto
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } 
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnowAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; } 
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }

    }
}