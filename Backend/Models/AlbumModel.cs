namespace Backend.Models
{
    public class AlbumModel
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Pubblica { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}