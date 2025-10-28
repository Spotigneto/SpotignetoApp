# Documentazione API - Applicazione Musicale

## Indice
1. [Panoramica](#panoramica)
2. [Schema Database](#schema-database)
3. [API Endpoints](#api-endpoints)
   - [Utenti](#utenti)
   - [Canzoni](#canzoni)
   - [Playlist](#playlist)
   - [Artisti](#artisti)
   - [Album](#album)
   - [Generi](#generi)
   - [Ricerca](#ricerca)

---

## Panoramica

Questa documentazione descrive tutti gli endpoint API REST per l'applicazione musicale, basati sullo schema database fornito dal team backend.

**Base URL**: `https://api.musicapp.com/v1`

**Formato Risposte**: JSON

**Autenticazione**: Bearer Token (JWT) dove richiesto

---

## Schema Database

Lo schema database include le seguenti tabelle principali:

### Entità Principali
- **Utente** (`ut_id`, `ut_nome`)
- **Artista** (`ar_id`, `ar_nome`)
- **Playlist** (`pl_id`, `pl_nome`, `pl_privata`)
- **Album** (`al_id`, `al_nome`, `al_pubblica`, `al_release_date`)
- **Canzone** (`ca_id`, `ca_nome`, `ca_file`, `ca_genere_fk`, `ca_sottogenere_fk`, `ca_durata`, `ca_secondi`)
- **Genere_tp** (`gtp_id`, `gtp_genere`)
- **Sottogenere_tp** (`stp_id`, `stp_sottogenere`)

### Tabelle Associative
- **as_utente_playlist** - Relazione Utente ↔ Playlist
- **as_canzone_playlist** - Relazione Canzone ↔ Playlist
- **as_artista_album** - Relazione Artista ↔ Album
- **as_artista_canzone** - Relazione Artista ↔ Canzone
- **as_album_canzone** - Relazione Album ↔ Canzone

---

## API Endpoints

---

## Utenti

### `GET /api/utenti`
**Descrizione**: Ottiene la lista di tutti gli utenti

**Response** (200):
```json
{
  "success": true,
  "data": [
    {
      "ut_id": 1,
      "ut_nome": "Mario Rossi"
    },
    {
      "ut_id": 2,
      "ut_nome": "Laura Bianchi"
    }
  ]
}
```

---

### `GET /api/utenti/:id`
**Descrizione**: Ottiene i dettagli di un utente specifico

**Parametri URL**:
- `id` - ID utente (BIGINT)

**Response** (200):
```json
{
  "success": true,
  "data": {
    "ut_id": 1,
    "ut_nome": "Mario Rossi",
    "playlists": [
      {
        "pl_id": 5,
        "pl_nome": "Gaming",
        "pl_privata": false
      }
    ]
  }
}
```

**Errori**:
- `404` - Utente non trovato

---

### `POST /api/utenti`
**Descrizione**: Crea un nuovo utente

**Request Body**:
```json
{
  "ut_nome": "Giovanni Verdi"
}
```

**Response** (201):
```json
{
  "success": true,
  "data": {
    "ut_id": 3,
    "ut_nome": "Giovanni Verdi"
  }
}
```

**Errori**:
- `400` - Nome utente obbligatorio

---

### `PUT /api/utenti/:id`
**Descrizione**: Aggiorna i dati di un utente

**Parametri URL**:
- `id` - ID utente

**Request Body**:
```json
{
  "ut_nome": "Mario Rossi Aggiornato"
}
```

**Response** (200):
```json
{
  "success": true,
  "data": {
    "ut_id": 1,
    "ut_nome": "Mario Rossi Aggiornato"
  }
}
```

---

### `DELETE /api/utenti/:id`
**Descrizione**: Elimina un utente

**Parametri URL**:
- `id` - ID utente

**Response** (200):
```json
{
  "success": true,
  "message": "Utente eliminato con successo"
}
```

---

## Canzoni

### `GET /api/canzoni`
**Descrizione**: Ottiene la lista di tutte le canzoni con filtri opzionali

**Query Parameters**:
- `genere` - Filtra per genere (ID genere)
- `sottogenere` - Filtra per sottogenere (ID sottogenere)
- `artista` - Filtra per artista (ID artista)
- `album` - Filtra per album (ID album)
- `search` - Ricerca per nome canzone
- `page` - Numero pagina (default: 1)
- `limit` - Elementi per pagina (default: 20, max: 100)

**Response** (200):
```json
{
  "success": true,
  "data": {
    "canzoni": [
      {
        "ca_id": 1,
        "ca_nome": "Say My Name",
        "ca_file": "https://cdn.musicapp.com/songs/say-my-name.mp3",
        "ca_durata": "3:59",
        "ca_secondi": 239,
        "genere": {
          "gtp_id": 1,
          "gtp_genere": "Electronic"
        },
        "sottogenere": {
          "stp_id": 5,
          "stp_sottogenere": "Chillwave"
        },
        "artisti": [
          {
            "ar_id": 10,
            "ar_nome": "ODESZA"
          },
          {
            "ar_id": 25,
            "ar_nome": "Zyra"
          }
        ],
        "album": {
          "al_id": 3,
          "al_nome": "In Return",
          "al_release_date": "2014-09-08"
        }
      }
    ],
    "pagination": {
      "page": 1,
      "limit": 20,
      "total": 150,
      "totalPages": 8
    }
  }
}
```

---

### `GET /api/canzoni/:id`
**Descrizione**: Ottiene i dettagli completi di una canzone

**Parametri URL**:
- `id` - ID canzone (BIGINT)

**Response** (200):
```json
{
  "success": true,
  "data": {
    "ca_id": 1,
    "ca_nome": "Say My Name",
    "ca_file": "https://cdn.musicapp.com/songs/say-my-name.mp3",
    "ca_durata": "3:59",
    "ca_secondi": 239,
    "genere": {
      "gtp_id": 1,
      "gtp_genere": "Electronic"
    },
    "sottogenere": {
      "stp_id": 5,
      "stp_sottogenere": "Chillwave"
    },
    "artisti": [
      {
        "ar_id": 10,
        "ar_nome": "ODESZA"
      },
      {
        "ar_id": 25,
        "ar_nome": "Zyra"
      }
    ],
    "album": {
      "al_id": 3,
      "al_nome": "In Return",
      "al_pubblica": true,
      "al_release_date": "2014-09-08"
    }
  }
}
```

**Errori**:
- `404` - Canzone non trovata

---

### `POST /api/canzoni`
**Descrizione**: Crea una nuova canzone

**Request Body**:
```json
{
  "ca_nome": "Higher Ground",
  "ca_file": "https://cdn.musicapp.com/songs/higher-ground.mp3",
  "ca_genere_fk": 1,
  "ca_sottogenere_fk": 5,
  "ca_durata": "3:46",
  "ca_secondi": 226,
  "artisti_ids": [10],
  "album_id": 4
}
```

**Response** (201):
```json
{
  "success": true,
  "data": {
    "ca_id": 45,
    "ca_nome": "Higher Ground",
    "ca_file": "https://cdn.musicapp.com/songs/higher-ground.mp3",
    "ca_durata": "3:46",
    "ca_secondi": 226
  }
}
```

**Errori**:
- `400` - Dati mancanti o non validi
- `404` - Genere, sottogenere, artista o album non trovati

---

### `PUT /api/canzoni/:id`
**Descrizione**: Aggiorna i dati di una canzone

**Parametri URL**:
- `id` - ID canzone

**Request Body**:
```json
{
  "ca_nome": "Say My Name (Remix)",
  "ca_durata": "4:15",
  "ca_secondi": 255
}
```

**Response** (200):
```json
{
  "success": true,
  "data": {
    "ca_id": 1,
    "ca_nome": "Say My Name (Remix)",
    "ca_durata": "4:15",
    "ca_secondi": 255
  }
}
```

---

### `DELETE /api/canzoni/:id`
**Descrizione**: Elimina una canzone

**Parametri URL**:
- `id` - ID canzone

**Response** (200):
```json
{
  "success": true,
  "message": "Canzone eliminata con successo"
}
```

---

### `POST /api/canzoni/:id/artisti`
**Descrizione**: Associa un artista a una canzone

**Parametri URL**:
- `id` - ID canzone

**Request Body**:
```json
{
  "artista_id": 25
}
```

**Response** (200):
```json
{
  "success": true,
  "message": "Artista associato alla canzone"
}
```

**Errori**:
- `404` - Canzone o artista non trovati
- `409` - Artista già associato a questa canzone

---

### `DELETE /api/canzoni/:canzoneId/artisti/:artistaId`
**Descrizione**: Rimuove l'associazione artista-canzone

**Parametri URL**:
- `canzoneId` - ID canzone
- `artistaId` - ID artista

**Response** (200):
```json
{
  "success": true,
  "message": "Associazione rimossa"
}
```

---

## Playlist

### `GET /api/playlist`
**Descrizione**: Ottiene tutte le playlist (pubbliche o dell'utente)

**Query Parameters**:
- `utente_id` - Filtra per utente specifico
- `privata` - Filtra per privata (true/false)

**Response** (200):
```json
{
  "success": true,
  "data": [
    {
      "pl_id": 1,
      "pl_nome": "Gaming",
      "pl_privata": false,
      "proprietari": [
        {
          "ut_id": 1,
          "ut_nome": "Mario Rossi"
        }
      ],
      "totale_canzoni": 25,
      "durata_totale_secondi": 6420
    }
  ]
}
```

---

### `GET /api/playlist/:id`
**Descrizione**: Ottiene i dettagli completi di una playlist con tutte le canzoni

**Parametri URL**:
- `id` - ID playlist

**Response** (200):
```json
{
  "success": true,
  "data": {
    "pl_id": 1,
    "pl_nome": "Gaming",
    "pl_privata": false,
    "proprietari": [
      {
        "ut_id": 1,
        "ut_nome": "Mario Rossi"
      }
    ],
    "canzoni": [
      {
        "ca_id": 1,
        "ca_nome": "Say My Name",
        "ca_durata": "3:59",
        "ca_secondi": 239,
        "ca_file": "https://cdn.musicapp.com/songs/say-my-name.mp3",
        "artisti": [
          {
            "ar_id": 10,
            "ar_nome": "ODESZA"
          }
        ],
        "album": {
          "al_id": 3,
          "al_nome": "In Return"
        }
      }
    ],
    "totale_canzoni": 25,
    "durata_totale_secondi": 6420
  }
}
```

**Errori**:
- `404` - Playlist non trovata
- `403` - Playlist privata (se non sei proprietario)

---

### `POST /api/playlist`
**Descrizione**: Crea una nuova playlist

**Request Body**:
```json
{
  "pl_nome": "My Awesome Playlist",
  "pl_privata": true,
  "utente_id": 1
}
```

**Response** (201):
```json
{
  "success": true,
  "data": {
    "pl_id": 15,
    "pl_nome": "My Awesome Playlist",
    "pl_privata": true
  }
}
```

**Errori**:
- `400` - Nome playlist e utente_id obbligatori
- `404` - Utente non trovato

---

### `PUT /api/playlist/:id`
**Descrizione**: Aggiorna i dati di una playlist

**Parametri URL**:
- `id` - ID playlist

**Request Body**:
```json
{
  "pl_nome": "Gaming Collection",
  "pl_privata": false
}
```

**Response** (200):
```json
{
  "success": true,
  "data": {
    "pl_id": 1,
    "pl_nome": "Gaming Collection",
    "pl_privata": false
  }
}
```

---

### `DELETE /api/playlist/:id`
**Descrizione**: Elimina una playlist

**Parametri URL**:
- `id` - ID playlist

**Response** (200):
```json
{
  "success": true,
  "message": "Playlist eliminata con successo"
}
```

---

### `POST /api/playlist/:id/canzoni`
**Descrizione**: Aggiunge una o più canzoni a una playlist

**Parametri URL**:
- `id` - ID playlist

**Request Body**:
```json
{
  "canzoni_ids": [1, 5, 10]
}
```

**Response** (200):
```json
{
  "success": true,
  "message": "3 canzoni aggiunte alla playlist"
}
```

**Errori**:
- `404` - Playlist o canzoni non trovate
- `409` - Una o più canzoni già presenti nella playlist

---

### `DELETE /api/playlist/:playlistId/canzoni/:canzoneId`
**Descrizione**: Rimuove una canzone da una playlist

**Parametri URL**:
- `playlistId` - ID playlist
- `canzoneId` - ID canzone

**Response** (200):
```json
{
  "success": true,
  "message": "Canzone rimossa dalla playlist"
}
```

---

### `POST /api/playlist/:id/utenti`
**Descrizione**: Aggiunge un utente come proprietario/collaboratore della playlist

**Parametri URL**:
- `id` - ID playlist

**Request Body**:
```json
{
  "utente_id": 5
}
```

**Response** (200):
```json
{
  "success": true,
  "message": "Utente aggiunto alla playlist"
}
```

---

### `DELETE /api/playlist/:playlistId/utenti/:utenteId`
**Descrizione**: Rimuove un utente dalla playlist

**Parametri URL**:
- `playlistId` - ID playlist
- `utenteId` - ID utente

**Response** (200):
```json
{
  "success": true,
  "message": "Utente rimosso dalla playlist"
}
```

---

## Artisti

### `GET /api/artisti`
**Descrizione**: Ottiene la lista di tutti gli artisti

**Query Parameters**:
- `search` - Ricerca per nome artista
- `page` - Numero pagina (default: 1)
- `limit` - Elementi per pagina (default: 20)

**Response** (200):
```json
{
  "success": true,
  "data": {
    "artisti": [
      {
        "ar_id": 10,
        "ar_nome": "ODESZA",
        "totale_canzoni": 45,
        "totale_album": 5
      },
      {
        "ar_id": 15,
        "ar_nome": "ILLENIUM",
        "totale_canzoni": 38,
        "totale_album": 4
      }
    ],
    "pagination": {
      "page": 1,
      "limit": 20,
      "total": 250
    }
  }
}
```

---

### `GET /api/artisti/:id`
**Descrizione**: Ottiene i dettagli completi di un artista con canzoni e album

**Parametri URL**:
- `id` - ID artista

**Response** (200):
```json
{
  "success": true,
  "data": {
    "ar_id": 10,
    "ar_nome": "ODESZA",
    "canzoni": [
      {
        "ca_id": 1,
        "ca_nome": "Say My Name",
        "ca_durata": "3:59",
        "album": {
          "al_id": 3,
          "al_nome": "In Return"
        }
      }
    ],
    "album": [
      {
        "al_id": 3,
        "al_nome": "In Return",
        "al_release_date": "2014-09-08",
        "totale_canzoni": 12
      },
      {
        "al_id": 4,
        "al_nome": "A Moment Apart",
        "al_release_date": "2017-09-08",
        "totale_canzoni": 16
      }
    ],
    "totale_canzoni": 45,
    "totale_album": 5
  }
}
```

**Errori**:
- `404` - Artista non trovato

---

### `POST /api/artisti`
**Descrizione**: Crea un nuovo artista

**Request Body**:
```json
{
  "ar_nome": "Porter Robinson"
}
```

**Response** (201):
```json
{
  "success": true,
  "data": {
    "ar_id": 50,
    "ar_nome": "Porter Robinson"
  }
}
```

**Errori**:
- `400` - Nome artista obbligatorio
- `409` - Artista già esistente

---

### `PUT /api/artisti/:id`
**Descrizione**: Aggiorna i dati di un artista

**Parametri URL**:
- `id` - ID artista

**Request Body**:
```json
{
  "ar_nome": "ODESZA (Harrison Mills & Clayton Knight)"
}
```

**Response** (200):
```json
{
  "success": true,
  "data": {
    "ar_id": 10,
    "ar_nome": "ODESZA (Harrison Mills & Clayton Knight)"
  }
}
```

---

### `DELETE /api/artisti/:id`
**Descrizione**: Elimina un artista

**Parametri URL**:
- `id` - ID artista

**Response** (200):
```json
{
  "success": true,
  "message": "Artista eliminato con successo"
}
```

---

## Album

### `GET /api/album`
**Descrizione**: Ottiene la lista di tutti gli album

**Query Parameters**:
- `artista` - Filtra per artista (ID artista)
- `pubblica` - Filtra per pubblica (true/false)
- `search` - Ricerca per nome album
- `page` - Numero pagina (default: 1)
- `limit` - Elementi per pagina (default: 20)

**Response** (200):
```json
{
  "success": true,
  "data": {
    "album": [
      {
        "al_id": 3,
        "al_nome": "In Return",
        "al_pubblica": true,
        "al_release_date": "2014-09-08",
        "artisti": [
          {
            "ar_id": 10,
            "ar_nome": "ODESZA"
          }
        ],
        "totale_canzoni": 12
      }
    ],
    "pagination": {
      "page": 1,
      "limit": 20,
      "total": 180
    }
  }
}
```

---

### `GET /api/album/:id`
**Descrizione**: Ottiene i dettagli completi di un album con tutte le canzoni

**Parametri URL**:
- `id` - ID album

**Response** (200):
```json
{
  "success": true,
  "data": {
    "al_id": 3,
    "al_nome": "In Return",
    "al_pubblica": true,
    "al_release_date": "2014-09-08",
    "artisti": [
      {
        "ar_id": 10,
        "ar_nome": "ODESZA"
      }
    ],
    "canzoni": [
      {
        "ca_id": 1,
        "ca_nome": "Say My Name",
        "ca_durata": "3:59",
        "ca_secondi": 239,
        "ca_file": "https://cdn.musicapp.com/songs/say-my-name.mp3",
        "genere": {
          "gtp_id": 1,
          "gtp_genere": "Electronic"
        }
      }
    ],
    "totale_canzoni": 12,
    "durata_totale_secondi": 2880
  }
}
```

**Errori**:
- `404` - Album non trovato
- `403` - Album non pubblica

---

### `POST /api/album`
**Descrizione**: Crea un nuovo album

**Request Body**:
```json
{
  "al_nome": "A Moment Apart",
  "al_pubblica": true,
  "al_release_date": "2017-09-08",
  "artisti_ids": [10]
}
```

**Response** (201):
```json
{
  "success": true,
  "data": {
    "al_id": 25,
    "al_nome": "A Moment Apart",
    "al_pubblica": true,
    "al_release_date": "2017-09-08"
  }
}
```

**Errori**:
- `400` - Nome album obbligatorio
- `404` - Artista non trovato

---

### `PUT /api/album/:id`
**Descrizione**: Aggiorna i dati di un album

**Parametri URL**:
- `id` - ID album

**Request Body**:
```json
{
  "al_nome": "In Return (Deluxe Edition)",
  "al_release_date": "2015-03-15"
}
```

**Response** (200):
```json
{
  "success": true,
  "data": {
    "al_id": 3,
    "al_nome": "In Return (Deluxe Edition)",
    "al_release_date": "2015-03-15"
  }
}
```

---

### `DELETE /api/album/:id`
**Descrizione**: Elimina un album

**Parametri URL**:
- `id` - ID album

**Response** (200):
```json
{
  "success": true,
  "message": "Album eliminato con successo"
}
```

---

### `POST /api/album/:id/artisti`
**Descrizione**: Associa un artista a un album

**Parametri URL**:
- `id` - ID album

**Request Body**:
```json
{
  "artista_id": 15
}
```

**Response** (200):
```json
{
  "success": true,
  "message": "Artista associato all'album"
}
```

---

### `POST /api/album/:id/canzoni`
**Descrizione**: Associa una canzone a un album

**Parametri URL**:
- `id` - ID album

**Request Body**:
```json
{
  "canzone_id": 20
}
```

**Response** (200):
```json
{
  "success": true,
  "message": "Canzone associata all'album"
}
```

---

## Generi

### `GET /api/generi`
**Descrizione**: Ottiene la lista di tutti i generi musicali

**Response** (200):
```json
{
  "success": true,
  "data": [
    {
      "gtp_id": 1,
      "gtp_genere": "Electronic",
      "totale_canzoni": 450
    },
    {
      "gtp_id": 2,
      "gtp_genere": "Rock",
      "totale_canzoni": 320
    },
    {
      "gtp_id": 3,
      "gtp_genere": "Pop",
      "totale_canzoni": 580
    }
  ]
}
```

---

### `GET /api/generi/:id`
**Descrizione**: Ottiene i dettagli di un genere con i sottogeneri

**Parametri URL**:
- `id` - ID genere

**Response** (200):
```json
{
  "success": true,
  "data": {
    "gtp_id": 1,
    "gtp_genere": "Electronic",
    "sottogeneri": [
      {
        "stp_id": 5,
        "stp_sottogenere": "Chillwave",
        "totale_canzoni": 85
      },
      {
        "stp_id": 6,
        "stp_sottogenere": "Dubstep",
        "totale_canzoni": 120
      }
    ],
    "totale_canzoni": 450
  }
}
```

**Errori**:
- `404` - Genere non trovato

---

### `POST /api/generi`
**Descrizione**: Crea un nuovo genere

**Request Body**:
```json
{
  "gtp_genere": "Jazz"
}
```

**Response** (201):
```json
{
  "success": true,
  "data": {
    "gtp_id": 8,
    "gtp_genere": "Jazz"
  }
}
```

---

### `GET /api/sottogeneri`
**Descrizione**: Ottiene la lista di tutti i sottogeneri

**Query Parameters**:
- `genere` - Filtra per genere (ID genere)

**Response** (200):
```json
{
  "success": true,
  "data": [
    {
      "stp_id": 5,
      "stp_sottogenere": "Chillwave",
      "genere": {
        "gtp_id": 1,
        "gtp_genere": "Electronic"
      },
      "totale_canzoni": 85
    }
  ]
}
```

---

### `POST /api/sottogeneri`
**Descrizione**: Crea un nuovo sottogenere

**Request Body**:
```json
{
  "stp_sottogenere": "Future Bass"
}
```

**Response** (201):
```json
{
  "success": true,
  "data": {
    "stp_id": 15,
    "stp_sottogenere": "Future Bass"
  }
}
```

---

## Ricerca

### `GET /api/ricerca`
**Descrizione**: Ricerca globale per canzoni, artisti, album e playlist

**Query Parameters**:
- `q` - Query di ricerca (required, min 2 caratteri)
- `tipo` - Tipo risultato: `canzoni`, `artisti`, `album`, `playlist`, `all` (default: `all`)
- `limit` - Numero massimo risultati per tipo (default: 10, max: 50)

**Response** (200):
```json
{
  "success": true,
  "data": {
    "canzoni": [
      {
        "ca_id": 1,
        "ca_nome": "Say My Name",
        "ca_durata": "3:59",
        "artisti": [
          {
            "ar_id": 10,
            "ar_nome": "ODESZA"
          }
        ],
        "album": {
          "al_id": 3,
          "al_nome": "In Return"
        }
      }
    ],
    "artisti": [
      {
        "ar_id": 10,
        "ar_nome": "ODESZA",
        "totale_canzoni": 45
      }
    ],
    "album": [
      {
        "al_id": 3,
        "al_nome": "In Return",
        "artisti": [
          {
            "ar_id": 10,
            "ar_nome": "ODESZA"
          }
        ],
        "totale_canzoni": 12
      }
    ],
    "playlist": [
      {
        "pl_id": 5,
        "pl_nome": "This is ODESZA",
        "pl_privata": false,
        "totale_canzoni": 50
      }
    ]
  }
}
```

**Errori**:
- `400` - Query di ricerca obbligatoria (minimo 2 caratteri)

---

### `GET /api/ricerca/suggerimenti`
**Descrizione**: Ottiene suggerimenti di ricerca in tempo reale

**Query Parameters**:
- `q` - Query parziale (min 1 carattere)
- `limit` - Numero suggerimenti (default: 5, max: 10)

**Response** (200):
```json
{
  "success": true,
  "data": {
    "suggerimenti": [
      "ODESZA",
      "ODESZA - Say My Name",
      "ODESZA - A Moment Apart",
      "In Return - ODESZA"
    ]
  }
}
```

---

## Formati di Errore Standard

Tutti gli endpoint utilizzano un formato di errore consistente:

```json
{
  "success": false,
  "error": {
    "code": "RESOURCE_NOT_FOUND",
    "message": "La risorsa richiesta non è stata trovata",
    "details": {
      "resource": "Canzone",
      "id": 999
    }
  }
}
```

### Codici di Errore Comuni

- `400 Bad Request` - Richiesta malformata o parametri mancanti
- `401 Unauthorized` - Autenticazione richiesta
- `403 Forbidden` - Accesso negato (es. playlist privata)
- `404 Not Found` - Risorsa non trovata
- `409 Conflict` - Conflitto (es. risorsa già esistente)
- `422 Unprocessable Entity` - Validazione fallita
- `500 Internal Server Error` - Errore del server

---

## Note Implementative

1. **Paginazione**: Tutti gli endpoint che restituiscono liste supportano paginazione con parametri `page` e `limit`

2. **Ordinamento**: Gli endpoint lista supportano ordinamento con parametri `sortBy` e `order` (asc/desc)

3. **Relazioni Many-to-Many**: Le tabelle associative permettono relazioni multiple (es. un artista può avere più album, un album può avere più artisti)

4. **Privacy**: Le playlist private (`pl_privata = 1`) sono accessibili solo ai proprietari

5. **Soft Delete**: Considerare implementazione di soft delete invece di eliminazione fisica

6. **Caching**: Implementare caching per richieste frequenti (lista generi, artisti popolari, ecc.)

7. **Rate Limiting**: Implementare rate limiting per proteggere le API da abusi

---

## Esempi di Utilizzo

### Scenario 1: Ottenere tutte le canzoni di un artista

```http
GET /api/canzoni?artista=10
```

### Scenario 2: Creare una playlist e aggiungere canzoni

```http
POST /api/playlist
{
  "pl_nome": "My Gaming Playlist",
  "pl_privata": false,
  "utente_id": 1
}

POST /api/playlist/15/canzoni
{
  "canzoni_ids": [1, 5, 10, 15]
}
```

### Scenario 3: Cercare brani electronic

```http
GET /api/canzoni?genere=1&limit=50
```

### Scenario 4: Ricerca globale

```http
GET /api/ricerca?q=odesza&tipo=all
```

