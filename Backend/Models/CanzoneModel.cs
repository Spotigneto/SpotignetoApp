namespace Backend.Models
{
    public class CanzoneModel
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;
        public long GenereId { get; set; }
        public long SottogenereId { get; set; }
        public string Durata { get; set; } = string.Empty;
        public int Secondi { get; set; }
    }
}