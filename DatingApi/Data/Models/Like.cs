namespace DatingApi.Data.Models
{
    public class Like
    {
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int ReceivedId { get; set; }
        public User Receiver { get; set; }
    }
}