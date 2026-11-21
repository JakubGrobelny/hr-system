using System.Net;
using System.Net.Http.Json;
using HrSystem.Api.Tests.Infrastructure;

namespace HrSystem.Api.Tests;

public sealed class AdminUsersTests(HrSystemApiFactory factory) : IClassFixture<HrSystemApiFactory>
{
    [Fact]
    public async Task GetUsers_AsEmployee_Returns403()
    {
        var client = factory.CreateClient();
        await client.LoginAsAsync(HrSystemApiFactory.EmployeeEmail);

        var response = await client.GetAsync("/api/admin/users");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetUsers_AsAdmin_ReturnsSeededAccountsWithRoles()
    {
        var client = factory.CreateClient();
        await client.LoginAsAsync(HrSystemApiFactory.AdminEmail);

        var users = await client.GetFromJsonAsync<UserSummary[]>("/api/admin/users");

        Assert.NotNull(users);
        Assert.Equal(4, users.Length);
        Assert.Equal(["Employee", "Hr"], users.Single(u => u.Email == "hr@hr.local").Roles);
        Assert.Equal(["Employee", "Manager"], users.Single(u => u.Email == "manager@hr.local").Roles);
        Assert.All(users, u => Assert.False(u.IsLockedOut));
    }
}
