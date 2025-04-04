using System.Text.Json.Serialization;
using API.BackgroundJobs;
using API.Data;
using API.Data.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

string? postgreSQLConnectionString = Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_DefaultConnection"); //POSTGRESQL for live

if (!string.IsNullOrWhiteSpace(postgreSQLConnectionString))
{
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseNpgsql(postgreSQLConnectionString));
}
else
{
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseSqlite("Data Source=localdatabase.db;")); //SQLite for local
}

builder.Services.AddScoped<PriceListRepo>()
    .AddScoped<LegsRepo>()
    .AddScoped<ProviderRepo>()
    .AddScoped<ReservationRepo>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<SpaceCompany>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = [.. Enum.GetNames(typeof(SpaceCompany)).Select(name => new OpenApiString(name))]
    });

    c.MapType<Planet>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = [.. Enum.GetNames(typeof(Planet)).Select(name => new OpenApiString(name))]
    });

    c.MapType<SpaceCompany[]>(() => new OpenApiSchema
    {
        Type = "array",
        Items = new OpenApiSchema
        {
            Type = "string",
            Enum = [.. Enum.GetNames(typeof(SpaceCompany)).Select(name => new OpenApiString(name))]
        }
    });
});
builder.Services.AddBackgroundJob();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder
        .SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<DataContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1;
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred applying migrations.");
    }
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cosmos Odyssey API v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.MapIdentityApi<User>();

app.UseCors("MyPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();