namespace Backend.Models
{
    public class PlaylistModel
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public bool Privata { get; set; }
    }
}