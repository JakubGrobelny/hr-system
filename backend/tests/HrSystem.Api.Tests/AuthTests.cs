using System.Net;
using System.Net.Http.Json;
using HrSystem.Api.Tests.Infrastructure;

namespace HrSystem.Api.Tests;

public sealed class AuthTests(HrSystemApiFactory factory) : IClassFixture<HrSystemApiFactory>
{
    [Fact]
    public async Task Me_WithoutSession_Returns401()
    {
        var response = await factory.CreateClient().GetAsync("/api/auth/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithWrongPassword_Returns401()
    {
        var response = await factory.CreateClient().LoginAsync(HrSystemApiFactory.EmployeeEmail, "WrongPassword1!");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<Problem>();
        Assert.Equal("Invalid credentials", problem?.Title);
    }

    [Fact]
    public async Task Login_WithUnknownUser_Returns401()
    {
        var response = await factory.CreateClient().LoginAsync("nobody@hr.local");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithInvalidPayload_Returns400()
    {
        var response = await factory.CreateClient().PostAsJsonAsync("/api/auth/login", new { email = "not-an-email", password = "" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsUserAndStartsSession()
    {
        var client = factory.CreateClient();

        var login = await client.LoginAsync(HrSystemApiFactory.AdminEmail);
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        Assert.Contains(login.Headers.GetValues("Set-Cookie"), c => c.StartsWith("hr_system.auth=") && c.Contains("httponly"));

        var user = await login.Content.ReadFromJsonAsync<CurrentUser>();
        Assert.NotNull(user);
        Assert.Equal(HrSystemApiFactory.AdminEmail, user.Email);
        Assert.Equal(["Admin", "Employee"], user.Roles);

        // The cookie issued by the login request authenticates subsequent requests.
        var me = await client.GetFromJsonAsync<CurrentUser>("/api/auth/me");
        Assert.NotNull(me);
        Assert.Equal(user.Id, me.Id);
        Assert.Equal(user.EmployeeId, me.EmployeeId);
        Assert.Equal(user.Roles, me.Roles);
    }

    [Fact]
    public async Task Logout_EndsSession()
    {
        var client = factory.CreateClient();
        await client.LoginAsAsync(HrSystemApiFactory.EmployeeEmail);

        var logout = await client.PostAsync("/api/auth/logout", content: null);
        Assert.Equal(HttpStatusCode.NoContent, logout.StatusCode);

        var me = await client.GetAsync("/api/auth/me");
        Assert.Equal(HttpStatusCode.Unauthorized, me.StatusCode);
    }
}
