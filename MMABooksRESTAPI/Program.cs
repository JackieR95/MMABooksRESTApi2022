using Microsoft.Extensions.Options;
using MMABooksEFClasses.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .WithMethods("POST", "PUT", "DELETE", "GET", "OPTIONS")
              .AllowAnyHeader();
    });
});

// adding the dbContext to the service
builder.Services.AddDbContext<MMABooksContext>();

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

// In a production app you would want t turn this back on!
// app.UseHttpsRedirection();

// Enables the CORS policy
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
