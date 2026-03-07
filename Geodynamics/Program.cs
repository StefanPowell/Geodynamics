using EarthQuake;
using EarthQuake.Extensions;
using EarthQuake.SignalR;
using EarthQuake.USGS;
using EarthQuake.USGS.Interfaces;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddEarthQuakeServices(builder.Configuration);


builder.Services.AddHttpClient<IUSGSQUAKEAPI, USGSQUAKEAPI>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.BaseAddress = new Uri("https://earthquake.usgs.gov/fdsnws/event/1/");
});

// Add built-in services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddHostedService<Worker>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5214", "https://localhost:44351", "https://localhost:7184")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // your Blazor app URL
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EarthQuake API V1");
        c.RoutePrefix = string.Empty; // Optional: serve Swagger at root /
    });
}

//app.UseHttpsRedirection();

app.UseRouting(); 
app.UseCors();
app.UseAuthorization();

app.MapControllers();
app.MapHub<EarthquakeHub>("/EarthquakeHub");

app.Run();
