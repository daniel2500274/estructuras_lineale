using System;

namespace Playlist_circular_con_prev_next
{
    internal class Song
    {
        public Guid Id { get; }
        public string Title { get; }
        public string Artist { get; }
        public int DurationInSeconds { get; }

        public Song(string title, string artist, int durationInSeconds)
        {
            Id = Guid.NewGuid();
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Artist = artist ?? throw new ArgumentNullException(nameof(artist));
            DurationInSeconds = durationInSeconds;
        }
    }
}
