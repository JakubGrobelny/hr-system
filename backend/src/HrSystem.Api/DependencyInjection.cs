using System.Text.Json.Serialization;
using HrSystem.Api.ErrorHandling;
using HrSystem.Api.Security;
using HrSystem.Application.Abstractions;
using HrSystem.Application.Common.Security;
using HrSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HrSystem.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddOpenApi();
        services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        AddSecurity(services);

        return services;
    }

    private static void AddSecurity(IServiceCollection services)
    {
        // Cookie authentication backed by ASP.NET Core Identity. The SPA is served from the same
        // origin as the API, so an HttpOnly, SameSite=Strict cookie keeps the session out of reach
        // of JavaScript and is not sent with cross-site requests.
        services
            .AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "hr_system.auth";
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.SlidingExpiration = true;

            // This is an API: answer with status codes instead of redirecting to a login page.
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
        });

        services
            .AddAuthorizationBuilder()
            // Deny by default: every endpoint requires a signed-in user unless marked [AllowAnonymous].
            .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build())
            .AddPolicy(Policies.Manager, policy => policy.RequireRole(Roles.Manager))
            .AddPolicy(Policies.Hr, policy => policy.RequireRole(Roles.Hr))
            .AddPolicy(Policies.Admin, policy => policy.RequireRole(Roles.Admin));
    }
}
