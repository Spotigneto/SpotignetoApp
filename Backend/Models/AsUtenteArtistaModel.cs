namespace Backend.Models
{
    public class AsUtenteArtistaModel
    {
        public long Id { get; set; }
        public string UtenteId { get; set; } = string.Empty;
        public string ArtistaId { get; set; } = string.Empty;
    }
}