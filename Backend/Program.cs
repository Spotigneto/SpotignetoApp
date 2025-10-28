using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel for proper port management
builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
    // Configure graceful shutdown timeout
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
});

// Configure explicit URLs to avoid port conflicts
builder.WebHost.UseUrls("http://localhost:5232");

// Configure shutdown timeout (moved to correct location)
builder.Host.ConfigureHostOptions(options =>
{
    options.ShutdownTimeout = TimeSpan.FromSeconds(30);
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework
builder.Services.AddDbContext<SpotigneteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Add repositories and services
builder.Services.AddScoped<ICanzoneRepository, CanzoneRepository>();
builder.Services.AddScoped<ICanzoneService, CanzoneService>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IAsAlbumCanzoneRepository, AsAlbumCanzoneRepository>();
builder.Services.AddScoped<IAsAlbumCanzoneService, AsAlbumCanzoneService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Avoid HTTPS redirect during local dev if HTTPS endpoint isn't configured
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors();

// Map controllers
app.MapControllers();

// Configure graceful shutdown with port cleanup
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    Console.WriteLine("Backend application is shutting down gracefully...");
    
    // Additional cleanup to ensure ports are released
    try
    {
        // Force garbage collection to help release resources
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during cleanup: {ex.Message}");
    }
});

app.Run();
