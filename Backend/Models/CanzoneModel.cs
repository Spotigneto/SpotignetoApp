namespace Backend.Models
{
    public class CanzoneModel
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? File { get; set; }
        public string Genere { get; set; } = string.Empty;
        public string Sottogenere { get; set; } = string.Empty;
        public string Durata { get; set; } = string.Empty;
        public int Secondi { get; set; }
    }
}