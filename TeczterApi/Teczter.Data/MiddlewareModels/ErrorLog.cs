namespace Teczter.Data.MiddlewareModels;
public class ErrorLog
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public string? User { get; set; }
    public string ExceptionType { get; set; } = null!;
    public string? StackTrace { get; set; }
    public string? Message { get; set; }
    public string? InnerExceptionMessage { get; set; }
    public int? RequestLogId { get; set; }

    public RequestLog? RequestLog {get; set;}
}
