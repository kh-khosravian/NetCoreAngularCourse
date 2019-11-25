using System;
using Microsoft.AspNetCore.Http;

namespace Dateing.API.DTO
{
    public class CreatePhotoModel
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicID { get; set; }

        public CreatePhotoModel()
        {
            DateAdded = DateTime.Now;
        }
    }
}