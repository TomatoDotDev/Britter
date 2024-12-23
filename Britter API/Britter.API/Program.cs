using Scalar.AspNetCore;
using Britter.DataAccess.Extensions;
using Britter.DataAccess.Seeder;
using Microsoft.AspNetCore.Identity;
using Britter.DataAccess.Context;
using Britter.DataAccess.Models;
using Britter.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();
builder.Services.AddDatabaseServices();
builder.Services.AddIdentityCore<BritterUser>(options =>
{
    options.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<BritterDBContext>()
                .AddApiEndpoints();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options => 
{ 
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); 
});

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.SeedAdminAccountAsync(services);
}

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
app.UseAuthentication();

app.MapControllers();
app.MapIdentityApi<BritterUser>();
app.MapIdentityApiAdditionalEndpoints<BritterUser>();

app.Run();
