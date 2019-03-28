using System;
using System.Collections.Generic;

namespace Afonsoft.m3u
{
    public class M3UEntry : IComparer<M3UEntry>, IComparable<M3UEntry>
    {
        public M3UEntry(TimeSpan duration, string title, string logo, string group, string name, string channelId,
            string epgId, Uri path)
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
        public string ChannelId { get; set; }
        public string EpgId { get; set; }
        public string Name { get; set; }


        public int Compare(M3UEntry x, M3UEntry y)
        {
            if (x != null && y != null)
            {
                int r = string.CompareOrdinal(x.Group, y.Group);
                if (r == 0)
                    r = string.CompareOrdinal(x.Title, y.Title);
                if (r == 0)
                    r = string.CompareOrdinal(x.Name, y.Name);
                return r;
            }

            return 0;
        }

        public int CompareTo(M3UEntry y)
        {
            return y != null ? Compare(this, y) : 0;
        }
    }
}
