using System.Net.Http.Json;

namespace HrSystem.Api.Tests.Infrastructure;

internal static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> LoginAsync(this HttpClient client, string email, string password = HrSystemApiFactory.Password)
    {
        return await client.PostAsJsonAsync("/api/auth/login", new { email, password });
    }

    public static async Task LoginAsAsync(this HttpClient client, string email)
    {
        var response = await client.LoginAsync(email);
        response.EnsureSuccessStatusCode();
    }
}
