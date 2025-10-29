namespace Backend.Models
{
    public class AsCanzonePlaylistModel
    {
        public long AcPlaylistId { get; set; }
        public long AcCanzoneId { get; set; }
        public long OldCanzoneId { get; set; }
        public long NewCanzoneId { get; set; }
        public int? TrackOrder { get; set; }
    }
}