using System.Text.Json.Serialization;
using API.Data;
using API.Data.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services
    .AddDbContext<DataContext>(options =>
        options.UseSqlite("Data Source=cosmos.db")
               .EnableSensitiveDataLogging())
        .AddScoped<PriceListRepo>()
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
        Enum = Enum.GetNames(typeof(SpaceCompany)).Select(name => new OpenApiString(name)).ToList<IOpenApiAny>()
    });

    c.MapType<Planet>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(Planet)).Select(name => new OpenApiString(name)).ToList<IOpenApiAny>()
    });

    c.MapType<SpaceCompany[]>(() => new OpenApiSchema
    {
        Type = "array",
        Items = new OpenApiSchema
        {
            Type = "string",
            Enum = Enum.GetNames(typeof(SpaceCompany)).Select(name => new OpenApiString(name)).ToList<IOpenApiAny>()
        }
    });
});

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder
        .SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));


var app = builder.Build();

using (var scope = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
using (var context = scope.ServiceProvider.GetService<DataContext>())
{
    context?.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();

