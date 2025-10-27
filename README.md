# SpotignetoApp

SPOTIGNETO PROJECT WORK
VISTE
| Home
| Profilo
| Navigazione
| Album
| Playlist
| Canzone

METODI
| Random_View (Home)
| Search_Filtri + Lettere(Navigazione)
| Search_Filtri (Album/Playlist)
| Coda,Random,Loop x Playlist,Canzoni,Album
| Aggiungere a Playlist(Canzone)

CONTROLLER
| HomeC
| ProfiloC
| NavigazioneC
| AlbumC
| PlaylistC
| CanzoneC

# Spotigneto DB

| SET UP DATABASE

- Aprire Sql Server Management Studio
- Aprire un nuovo Database (Nome preferibile Spotigneto)
- Incollare le query sottostanti

| QUERY CREAZIONE TABELLE (Entity / Tipologiche)
CREATE TABLE Utente(
ut_id BIGINT IDENTITY(1,1) PRIMARY KEY,
ut_nome VARCHAR(100) NOT NULL,
);
GO

CREATE TABLE Artista(
ar_id BIGINT IDENTITY(1,1) PRIMARY KEY,
ar_nome VARCHAR(100) NOT NULL,
);

GO

CREATE TABLE Playlist(
pl_id BIGINT IDENTITY(1,1) PRIMARY KEY,
pl_nome VARCHAR(100) NOT NULL,
pl_privata BIT NOT NULL DEFAULT 1
);

GO

CREATE TABLE Album(
al_id BIGINT IDENTITY(1,1) PRIMARY KEY,
al_nome VARCHAR(100) NOT NULL,
al_pubblica BIT NOT NULL DEFAULT 1,
al_release_date DATE NULL
);

GO

CREATE TABLE Genere_tp(
gtp_id BIGINT IDENTITY(1,1) PRIMARY KEY,
gtp_genere VARCHAR(100) NOT NULL
);

CREATE TABLE Sottogenere_tp(
stp_id BIGINT IDENTITY(1,1) PRIMARY KEY,
stp_sottogenere VARCHAR(100) NOT NULL
);


GO

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

GO

| QUERY CREAZIONE ASSOCIATIVE

CREATE TABLE as_utente_playlist(
asup_id BIGINT IDENTITY(1,1) PRIMARY KEY,
asup_utente_fk BIGINT NOT NULL,
asup_playlist_fk BIGINT NOT NULL,
CONSTRAINT FK_Utente_Playlist FOREIGN KEY (asup_utente_fk) REFERENCES Utente(ut_id),
CONSTRAINT FK_Playlist_Utente FOREIGN KEY (asup_playlist_fk) REFERENCES Playlist(pl_id)
);

GO

CREATE TABLE as_canzone_playlist(
ascp_id BIGINT IDENTITY(1,1) PRIMARY KEY,
ascp_canzone_fk BIGINT NOT NULL,
ascp_playlist_fk BIGINT NOT NULL,
CONSTRAINT FK_Canzone_Playlist FOREIGN KEY (ascp_canzone_fk) REFERENCES Canzone(ca_id),
CONSTRAINT FK_Playlist_Canzone FOREIGN KEY (ascp_playlist_fk) REFERENCES Playlist(pl_id)
);

GO

CREATE TABLE as_artista_album(
asaa_id BIGINT IDENTITY(1,1) PRIMARY KEY,
asaa_artista_fk BIGINT NOT NULL,
asaa_album_fk BIGINT NOT NULL,
CONSTRAINT FK_Artista_Album FOREIGN KEY (asaa_artista_fk) REFERENCES Artista(ar_id),
CONSTRAINT FK_Album_Artista FOREIGN KEY (asaa_album_fk) REFERENCES Album(al_id)
);

GO

CREATE TABLE as_artista_canzone(
asarc_id BIGINT IDENTITY(1,1) PRIMARY KEY,
asarc_artista_fk BIGINT NOT NULL,
asarc_canzone_fk BIGINT NOT NULL,
CONSTRAINT FK_Artista_Canzone FOREIGN KEY (asarc_artista_fk) REFERENCES Artista(ar_id),
CONSTRAINT FK_Canzone_Artista FOREIGN KEY (asarc_canzone_fk) REFERENCES Canzone(ca_id)
);

GO

CREATE TABLE as_album_canzone(
asalc_id BIGINT IDENTITY(1,1) PRIMARY KEY,
asalc_canzone_fk BIGINT NOT NULL,
asalc_album_fk BIGINT NOT NULL,
CONSTRAINT FK_Canzone_Album FOREIGN KEY (asalc_canzone_fk) REFERENCES Canzone(ca_id),
CONSTRAINT FK_Album_Canzone FOREIGN KEY (asalc_album_fk) REFERENCES Album(al_id)
);

GO
