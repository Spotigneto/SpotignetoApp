namespace Backend.Models
{
    public class AsAlbumCanzoneModel
    {
        public long Id { get; set; }
        public string CanzoneId { get; set; } = string.Empty;
        public string AlbumId { get; set; } = string.Empty;
    }
}