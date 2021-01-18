using DatingApi.Data.DataTransferObjects;

namespace DatingApi.Data.OperationResults
{
    public class OperationResult<T>
    {
        public bool IsSuccessful { get; set; }   
        public string Message { get; set; }
        public T Value { get; set; }
    }
}