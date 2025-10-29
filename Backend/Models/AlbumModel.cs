namespace Backend.Models
{
    public class AlbumModel
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public bool Pubblica { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}