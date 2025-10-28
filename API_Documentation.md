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
```json
{
  "nome": "Nome della canzone",
  "file": "percorso/del/file.mp3",
  "genereId": 1,
  "sottogenereId": 1,
  "durata": "03:45",
  "secondi": 225
}
```

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
- **Dati di Input**: ID della canzone nell'URL

### 1.4 Ottenere una Canzone per Nome
- **URL**: `GET /api/Canzone/canzone/by-name/{name}`
- **Funzione**: Recupera una canzone specifica tramite nome
- **Dati di Input**: Nome della canzone nell'URL

### 1.5 Aggiornare una Canzone
- **URL**: `PUT /api/Canzone/canzone/{id}`
- **Funzione**: Aggiorna i dati di una canzone esistente
- **Dati di Input**: ID nell'URL + dati JSON come per la creazione

### 1.6 Eliminare una Canzone
- **URL**: `DELETE /api/Canzone/canzone/{id}`
- **Funzione**: Elimina una canzone dal database
- **Dati di Input**: ID della canzone nell'URL

---

## 2. API ALBUM

### 2.1 Creare un Album
- **URL**: `POST /api/Records/album`
- **Funzione**: Crea un nuovo album nel database
- **Dati di Input**:
```json
{
  "nome": "Nome dell'album",
  "pubblica": true,
  "releaseDate": "2024-01-15"
}
```

### 2.2 Ottenere Tutti gli Album
- **URL**: `GET /api/Records/albums`
- **Funzione**: Recupera l'elenco di tutti gli album
- **Dati di Input**: Nessuno

### 2.3 Ottenere un Album per ID
- **URL**: `GET /api/Records/album/{id}`
- **Funzione**: Recupera un album specifico tramite ID
- **Dati di Input**: ID dell'album nell'URL

### 2.4 Aggiornare un Album
- **URL**: `PUT /api/Records/album/{id}`
- **Funzione**: Aggiorna i dati di un album esistente
- **Dati di Input**: ID nell'URL + dati JSON come per la creazione

### 2.5 Eliminare un Album
- **URL**: `DELETE /api/Records/album/{id}`
- **Funzione**: Elimina un album dal database
- **Dati di Input**: ID dell'album nell'URL

---

## 3. API PLAYLIST

### 3.1 Creare una Playlist
- **URL**: `POST /api/Records/playlist`
- **Funzione**: Crea una nuova playlist nel database
- **Dati di Input**:
```json
{
  "nome": "Nome della playlist",
  "privata": false
}
```

### 3.2 Ottenere Tutte le Playlist
- **URL**: `GET /api/Records/playlists`
- **Funzione**: Recupera l'elenco di tutte le playlist
- **Dati di Input**: Nessuno

### 3.3 Ottenere una Playlist per ID
- **URL**: `GET /api/Records/playlist/{id}`
- **Funzione**: Recupera una playlist specifica tramite ID
- **Dati di Input**: ID della playlist nell'URL

### 3.4 Aggiornare una Playlist
- **URL**: `PUT /api/Records/playlist/{id}`
- **Funzione**: Aggiorna i dati di una playlist esistente
- **Dati di Input**: ID nell'URL + dati JSON come per la creazione

### 3.5 Eliminare una Playlist
- **URL**: `DELETE /api/Records/playlist/{id}`
- **Funzione**: Elimina una playlist dal database
- **Dati di Input**: ID della playlist nell'URL

---

## 4. API RELAZIONI CANZONE-PLAYLIST

### 4.1 Aggiungere Canzone a Playlist
- **URL**: `POST /api/AsCanzonePlaylist`
- **Funzione**: Aggiunge una canzone a una playlist
- **Dati di Input**:
```json
{
  "acPlaylistId": 1,
  "acCanzoneId": 5
}
```

### 4.2 Ottenere Tutte le Relazioni
- **URL**: `GET /api/AsCanzonePlaylist`
- **Funzione**: Recupera tutte le relazioni canzone-playlist
- **Dati di Input**: Nessuno

### 4.3 Ottenere Relazione per ID
- **URL**: `GET /api/AsCanzonePlaylist/{id}`
- **Funzione**: Recupera una relazione specifica tramite ID
- **Dati di Input**: ID della relazione nell'URL

### 4.4 Rimuovere Canzone da Playlist
- **URL**: `DELETE /api/AsCanzonePlaylist/{id}`
- **Funzione**: Rimuove una canzone da una playlist
- **Dati di Input**: ID della relazione nell'URL

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