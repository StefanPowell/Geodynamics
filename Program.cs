using EarthQuake.Repository;
using EarthQuake.USGS;
using Microsoft.Extensions.DependencyInjection;
using EarthQuake.Interface;
using EarthQuake.USGS.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var connectionString = config.GetConnectionString("DefaultConnection");


// Add services to the container.
builder.Services.AddSingleton<IApplicationRepoContext, ApplicationRepoContext>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    return new ApplicationRepoContext(config);
});
builder.Services.AddSingleton<IFaultRepository, FaultRepository>();
builder.Services.AddSingleton<IUSGSQUAKEAPI, USGSQUAKEAPI>(provider =>
{
    var applicationrepo = provider.GetRequiredService<IApplicationRepoContext>();
    return new USGSQUAKEAPI(applicationrepo);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


