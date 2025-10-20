
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Parking.Api.Data;
using Parking.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL connection
var conn = builder.Configuration.GetConnectionString("Postgres")
           ?? "Host=localhost;Port=5432;Database=parking_teste;Username=postgres;Password=user123";

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(conn);
});

builder.Services.AddScoped<PlacaService>();
builder.Services.AddScoped<FaturamentoService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Parking API", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // porta do Angular
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parking API v1");
    c.RoutePrefix = "swagger"; // garante que /swagger abra a UI
});

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseCors("AllowFrontend");
app.MapControllers();

app.Run();
