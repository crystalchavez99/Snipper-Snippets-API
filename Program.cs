using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using Snipper_Snippet_API.Controllers;
using Snipper_Snippet_API.Data;
using Snipper_Snippet_API.Middleware;
using Snipper_Snippet_API.Models;
using Snipper_Snippet_API.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddDbContext<SnippetContext>(opt =>
  //  opt.UseInMemoryDatabase("SnipperSnippets"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<EncryptUtility>();
builder.Services.AddSingleton<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<basicAuth>();

app.UseAuthorization();

app.MapControllers();

SnippetInitializer.Initialize(app.Services.GetRequiredService<IWebHostEnvironment>());

app.Run();
