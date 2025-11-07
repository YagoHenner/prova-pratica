public class ErrorResponse
{
    public string ErrorType { get; set; }
    public IEnumerable<string> Message { get; set; }
    public int ErrorCode { get; set; }

    public ErrorResponse(string errorType, IEnumerable<string> message, int errorCode)
    {
        ErrorType = errorType;
        Message = message;
        ErrorCode = errorCode;
    }
}
