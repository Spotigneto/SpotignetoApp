# Documentazione API - Spotigneto

## Panoramica
Questa documentazione descrive le API REST disponibili per l'applicazione Spotigneto, compatibili con la struttura del database esistente.

## Entità Supportate
- **Canzone**: Gestione delle canzoni
- **Album**: Gestione degli album
- **Playlist**: Gestione delle playlist
- **AsCanzonePlaylist**: Relazioni tra canzoni e playlist

---

## 1. API CANZONE

### 1.1 Creare una Canzone
- **URL**: `POST /api/Canzone/canzone`
- **Funzione**: Crea una nuova canzone nel database
- **Dati di Input**:
  - [id]: string (esempio: "CAN001")
  - [nome]: string (esempio: "Bohemian Rhapsody")
  - [file]: string (esempio: "audio/bohemian_rhapsody.mp3")
  - [genere]: string (esempio: "Rock")
  - [sottogenere]: string (esempio: "Progressive Rock")
  - [durata]: string (esempio: "05:55")
  - [secondi]: int (esempio: 355)

### 1.2 Ottenere Tutte le Canzoni
- **URL**: `GET /api/Canzone/canzoni`
- **Funzione**: Recupera l'elenco di tutte le canzoni
- **Dati di Input**: Nessuno
- **Risposta**:
```json
[
  {
    "id": 1,
    "nome": "Nome della canzone",
    "file": "percorso/del/file.mp3",
    "genereId": 1,
    "sottogenereId": 1,
    "durata": "03:45",
    "secondi": 225
  }
]
```

### 1.3 Ottenere una Canzone per ID
- **URL**: `GET /api/Canzone/canzone/{id}`
- **Funzione**: Recupera una canzone specifica tramite ID
- **Dati di Input**: 
  - [id]: string (esempio: "CAN001") - nell'URL

### 1.4 Ottenere una Canzone per Nome
- **URL**: `GET /api/Canzone/canzone/by-name/{name}`
- **Funzione**: Recupera una canzone specifica tramite nome
- **Dati di Input**: 
  - [name]: string (esempio: "Bohemian Rhapsody") - nell'URL

### 1.5 Aggiornare una Canzone
- **URL**: `PUT /api/Canzone/canzone/{id}`
- **Funzione**: Aggiorna i dati di una canzone esistente
- **Dati di Input**: 
  - [id]: string (esempio: "CAN001") - nell'URL
  - [nome]: string (esempio: "Bohemian Rhapsody")
  - [file]: string (esempio: "audio/bohemian_rhapsody.mp3")
  - [genere]: string (esempio: "Rock")
  - [sottogenere]: string (esempio: "Progressive Rock")
  - [durata]: string (esempio: "05:55")
  - [secondi]: int (esempio: 355)

### 1.6 Eliminare una Canzone
- **URL**: `DELETE /api/Canzone/canzone/{id}`
- **Funzione**: Elimina una canzone dal database
- **Dati di Input**: 
  - [id]: string (esempio: "CAN001") - nell'URL

---

## 2. API ALBUM

### 2.1 Creare un Album
- **URL**: `POST /api/Records/album`
- **Funzione**: Crea un nuovo album nel database
- **Dati di Input**:
  - [id]: string (esempio: "ALB001")
  - [nome]: string (esempio: "A Night at the Opera")
  - [pubblica]: bool (esempio: true)
  - [releaseDate]: DateTime? (esempio: "1975-11-21T00:00:00Z")

### 2.2 Ottenere Tutti gli Album
- **URL**: `GET /api/Records/albums`
- **Funzione**: Recupera l'elenco di tutti gli album
- **Dati di Input**: Nessuno

### 2.3 Ottenere un Album per ID
- **URL**: `GET /api/Records/albums/{id}`
- **Funzione**: Recupera un album specifico tramite ID
- **Dati di Input**: 
  - [id]: string (esempio: "ALB001") - nell'URL

### 2.4 Aggiornare un Album
- **URL**: `PUT /api/Records/albums/{id}`
- **Funzione**: Aggiorna i dati di un album esistente
- **Dati di Input**: 
  - [id]: string (esempio: "ALB001") - nell'URL
  - [nome]: string (esempio: "A Night at the Opera")
  - [pubblica]: bool (esempio: true)
  - [releaseDate]: DateTime? (esempio: "1975-11-21T00:00:00Z")

### 2.5 Eliminare un Album
- **URL**: `DELETE /api/Records/albums/{id}`
- **Funzione**: Elimina un album dal database
- **Dati di Input**: 
  - [id]: string (esempio: "ALB001") - nell'URL

---

## 3. API PLAYLIST

### 3.1 Creare una Playlist
- **URL**: `POST /api/Records/playlist`
- **Funzione**: Crea una nuova playlist nel database
- **Dati di Input**:
  - [id]: string (esempio: "PL001")
  - [nome]: string (esempio: "My Favorite Songs")
  - [privata]: bool (esempio: false)

### 3.2 Ottenere Tutte le Playlist
- **URL**: `GET /api/Records/playlists`
- **Funzione**: Recupera l'elenco di tutte le playlist
- **Dati di Input**: Nessuno

### 3.3 Ottenere una Playlist per ID
- **URL**: `GET /api/Records/playlists/{id}`
- **Funzione**: Recupera una playlist specifica tramite ID
- **Dati di Input**: 
  - [id]: string (esempio: "PL001") - nell'URL

### 3.4 Aggiornare una Playlist
- **URL**: `PUT /api/Records/playlists/{id}`
- **Funzione**: Aggiorna i dati di una playlist esistente
- **Dati di Input**: 
  - [id]: string (esempio: "PL001") - nell'URL
  - [nome]: string (esempio: "My Favorite Songs")
  - [privata]: bool (esempio: false)

### 3.5 Eliminare una Playlist
- **URL**: `DELETE /api/Records/playlists/{id}`
- **Funzione**: Elimina una playlist dal database
- **Dati di Input**: 
  - [id]: string (esempio: "PL001") - nell'URL

---

## 4. API RELAZIONI CANZONE-PLAYLIST

### 4.1 Aggiungere Canzone a Playlist
- **URL**: `POST /api/AsCanzonePlaylist/AddTrackToPlaylist`
- **Funzione**: Aggiunge una canzone a una playlist
- **Dati di Input**:
  - [acPlaylistId]: string (esempio: "PL001")
  - [acCanzoneId]: string (esempio: "CAN001")
  - [trackOrder]: int? (esempio: 1) - opzionale

### 4.2 Ottenere Tutte le Relazioni
- **URL**: `GET /api/AsCanzonePlaylist`
- **Funzione**: Recupera tutte le relazioni canzone-playlist
- **Dati di Input**: Nessuno

### 4.3 Ottenere Relazione per ID
- **URL**: `GET /api/AsCanzonePlaylist/{id}`
- **Funzione**: Recupera una relazione specifica tramite ID
- **Dati di Input**: 
  - [id]: long (esempio: 1) - nell'URL

### 4.4 Aggiornare Canzone in Playlist
- **URL**: `PUT /api/AsCanzonePlaylist/UpdateTrackInPlaylist`
- **Funzione**: Aggiorna una canzone in una playlist (sostituisce una canzone con un'altra)
- **Dati di Input**:
  - [acPlaylistId]: string (esempio: "PL001")
  - [oldCanzoneId]: string (esempio: "CAN001")
  - [newCanzoneId]: string (esempio: "CAN002")

### 4.5 Rimuovere Canzone da Playlist
- **URL**: `DELETE /api/AsCanzonePlaylist/RemoveTrackFromPlaylist`
- **Funzione**: Rimuove una canzone da una playlist
- **Dati di Input**:
  - [acPlaylistId]: string (esempio: "PL001")
  - [acCanzoneId]: string (esempio: "CAN001")

---

## Coda (Queue)

### Ottenere la Coda Corrente
- **URL**: `/api/Queue`
- **Metodo**: `GET`
- **Funzione**: Restituisce la coda di riproduzione corrente
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: 
  ```json
  {
    "id": "queue_001",
    "items": [...],
    "currentIndex": 0,
    "mode": 0,
    "isShuffled": false,
    "shuffleOrder": []
  }
  ```

### Impostare Coda da Playlist
- **URL**: `/api/Queue/playlist/{id}`
- **Metodo**: `POST`
- **Funzione**: Imposta la coda di riproduzione da una playlist
- **Dati in Input**:
  - id: string (playlist_001)
- **Esempio di Risposta**: `200 OK`

### Impostare Coda da Album
- **URL**: `/api/Queue/album/{id}`
- **Metodo**: `POST`
- **Funzione**: Imposta la coda di riproduzione da un album
- **Dati in Input**:
  - id: string (album_001)
- **Esempio di Risposta**: `200 OK`

### Aggiungere Canzone alla Coda
- **URL**: `/api/Queue/add/{canzoneId}`
- **Metodo**: `POST`
- **Funzione**: Aggiunge una canzone alla coda di riproduzione
- **Dati in Input**:
  - canzoneId: string (canzone_001)
- **Esempio di Risposta**: `200 OK`

### Rimuovere Canzone dalla Coda
- **URL**: `/api/Queue/{index}`
- **Metodo**: `DELETE`
- **Funzione**: Rimuove una canzone dalla coda per indice
- **Dati in Input**:
  - index: int (0)
- **Esempio di Risposta**: `200 OK`

### Svuotare la Coda
- **URL**: `/api/Queue`
- **Metodo**: `DELETE`
- **Funzione**: Svuota completamente la coda di riproduzione
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: `200 OK`

### Riordinare la Coda
- **URL**: `/api/Queue/reorder`
- **Metodo**: `PUT`
- **Funzione**: Riordina gli elementi nella coda
- **Dati in Input**:
  - fromIndex: int (0)
  - toIndex: int (2)
- **Esempio di Input**:
  ```json
  {
    "fromIndex": 0,
    "toIndex": 2
  }
  ```

---

## Controlli Player

### Ottenere Stato del Player
- **URL**: `/api/Queue/player/state`
- **Metodo**: `GET`
- **Funzione**: Restituisce lo stato corrente del player
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: 
  ```json
  {
    "isPlaying": true,
    "isPaused": false,
    "currentCanzoneId": "canzone_001",
    "currentPosition": 45,
    "volume": 0.8,
    "mode": 0
  }
  ```

### Avviare Riproduzione
- **URL**: `/api/Queue/player/play`
- **Metodo**: `POST`
- **Funzione**: Avvia la riproduzione
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: `200 OK`

### Mettere in Pausa
- **URL**: `/api/Queue/player/pause`
- **Metodo**: `POST`
- **Funzione**: Mette in pausa la riproduzione
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: `200 OK`

### Fermare Riproduzione
- **URL**: `/api/Queue/player/stop`
- **Metodo**: `POST`
- **Funzione**: Ferma la riproduzione
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: `200 OK`

### Traccia Successiva
- **URL**: `/api/Queue/player/next`
- **Metodo**: `POST`
- **Funzione**: Passa alla traccia successiva
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: `200 OK`

### Traccia Precedente
- **URL**: `/api/Queue/player/previous`
- **Metodo**: `POST`
- **Funzione**: Passa alla traccia precedente
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: `200 OK`

### Posizionare Riproduzione
- **URL**: `/api/Queue/player/seek`
- **Metodo**: `POST`
- **Funzione**: Posiziona la riproduzione a un punto specifico
- **Dati in Input**:
  - positionSeconds: int (120)
- **Esempio di Input**:
  ```json
  {
    "positionSeconds": 120
  }
  ```

### Impostare Volume
- **URL**: `/api/Queue/player/volume`
- **Metodo**: `POST`
- **Funzione**: Imposta il volume di riproduzione
- **Dati in Input**:
  - volume: float (0.8)
- **Esempio di Input**:
  ```json
  {
    "volume": 0.8
  }
  ```

### Impostare Modalità di Riproduzione
- **URL**: `/api/Queue/mode/{mode}`
- **Metodo**: `POST`
- **Funzione**: Imposta la modalità di riproduzione (0=Sequential, 1=Loop, 2=LoopSingle, 3=Random)
- **Dati in Input**:
  - mode: int (1)
- **Esempio di Risposta**: `200 OK`

### Attivare/Disattivare Shuffle
- **URL**: `/api/Queue/shuffle/toggle`
- **Metodo**: `POST`
- **Funzione**: Attiva o disattiva la modalità shuffle
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: `200 OK`

### Attivare/Disattivare Repeat
- **URL**: `/api/Queue/repeat/toggle`
- **Metodo**: `POST`
- **Funzione**: Attiva o disattiva la modalità repeat
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: `200 OK`

### Riprodurre Traccia per Indice
- **URL**: `/api/Queue/play/{index}`
- **Metodo**: `POST`
- **Funzione**: Riproduce una traccia specifica per indice nella coda
- **Dati in Input**:
  - index: int (2)
- **Esempio di Risposta**: `200 OK`

### Ottenere Traccia Corrente
- **URL**: `/api/Queue/current-track`
- **Metodo**: `GET`
- **Funzione**: Restituisce la traccia attualmente in riproduzione
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: 
  ```json
  {
    "id": "canzone_001",
    "nome": "Bohemian Rhapsody",
    "file": "bohemian_rhapsody.mp3",
    "genere": "Rock",
    "sottogenere": "Progressive Rock",
    "durata": "5:55",
    "secondi": 355
  }
  ```

### Ottenere Traccia Successiva
- **URL**: `/api/Queue/next-track`
- **Metodo**: `GET`
- **Funzione**: Restituisce la prossima traccia nella coda
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: 
  ```json
  {
    "id": "canzone_002",
    "nome": "Another One Bites the Dust",
    "file": "another_one_bites_the_dust.mp3",
    "genere": "Rock",
    "sottogenere": "Funk Rock",
    "durata": "3:35",
    "secondi": 215
  }
  ```

### Ottenere Traccia Precedente
- **URL**: `/api/Queue/previous-track`
- **Metodo**: `GET`
- **Funzione**: Restituisce la traccia precedente nella coda
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: 
  ```json
  {
    "id": "canzone_003",
    "nome": "We Will Rock You",
    "file": "we_will_rock_you.mp3",
    "genere": "Rock",
    "sottogenere": "Arena Rock",
    "durata": "2:02",
    "secondi": 122
  }
  ```

---

## Ricerca e Navigazione

### Ricerca con Filtri
- **URL**: `/api/Navigate/Search_Filtri`
- **Metodo**: `GET`
- **Funzione**: Ricerca canzoni, playlist e artisti con filtri opzionali
- **Dati in Input**:
  - q: string (bohemian) - Query di ricerca (opzionale)
  - genereId: string (rock_001) - ID del genere (opzionale)
  - sottoGenereId: string (progressive_001) - ID del sottogenere (opzionale)
  - playlistPrivata: bool (false) - Filtra playlist private (opzionale)
- **Esempio di Risposta**: 
  ```json
  {
    "songs": [
      {
        "id": "canzone_001",
        "nome": "Bohemian Rhapsody"
      }
    ],
    "playlists": [
      {
        "id": "playlist_001",
        "nome": "Rock Classics"
      }
    ],
    "artists": [
      {
        "id": "artista_001",
        "nome": "Queen"
      }
    ]
  }
  ```

---

## Relazioni Utente-Artista

### Creare Relazione Utente-Artista
- **URL**: `/api/AsUtenteArtista`
- **Metodo**: `POST`
- **Funzione**: Crea una nuova relazione tra utente e artista (follow)
- **Dati in Input**:
  - utenteId: string (utente_001)
  - artistaId: string (artista_001)
- **Esempio di Input**:
  ```json
  {
    "utenteId": "utente_001",
    "artistaId": "artista_001"
  }
  ```

### Ottenere Tutte le Relazioni Utente-Artista
- **URL**: `/api/AsUtenteArtista`
- **Metodo**: `GET`
- **Funzione**: Restituisce tutte le relazioni utente-artista
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: 
  ```json
  [
    {
      "utenteId": "utente_001",
      "artistaId": "artista_001"
    }
  ]
  ```

### Ottenere Relazioni per Utente
- **URL**: `/api/AsUtenteArtista/utente/{utenteId}`
- **Metodo**: `GET`
- **Funzione**: Restituisce tutti gli artisti seguiti da un utente
- **Dati in Input**:
  - utenteId: string (utente_001)
- **Esempio di Risposta**: 
  ```json
  [
    {
      "utenteId": "utente_001",
      "artistaId": "artista_001"
    }
  ]
  ```

### Ottenere Relazioni per Artista
- **URL**: `/api/AsUtenteArtista/artista/{artistaId}`
- **Metodo**: `GET`
- **Funzione**: Restituisce tutti gli utenti che seguono un artista
- **Dati in Input**:
  - artistaId: string (artista_001)
- **Esempio di Risposta**: 
  ```json
  [
    {
      "utenteId": "utente_001",
      "artistaId": "artista_001"
    }
  ]
  ```

### Ottenere Relazione Specifica
- **URL**: `/api/AsUtenteArtista/{utenteId}/{artistaId}`
- **Metodo**: `GET`
- **Funzione**: Restituisce una relazione specifica utente-artista
- **Dati in Input**:
  - utenteId: string (utente_001)
  - artistaId: string (artista_001)
- **Esempio di Risposta**: 
  ```json
  {
    "utenteId": "utente_001",
    "artistaId": "artista_001"
  }
  ```

### Verificare se Utente Segue Artista
- **URL**: `/api/AsUtenteArtista/isFollowing/{utenteId}/{artistaId}`
- **Metodo**: `GET`
- **Funzione**: Verifica se un utente segue un artista
- **Dati in Input**:
  - utenteId: string (utente_001)
  - artistaId: string (artista_001)
- **Esempio di Risposta**: `true`

### Smettere di Seguire Artista
- **URL**: `/api/AsUtenteArtista/unfollow/{utenteId}/{artistaId}`
- **Metodo**: `DELETE`
- **Funzione**: Rimuove la relazione di follow tra utente e artista
- **Dati in Input**:
  - utenteId: string (utente_001)
  - artistaId: string (artista_001)
- **Esempio di Risposta**: `200 OK`

---

## Database

### Test Connessione Database
- **URL**: `/api/Database/test-connection`
- **Metodo**: `GET`
- **Funzione**: Testa la connessione al database
- **Dati in Input**: Nessuno
- **Esempio di Risposta**: 
  ```json
  {
    "status": "Success",
    "message": "Database connection successful",
    "connectionString": "Server=localhost;Database=SpotigneteDB;User Id=***;Password=***;"
  }
  ```

---

## Codici di Risposta HTTP

- **200 OK**: Operazione completata con successo
- **201 Created**: Risorsa creata con successo
- **204 No Content**: Operazione completata, nessun contenuto da restituire
- **400 Bad Request**: Dati di input non validi
- **404 Not Found**: Risorsa non trovata
- **500 Internal Server Error**: Errore interno del server

## Note Tecniche

1. **Formato Date**: Le date devono essere nel formato ISO 8601 (YYYY-MM-DD)
2. **Formato Durata**: La durata deve essere nel formato MM:SS o HH:MM:SS
3. **Validazione**: Tutti i campi obbligatori devono essere forniti
4. **Encoding**: Utilizzare UTF-8 per tutti i contenuti testuali
5. **Content-Type**: Utilizzare `application/json` per tutte le richieste POST/PUT

## Limitazioni Attuali

- Non sono disponibili API per la gestione di Utenti e Artisti
- Non sono implementate le relazioni Album-Canzone
- Le funzionalità di ricerca avanzata e filtri non sono disponibili
- Non sono presenti funzionalità di autenticazione e autorizzazione