# MP3 File Handling API Documentation

## Overview
The backend now supports complete MP3 file management for the Canzone entity, including upload, streaming, and download functionality.

## API Endpoints

### 1. Upload MP3 File
**POST** `/api/Canzone/upload/{id}`

Uploads an MP3 file for a specific Canzone.

**Parameters:**
- `id` (long): The ID of the Canzone
- `file` (IFormFile): The MP3 file to upload

**Validation:**
- File must be MP3 format (audio/mpeg or .mp3 extension)
- Maximum file size: 50MB
- Canzone must exist in database

**Response:**
```json
{
  "message": "File uploaded successfully",
  "filePath": "/audio/songs/1_abc123.mp3",
  "fileName": "1_abc123.mp3"
}
```

### 2. Stream MP3 File
**GET** `/api/Canzone/stream/{id}`

Streams an MP3 file for audio playback with range support.

**Parameters:**
- `id` (long): The ID of the Canzone

**Features:**
- Supports HTTP range requests for efficient streaming
- Returns audio/mpeg content type
- Enables progressive download

### 3. Download MP3 File
**GET** `/api/Canzone/download/{id}`

Downloads the MP3 file with the song name as filename.

**Parameters:**
- `id` (long): The ID of the Canzone

**Response:**
- File download with filename: `{SongName}.mp3`

## File Storage Structure
```
Backend/
└── wwwroot/
    └── audio/
        └── songs/
            ├── 1_abc123.mp3
            ├── 2_def456.mp3
            └── ...
```

## File Naming Convention
Files are stored with the format: `{CanzoneId}_{UniqueGuid}.mp3`

## Security Features
- File type validation (only MP3 files allowed)
- File size limits (50MB maximum)
- Automatic cleanup of old files when uploading new ones
- Secure file path handling

## Usage Example

### Upload a file using curl:
```bash
curl -X POST "http://localhost:5232/api/Canzone/upload/1" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@song.mp3"
```

### Stream a file in HTML:
```html
<audio controls>
  <source src="http://localhost:5232/api/Canzone/stream/1" type="audio/mpeg">
</audio>
```

### Download a file:
```html
<a href="http://localhost:5232/api/Canzone/download/1" download>Download Song</a>
```

## Database Integration
The `CaFile` field in the `CanzoneEntity` stores the relative path to the MP3 file (e.g., `/audio/songs/1_abc123.mp3`).

## Error Handling
All endpoints include comprehensive error handling for:
- File not found scenarios
- Invalid file formats
- Missing Canzone records
- Server errors

## Dependencies
- **IFileService**: Custom service for file operations
- **Static Files Middleware**: Enabled for serving audio files
- **Entity Framework**: For database operations