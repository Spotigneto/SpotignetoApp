using Microsoft.EntityFrameworkCore;
using Backend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Entity Framework
builder.Services.AddDbContext<SpotignetoDbContext>(options =>
            options.UseSqlServer(defaultConn));
try
{
    using var c = new SqlConnection(defaultConn);
    c.Open();
    var sql = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Utente' AND schema_id=SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE Utente(
    ut_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    ut_nome VARCHAR(100) NOT NULL
);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Artista' AND schema_id=SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE Artista(
    ar_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    ar_nome VARCHAR(100) NOT NULL
);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Playlist' AND schema_id=SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE Playlist(
    pl_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    pl_nome VARCHAR(100) NOT NULL,
    pl_privata BIT NOT NULL DEFAULT 1
);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Album' AND schema_id=SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE Album(
    al_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    al_nome VARCHAR(100) NOT NULL,
    al_pubblica BIT NOT NULL DEFAULT 1,
    al_release_date DATE NULL
);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Genere_tp' AND schema_id=SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE Genere_tp(
    gtp_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    gtp_genere VARCHAR(100) NOT NULL
);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Sottogenere_tp' AND schema_id=SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE Sottogenere_tp(
    stp_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    stp_sottogenere VARCHAR(100) NOT NULL
);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Canzone' AND schema_id=SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE Canzone(
    ca_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    ca_nome VARCHAR(100) NOT NULL,
    ca_file VARCHAR(500) NOT NULL,
    ca_genere BIGINT NOT NULL,
    ca_sottogenere BIGINT NOT NULL,
    ca_durata INT NOT NULL,
    CONSTRAINT FK_Canzone_Genere FOREIGN KEY (ca_genere) REFERENCES Genere_tp(gtp_id),
    CONSTRAINT FK_Canzone_Sottogenere FOREIGN KEY (ca_sottogenere) REFERENCES Sottogenere_tp(stp_id)
);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='as_utente_playlist' AND schema_id=SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE as_utente_playlist(
    asup_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    asup_utente_fk BIGINT NOT NULL,
    asup_playlist_fk BIGINT NOT NULL,
    CONSTRAINT FK_Utente_Playlist FOREIGN KEY (asup_utente_fk) REFERENCES Utente(ut_id),
    CONSTRAINT FK_Playlist_Utente FOREIGN KEY (asup_playlist_fk) REFERENCES Playlist(pl_id)
);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='as_canzone_playlist' AND schema_id=SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE as_canzone_playlist(
    ascp_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    ascp_canzone_fk BIGINT NOT NULL,
    ascp_playlist_fk BIGINT NOT NULL,
    CONSTRAINT FK_Canzone_Playlist FOREIGN KEY (ascp_canzone_fk) REFERENCES Canzone(ca_id),
    CONSTRAINT FK_Playlist_Canzone FOREIGN KEY (ascp_playlist_fk) REFERENCES Playlist(pl_id)
);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='as_artista_album' AND schema_id=SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE as_artista_album(
    asaa_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    asaa_artista_fk BIGINT NOT NULL,
    asaa_album_fk BIGINT NOT NULL,
    CONSTRAINT FK_Artista_Album FOREIGN KEY (asaa_artista_fk) REFERENCES Artista(ar_id),
    CONSTRAINT FK_Album_Artista FOREIGN KEY (asaa_album_fk) REFERENCES Album(al_id)
);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='as_artista_canzone' AND schema_id=SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE as_artista_canzone(
    asarc_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    asarc_artista_fk BIGINT NOT NULL,
    asarc_canzone_fk BIGINT NOT NULL,
    CONSTRAINT FK_Artista_Canzone FOREIGN KEY (asarc_artista_fk) REFERENCES Artista(ar_id),
    CONSTRAINT FK_Canzone_Artista FOREIGN KEY (asarc_canzone_fk) REFERENCES Canzone(ca_id)
);
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='as_album_canzone' AND schema_id=SCHEMA_ID('dbo'))
BEGIN
CREATE TABLE as_album_canzone(
    asalc_id BIGINT IDENTITY(1,1) PRIMARY KEY,
    asalc_canzone_fk BIGINT NOT NULL,
    asalc_album_fk BIGINT NOT NULL,
    CONSTRAINT FK_Canzone_Album FOREIGN KEY (asalc_canzone_fk) REFERENCES Canzone(ca_id),
    CONSTRAINT FK_Album_Canzone FOREIGN KEY (asalc_album_fk) REFERENCES Album(al_id)
);
END";
    using var cmd = new SqlCommand(sql, c);
    cmd.ExecuteNonQuery();
    c.Close();
}
catch
{
}
}

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}