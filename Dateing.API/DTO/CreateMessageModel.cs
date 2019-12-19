using System;

namespace Dateing.API.DTO
{
    public class CreateMessageModel
    {
        public CreateMessageModel()
        {
            this.MessageSent = DateTime.Now;
        }
        public long SenderId { get; set; }
        public long RecipientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }

    }
}