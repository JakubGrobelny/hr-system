using System.Net;
using HrSystem.Api.Tests.Infrastructure;

namespace HrSystem.Api.Tests;

public sealed class HealthTests(HrSystemApiFactory factory) : IClassFixture<HrSystemApiFactory>
{
    [Fact]
    public async Task Health_IsAvailableWithoutAuthentication()
    {
        var response = await factory.CreateClient().GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }
}
