using Desafio100KUsers.Model.Response;
using Desafio100KUsers.Model.User;
using System.Diagnostics;
using System.Text.Json;
namespace Desafio100KUsers.Services;

public class EvaluateService
{
    private readonly HttpClient client;
    private readonly Stopwatch st;
    private List<User> users = [];
    private readonly Dictionary<string, EvaluationResponse> results = [];
    private readonly ILogger<EvaluateService> logger;

    public EvaluateService(IHttpContextAccessor context, ILogger<EvaluateService> logger)
    {
        client = new HttpClient() 
        { 
            BaseAddress = new Uri("http://" + context.HttpContext!.Request.Host.Value)
        };
        st = new Stopwatch();
        this.logger = logger;
    }

    public async Task<Dictionary<string, EvaluationResponse>> ValidateAllResponsesAsync()
    {
        LoadUsersFromJson();
        await PostCreateUsers();
        await RunGetAPI("/superusers");
        await RunGetAPI("/top-countries");
        await RunGetAPI("/active-users-per-day?MIn=34000");

        return results;
    }

    private async Task PostCreateUsers()
    {
        st.Start();
        var response = await client.PostAsJsonAsync("/users", users);
        results.Add("/users", await BuildResponse(response));
    }

    private void LoadUsersFromJson()
    {
        var usersJson = File.ReadAllText("usuarios.json");
        users = JsonSerializer.Deserialize<List<User>>(usersJson) ?? [];
    }

    private async Task RunGetAPI(string url)
    {
        st.Restart();
        var response = await client.GetAsync(url);
        results.Add(url, await BuildResponse(response));
    }

    private async Task<EvaluationResponse> BuildResponse(HttpResponseMessage response)
    {
        st.Stop();
        return new()
        {
            StatusCode = response.StatusCode,
            TimeMs = st.ElapsedMilliseconds,
            Elapsed = st.Elapsed,
            ValidResponse = ValidateJson(await response.Content.ReadAsStringAsync())
        };
    }
    private bool ValidateJson(string json)
    {
        var deserializedJson = JsonSerializer.Deserialize<object>(json);
        logger.LogInformation("Deserialized JSON: {Json}", deserializedJson);
        return deserializedJson != null;
    }
}
