using Frontend.Components;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel for proper port management
builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
    // Configure graceful shutdown timeout
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
});

//// Configure explicit URLs to avoid port conflicts
//builder.WebHost.UseUrls("http://localhost:5001", "https://localhost:7007");

// Configure shutdown timeout (moved to correct location)
builder.Host.ConfigureHostOptions(options =>
{
    options.ShutdownTimeout = TimeSpan.FromSeconds(30);
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add HttpClient service for API calls
builder.Services.AddHttpClient("ApiClient", client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5232";
    client.BaseAddress = new Uri(baseUrl);
});

// Register HttpClient for dependency injection
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));

// Playback service as singleton for shared queue and state
builder.Services.AddSingleton<Frontend.Services.Playback.PlaybackService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Configure graceful shutdown with port cleanup
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    Console.WriteLine("Frontend application is shutting down gracefully...");
    
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
