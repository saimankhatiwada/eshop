using System.Xml.Linq;
using Application.Abstractions.Authentication;
using Application.Abstractions.Clock;
using Application.Abstractions.Data;
using Application.Abstractions.Email;
using Domain.Abstractions;
using Domain.Products;
using Domain.Reviews;
using Domain.Users;
using Infrastructure.Authentication;
using Infrastructure.Clock;
using Infrastructure.Data;
using Infrastructure.Email;
using Infrastructure.Outbox;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        
        AddMail(services, configuration);

        AddPresistence(services, configuration);

        AddAuthentication(services, configuration);

        AddBackgroundJobs(services, configuration);

        return services;
    }

    private static void AddMail(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailOptions>(configuration.GetSection("Mail"));
        services.AddTransient<IEmailService, EmailService>();
    }

    private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));

        #pragma warning disable CS0618
        services.AddQuartz(options => 
        {
            options.UseMicrosoftDependencyInjectionJobFactory();
        });
        #pragma warning restore

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.ConfigureOptions<ProcessOutboxMessagesJobSetup>();
    }

    private static void AddPresistence(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = 
            configuration.GetConnectionString("Database") ??
            throw new ArgumentNullException(nameof(configuration));
        
        services.AddDbContext<ApplicationDbContext>(options => 
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IReviewRepository, ReviewRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(connectionString));
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));

        services.ConfigureOptions<JwtBearerOptionSetup>();

        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

        services.AddTransient<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpClient) =>
            {
                var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
            })
            .AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            httpClient.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });
    }
}
