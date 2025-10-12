using EarthQuake.Extensions;
using EarthQuake.Persistence.Extensions;
using EarthQuake.USGS;
using EarthQuake.USGS.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEarthQuakeServices(builder.Configuration);

builder.Services.AddHttpClient<IUSGSQUAKEAPI, USGSQUAKEAPI>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
