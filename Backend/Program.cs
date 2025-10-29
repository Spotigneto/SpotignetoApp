using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Backend.Data;
using Backend.Repositories;
using Backend.Repositories.IRepositories;
using Backend.Services;
using Backend.Services.IServices;

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

builder.Services.AddDbContext<SpotigneteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
builder.Services.AddScoped<IAsUtenteArtistaRepository, AsUtenteArtistaRepository>();
builder.Services.AddScoped<IAsUtenteArtistaService, AsUtenteArtistaService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IQueueService, QueueService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "Backend";
    });
}

// Avoid HTTPS redirect during local dev if HTTPS endpoint isn't configured
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors();

// Configure static file serving for audio files
app.UseStaticFiles();

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
