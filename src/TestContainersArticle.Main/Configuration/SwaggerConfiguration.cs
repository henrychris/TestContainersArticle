using Microsoft.OpenApi.Models;

namespace TestContainersArticle.Host.Configuration;

public static class SwaggerConfiguration
{
    private const string SECURITY_SCHEME = "Bearer";

    public static void AddSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // setup swagger to accept bearer tokens
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TestArticleDemo API",
                    Description = "An API to demonstrate testing with TestContainers and Respawn!"
                }
            );

            options.AddSecurityDefinition(
                SECURITY_SCHEME,
                new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = SECURITY_SCHEME
                }
            );
        });
    }

    public static void RegisterSwagger(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }

        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
