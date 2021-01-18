namespace DatingApi.Data.OperationResults
{
    public class LoginResult: OperationResult<string>
    {
        public string loggedInUserId { get; set; }
    }
}