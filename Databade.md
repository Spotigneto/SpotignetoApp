# || Google Colab

https://colab.research.google.com/drive/1iiSSP39SaA02RaQyvDIcbbEiHxFWVo96?usp=sharing

Accedendo a questo link di Google Colab è possibile caricarci il data set proveniente dal file .xlsx e scremarlo così da avere dei file .csv contenenti solo le informazioni utili al fine del database.

Il Layer Bronze si ottiene nel momento in cui lo script verifica che il file non contenga né duplicati né righe nulle.

Si passa quindi al Layer Silver dove c'e stata una scrematura dei dati e conseguente rimozione delle colonne superflue.
Quindi alcuni dati sono stati adattati alle nostre necessità, che esso sia rinominare le colonne di genere e sottogenere oppure le conversioni temporali da millisecondi a secondi e minuti + secondi

Infine per ridurre le dimensioni dei file, nel layer gold è stata fatta una divisione delle canzoni per genere.

L'ultimo blocco di codice permette il download delle 3 entità forti provenienti dal Dataset: Canzone, Artista e Album tramite file .csv
Garantisce inoltre l'automatizzazione della scrittura delle query SQL di Insert delle stesse entità, le query sono scritte in file .txt, pronte ad essere copiate ed incollate su Sql Server Management Studio ed essere eseguite

Facendo ciò il Database inizia ad essere popolato tramite i dati provenienti dal Dataset.

# || Spotigneto DB

## | SET UP DATABASE

- Aprire Sql Server Management Studio
- Aprire un nuovo Database (Nome preferibile Spotigneto)
- Incollare le query sottostanti

## | QUERY CREAZIONE TABELLE (Entity / Tipologiche)

CREATE TABLE Utente( ut_id VARCHAR(255) PRIMARY KEY, ut_nome VARCHAR(100) NOT NULL, );

GO

CREATE TABLE Artista( ar_id VARCHAR(255) PRIMARY KEY, ar_nome VARCHAR(100) NOT NULL, );

GO

CREATE TABLE Playlist( pl_id VARCHAR(255) PRIMARY KEY, pl_nome VARCHAR(100) NOT NULL, pl_privata BIT NOT NULL DEFAULT 1 );

GO

CREATE TABLE Album( al_id VARCHAR(255) PRIMARY KEY, al_nome VARCHAR(100) NOT NULL, al_pubblica BIT NOT NULL DEFAULT 1, al_release_date DATE NULL );

GO

CREATE TABLE Canzone( ca_id VARCHAR(255) PRIMARY KEY, ca_nome VARCHAR(100) NOT NULL, ca_file VARCHAR(500) NULL, ca_genere VARCHAR(100) NOT NULL, ca_sottogenere VARCHAR(100) NOT NULL, ca_durata VARCHAR(10) NOT NULL, ca_secondi INT NOT NULL, );

GO

## | Query per Tabelle Associative

CREATE TABLE as_utente_playlist(
asup_id BIGINT IDENTITY(1,1) PRIMARY KEY,
asup_utente_fk VARCHAR(255) NOT NULL,
asup_playlist_fk VARCHAR(255) NOT NULL,
CONSTRAINT FK_Utente_Playlist FOREIGN KEY (asup_utente_fk) REFERENCES Utente(ut_id),
CONSTRAINT FK_Playlist_Utente FOREIGN KEY (asup_playlist_fk) REFERENCES Playlist(pl_id)
);

GO

CREATE TABLE as_canzone_playlist(
ascp_id BIGINT IDENTITY(1,1) PRIMARY KEY,
ascp_canzone_fk VARCHAR(255) NOT NULL,
ascp_playlist_fk VARCHAR(255) NOT NULL,
CONSTRAINT FK_Canzone_Playlist FOREIGN KEY (ascp_canzone_fk) REFERENCES Canzone(ca_id),
CONSTRAINT FK_Playlist_Canzone FOREIGN KEY (ascp_playlist_fk) REFERENCES Playlist(pl_id)
);

GO

CREATE TABLE as_artista_album(
asaa_id BIGINT IDENTITY(1,1) PRIMARY KEY,
asaa_artista_fk VARCHAR(255) NOT NULL,
asaa_album_fk VARCHAR(255) NOT NULL,
CONSTRAINT FK_Artista_Album FOREIGN KEY (asaa_artista_fk) REFERENCES Artista(ar_id),
CONSTRAINT FK_Album_Artista FOREIGN KEY (asaa_album_fk) REFERENCES Album(al_id)
);

GO

CREATE TABLE as_artista_canzone(
asarc_id BIGINT IDENTITY(1,1) PRIMARY KEY,
asarc_artista_fk VARCHAR(255) NOT NULL,
asarc_canzone_fk VARCHAR(255) NOT NULL,
CONSTRAINT FK_Artista_Canzone FOREIGN KEY (asarc_artista_fk) REFERENCES Artista(ar_id),
CONSTRAINT FK_Canzone_Artista FOREIGN KEY (asarc_canzone_fk) REFERENCES Canzone(ca_id)
);

GO

CREATE TABLE as_album_canzone(
asalc_id BIGINT IDENTITY(1,1) PRIMARY KEY,
asalc_canzone_fk VARCHAR(255) NOT NULL,
asalc_album_fk VARCHAR(255) NOT NULL,
CONSTRAINT FK_Canzone_Album FOREIGN KEY (asalc_canzone_fk) REFERENCES Canzone(ca_id),
CONSTRAINT FK_Album_Canzone FOREIGN KEY (asalc_album_fk) REFERENCES Album(al_id)
);

GO

CREATE TABLE as_utente_artista(
asua_id BIGINT IDENTITY(1,1) PRIMARY KEY,
asua_utente_fk VARCHAR(255) NOT NULL,
asua_artista_fk VARCHAR(255) NOT NULL,
CONSTRAINT FK_Utente_Artista FOREIGN KEY (asua_utente_fk) REFERENCES Utente(ut_id),
CONSTRAINT FK_Artista_Utente FOREIGN KEY (asua_artista_fk) REFERENCES Artista(ar_id)
);

GO
