using FluentValidation;
using TestContainersArticle.Host.Configuration;
using TestContainersArticle.Main.Configuration;
using TestContainersArticle.Main.Data;
using TestContainersArticle.Main.Features.CreateArticle;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment.EnvironmentName;
Console.WriteLine(environment);

builder
    .Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

builder.Services.SetupConfigFiles();

var assemblyToScan = typeof(CreateArticleRequest).Assembly;
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assemblyToScan));
builder.Services.AddValidatorsFromAssemblyContaining<CreateArticleRequest>();

builder.Services.SetupDatabase<DataContext>();
builder.Services.SetupControllers();
builder.Services.AddSwagger();
builder.Services.SetupFilters();

builder.Services.RegisterServices();
builder.Services.SetupJsonOptions();

var app = builder.Build();
app.RegisterSwagger();
app.RegisterMiddleware();

await app.ApplyMigrationsAsync<DataContext>();
app.Run();

// this is here for integration tests
public partial class Program;
