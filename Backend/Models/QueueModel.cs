namespace Backend.Models
{
    public class QueueModel
    {
        public string Id { get; set; } = string.Empty;
        public List<QueueItemModel> Items { get; set; } = new List<QueueItemModel>();
        public int CurrentIndex { get; set; } = 0;
        public PlaybackMode Mode { get; set; } = PlaybackMode.Sequential;
        public bool IsShuffled { get; set; } = false;
        public List<int> ShuffleOrder { get; set; } = new List<int>();
    }

    public class QueueItemModel
    {
        public string CanzoneId { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;
        public string Durata { get; set; } = string.Empty;
        public int Secondi { get; set; }
        public int OriginalIndex { get; set; }
    }

    public enum PlaybackMode
    {
        Sequential = 0,     // Riproduzione in ordine
        Loop = 1,          // Loop della coda/playlist
        LoopSingle = 2,    // Loop del singolo brano
        Random = 3         // Riproduzione casuale
    }

    public class PlayerStateModel
    {
        public bool IsPlaying { get; set; } = false;
        public bool IsPaused { get; set; } = false;
        public string? CurrentCanzoneId { get; set; }
        public int CurrentPosition { get; set; } = 0; // Posizione in secondi
        public float Volume { get; set; } = 1.0f;
        public PlaybackMode Mode { get; set; } = PlaybackMode.Sequential;
        public QueueModel Queue { get; set; } = new QueueModel();
    }
}