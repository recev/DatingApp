namespace DatingApi.Data.DataTransferObjects
{
    public class UpdateUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}