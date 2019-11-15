using System;
using System.Collections.Generic;
using Dateing.API.Models;

namespace Dateing.API.DTO
{
    public class UserDetailModel
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Introduction { get; set; }

        public string lookingFor { get; set; }

        public string Intrest { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PhotoURL { get; set; }

        public ICollection<PhotoModel> Photos { get; set; }
    }
}