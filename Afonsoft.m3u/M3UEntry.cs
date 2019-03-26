using System;

namespace Afonsoft.m3u
{
    public class M3UEntry
    {
        public M3UEntry(TimeSpan duration, string title, string logo, string group, string name, int channelId, int epgId, Uri path)
        {
            Duration = duration;
            Title = title;
            Path = path;
            Logo = logo;
            Group = group;
            Name = name;
            ChannelId = channelId;
            EpgId = epgId;
        }

        public TimeSpan Duration { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }
        public Uri Path { get; set; }
        public string Logo { get; set; }
        public int ChannelId { get; set; }
        public int EpgId { get; set; }
        public string Name { get; set; }
    }
}
