namespace Dateing.API.Models
{
    public class Like
    {
        public long LikerId { get; set; }
        public long LikeeId { get; set; }
        public User Liker { get; set; }
        public User Likee { get; set; }
    }
}