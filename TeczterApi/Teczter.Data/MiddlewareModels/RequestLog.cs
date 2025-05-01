namespace Teczter.Data.MiddlewareModels;

public class RequestLog
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public string? User { get; set; }
    public string? Path { get; set; }
    public string? Method { get; set; }
    public string? Query { get; set; }
    public string? StatusCode { get; set; }
}
