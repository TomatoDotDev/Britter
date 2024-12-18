using Scalar.AspNetCore;
using Britter.DataAccess.Extensions;
using Microsoft.AspNetCore.Identity;
using Britter.DataAccess.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddDatabaseServices().AddIdentityApiEndpoints<BritterUser>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Britter API")
        .WithTheme(ScalarTheme.DeepSpace)
        .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch)
        .WithForceThemeMode(ThemeMode.Dark);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapIdentityApi<BritterUser>();

app.Run();
