using Desafio100KUsers.Model.Response;
using Desafio100KUsers.Model.User;
using Desafio100KUsers.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace Desafio100KUsers.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly Stopwatch st;
    private readonly UserService userService;

    public UserController( UserService userService)
    {
        this.userService = userService;
        st = new Stopwatch();
    }

    // POST users
    [HttpPost("/users")]
    public IActionResult Post([FromBody] List<User> users)
    {
        st.Restart();
        var count = userService.SaveUsers(users);
        var response = new UserSaveResponse { Message = "Arquivo recebido com sucesso", UserCount = users.Count() };
        return BuildResponseOK(response);
    }
   
    // GET superusers
    [HttpGet("/superusers")]
    public IActionResult GetSuperUsers()
    {
        st.Restart();
        var data = userService.GetSuperUsers();
        return BuildResponseOK(data);
    }

    // GET top-countries
    [HttpGet("/top-countries")]
    public IActionResult GetTopCoutries()
    {
        st.Restart();
        var group = userService.GetTopCoutries();
        return BuildResponseOK(group);
    }

    // GET active-users-per-day
    [HttpGet("/active-users-per-day")]
    public IActionResult GetActiveUsersPerDay([FromQuery] int? Min)
    {
        st.Restart();
        var data = userService.GetActiveUsersPerDay(Min);
        return BuildResponseOK(data);
    }

    [HttpGet("/evaluation")]
    public async Task<IActionResult> GetEvaluation([FromServices] EvaluateService evaluateService)
    {
        st.Restart();
        var data = await evaluateService.ValidateAllResponsesAsync();
        return BuildResponseOK( new { tested_endpoints = data });
    }
    private IActionResult BuildResponseOK<T>(T? data)
    {
        st.Stop();
        return Ok(new TimmedResponse<T>
        {
            Timestamp = DateTime.Now,
            ExecutionTimeMs = st.ElapsedMilliseconds,
            Elapsed = st.Elapsed,
            Data = data
        });
    }
}
