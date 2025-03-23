using CustomerManagement.Infrastructure;
using CustomerManagement.Application;
using CustomerManagement.Presentation;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddPresentationServices();
services.AddApplicationServices();
services.AddInfrastructureServices();

var app = builder.Build();

app.ConfigurePresentation();

app.Run();

public partial class Program;
