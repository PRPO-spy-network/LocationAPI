using Locations.Models;
using Microsoft.EntityFrameworkCore;

var config = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.Build();

var builder = WebApplication.CreateBuilder(args);
string connectionString = config["TIMESCALE_CONN_STRING"] ?? throw new InvalidDataException("TIMESCALE_CONN_STRING ne obstaja");
builder.Services.AddDbContextFactory<PostgresContext>(options => options.UseNpgsql(connectionString));



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");
var logger = app.Logger;

if (app.Environment.IsDevelopment())
{
	logger.LogInformation("Running in dev mode");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
	app.UseHttpsRedirection();
	// app.UseAuthorization();
}

app.MapControllers();

app.Run();
