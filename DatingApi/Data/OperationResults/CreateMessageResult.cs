using DatingApi.Data.DataTransferObjects;

namespace DatingApi.Data.OperationResults
{
    public class CreateMessageResult
    {
        public bool IsSuccessful { get; set; }   
        public string Message { get; set; }
        public MessageDto CreatedMessage { get; set; }
    }
}