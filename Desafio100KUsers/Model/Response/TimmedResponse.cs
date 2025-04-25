namespace Desafio100KUsers.Model.Response;

public class TimmedResponse<T>
{
    public DateTime Timestamp { get; set; }
    public long ExecutionTimeMs { get; set; }
    public TimeSpan Elapsed { get; set; }
    public T? Data { get; set; }
}
