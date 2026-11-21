using System.Net;
using System.Net.Http.Json;
using HrSystem.Api.Tests.Infrastructure;

namespace HrSystem.Api.Tests;

public sealed class EmployeesTests(HrSystemApiFactory factory) : IClassFixture<HrSystemApiFactory>
{
    [Fact]
    public async Task GetMe_ReturnsProfileOfSignedInEmployee()
    {
        var client = factory.CreateClient();
        await client.LoginAsAsync(HrSystemApiFactory.EmployeeEmail);

        var employee = await client.GetFromJsonAsync<Employee>("/api/employees/me");

        Assert.NotNull(employee);
        Assert.Equal(HrSystemApiFactory.EmployeeEmail, employee.Email);
        Assert.Equal("Jan", employee.FirstName);
        Assert.True(employee.IsActive);
    }

    [Fact]
    public async Task GetById_UnknownEmployee_Returns404ProblemDetails()
    {
        var client = factory.CreateClient();
        await client.LoginAsAsync(HrSystemApiFactory.EmployeeEmail);

        var response = await client.GetAsync($"/api/employees/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetById_WithoutSession_Returns401()
    {
        var response = await factory.CreateClient().GetAsync($"/api/employees/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
