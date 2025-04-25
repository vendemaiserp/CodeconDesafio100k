using Desafio100KUsers.Model.Response;
using Desafio100KUsers.Model.User;
using Microsoft.Extensions.Caching.Memory;

namespace Desafio100KUsers.Services;

public class UserService(IMemoryCache memoryCache)
{
    private readonly IMemoryCache memoryCache = memoryCache;

    public int SaveUsers(List<User> users)
    {
        memoryCache.Set("users", users);
        return users.Count;
    }

    public List<CountriesResponse> GetTopCoutries()
    {
        var users = GetSuperUsers();
        var group = users
            .GroupBy(u => u.Pais)
            .Select(g => new CountriesResponse
            {
                Country = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(c => c.Count)
            .Take(5)
            .ToList();
        return group;
    }
    public List<User> GetSuperUsers()
    {
        var users = GetUserList();
        return users.Where(u => u.Score > 900 && u.Ativo).ToList();
    }

    public List<LoginsPerdayResponse> GetActiveUsersPerDay(int? min)
    {
        var users = GetUserList();
        var groupedLogins = users
            .SelectMany(u => u.Logs)
            .Where(l => l.Acao == "login")
            .GroupBy(l => l.Data)
            .Select(g => new LoginsPerdayResponse
            {
                Date = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(c => c.Date)
            .ToList();

        var filteredData = min is not null ?
            groupedLogins.Where(l => l.Count >= min) :
            groupedLogins;

        return filteredData.ToList();
    }

    private IEnumerable<User> GetUserList() =>
        memoryCache.Get<List<User>>("users") ?? [];
}
