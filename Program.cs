using EarthQuake.Extensions;
using EarthQuake.USGS;
using EarthQuake.USGS.Interfaces;

var builder = WebApplication.CreateBuilder(args);


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

app.UseHttpsRedirection();

app.UseRouting(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
