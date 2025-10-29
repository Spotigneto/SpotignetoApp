namespace Backend.Models
{
    public class AsCanzonePlaylistModel
    {
        public string AcPlaylistId { get; set; } = string.Empty;
        public string AcCanzoneId { get; set; } = string.Empty;
        public string OldCanzoneId { get; set; } = string.Empty;
        public string NewCanzoneId { get; set; } = string.Empty;
        public int? TrackOrder { get; set; }
    }
}