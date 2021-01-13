namespace DatingApi.Data.Models
{
    public class Like
    {
        public string SenderId { get; set; }
        public User Sender { get; set; }
        public string ReceivedId { get; set; }
        public User Receiver { get; set; }
    }
}