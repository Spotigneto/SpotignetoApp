using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Backend.Data;
using Backend.Repositories;
using Backend.Services;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _env;

    public Startup(IConfiguration configuration, IHostEnvironment env)
    {
        _configuration = configuration;
        _env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext<SpotigneteDbContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
        });


        services.AddScoped<ICanzoneRepository, CanzoneRepository>();
        services.AddScoped<IPlaylistRepository, PlaylistRepository>();
        services.AddScoped<IAlbumRepository, AlbumRepository>();
        services.AddScoped<IArtistaRepository, ArtistaRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();


        services.AddScoped<ICanzoneService, CanzoneService>();
        services.AddScoped<IPlaylistService, PlaylistService>();
        services.AddScoped<IAlbumService, AlbumService>();
        services.AddScoped<IHomeService, HomeService>();
        services.AddScoped<INavigateService, NavigateService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IArtistaService, ArtistaService>();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (_env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.UseRouting();

        app.UseCors();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}