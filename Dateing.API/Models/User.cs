using System;
using System.Collections;
using System.Collections.Generic;

namespace Dateing.API.Models
{
    public class User
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDay { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Introduction { get; set; }

        public string lookingFor { get; set; }

        public string Intrest { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public ICollection<Like> Likers { get; set; }
        
        public ICollection<Like> Likees { get; set; }
    }
}