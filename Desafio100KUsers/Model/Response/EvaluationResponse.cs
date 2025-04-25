using System.Net;

namespace Desafio100KUsers.Model.Response;

public class EvaluationResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public long TimeMs { get; set; }
    public TimeSpan Elapsed { get; set; }
    public bool ValidResponse { get; set; }
}
