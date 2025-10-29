namespace Backend.Models
{
    public class PlaylistModel
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Privata { get; set; }
    }
}