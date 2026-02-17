using EarthQuakeDataAutomate;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// Register HttpClient and Worker
builder.Services.AddHttpClient();
builder.Services.AddSingleton<DataAutomate>();
builder.Services.AddHostedService<Worker>();

var app = builder.Build();
app.Run();
