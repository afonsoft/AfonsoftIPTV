using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Afonsoft.m3u.Extensions;

namespace Afonsoft.m3u
{
    public class M3UFile : ICollection<M3UEntry>
    {
        private readonly List<M3UEntry> _entries = new List<M3UEntry>();

        public M3UEntry this[int index] => _entries[index];

        public M3UFile(string fileName, bool resolveRelativePaths = false)
        {
            Load(fileName, resolveRelativePaths);
        }

        public void Load(string fileName, bool resolveRelativePaths = false)
        {
            _entries.Clear();

            if(string.IsNullOrEmpty(fileName))
                throw new M3UException("fime is missing.");

            using (var reader = new StreamReader(fileName))
            {
                var workingUri = new Uri(Path.GetDirectoryName(fileName) ?? throw new M3UException("fime is missing."));

                string line;
                var lineCount = 0;

                M3UEntry entry = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (lineCount == 0 && line != "#EXTM3U")
                        throw new M3UException("M3U header is missing.");

                    if (line.StartsWith("#EXTINF:"))
                    {
                        if (entry != null)
                            throw new M3UException("Unexpected entry detected.");

                        //Remove "#EXTINF:"
                        var split = line.Substring(8, line.Length - 8).Split(new[] { ',' }, 2);

                        if (split.Length != 2)
                            throw new M3UException("Invalid track information.");

                        var newLine = split[0];
                        var time = split[0].Substring(0, split[0].IndexOf(' '));

                        if (!int.TryParse(time, out int seconds))
                            throw new M3UException("Invalid track duration.");

                        var title = split[1];
                        var logo = GetValue(newLine, "tvg-logo");
                        var group = GetValue(newLine, "group-title");
                        var channelId = GetValue(newLine, "channel-id");
                        var epgId = GetValue(newLine, "epg-id");
                        var name = GetValue(newLine, "tvg-name");

                        var duration = TimeSpan.FromSeconds(seconds);
                        int.TryParse(channelId, out int idChannel);
                        int.TryParse(epgId, out int idEPG);

                        entry = new M3UEntry(duration, title, logo, group, name, idChannel, idEPG, null);
                    }

                    else if (entry != null && !line.StartsWith("#")) //ignore comments
                    {
                        if (!Uri.TryCreate(line, UriKind.RelativeOrAbsolute, out var path))
                            throw new M3UException("Invalid entry path.");

                        if (path.IsFile && resolveRelativePaths)
                            path = path.MakeAbsoluteUri(workingUri);

                        entry.Path = path;

                        _entries.Add(entry);

                        entry = null;
                    }

                    lineCount++;
                }
            }
        }

        private string GetValue(string line, string value, string comma = "\"")
        {
            if (line.IndexOf(value, StringComparison.Ordinal) != -1)
            {
                string stringAfterChar =line.Substring(line.IndexOf(value, StringComparison.Ordinal) + value.Length + 1).Split(' ')[0];
                int firstStringPosition = stringAfterChar.IndexOf(comma, StringComparison.Ordinal) + 1;
                int LastStringPosition = stringAfterChar.LastIndexOf(comma, StringComparison.Ordinal) - 1;
                return stringAfterChar.Substring(firstStringPosition, LastStringPosition);
            }

            return "";
        }

        public void Save(string fileName, bool useAbsolutePaths = false, bool useLocalFilePath = true)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new M3UException("file is missing.");

            var workingUri = new Uri(Path.GetDirectoryName(fileName) ?? throw new M3UException("fime is missing."));

            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine("#EXTM3U");

                foreach (var entry in this)
                {
                    writer.WriteLine("#EXTINF:{0},{1}", entry.Duration.TotalSeconds, entry.Title);

                    if (entry.Path.IsFile && useLocalFilePath)
                        writer.WriteLine(entry.Path.LocalPath);
                    else if (!entry.Path.IsAbsoluteUri && useAbsolutePaths)
                        writer.WriteLine(entry.Path.MakeAbsoluteUri(workingUri));
                    else
                        writer.WriteLine(entry.Path);
                }
            }
        }

        #region Implementation of IEnumerable

        public IEnumerator<M3UEntry> GetEnumerator()
        {
            return ((IEnumerable<M3UEntry>)_entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<M3UEntry>

        public void Add(M3UEntry item)
        {
            _entries.Add(item);
        }

        public void Clear()
        {
            _entries.Clear();
        }

        public bool Contains(M3UEntry item)
        {
            return _entries.Contains(item);
        }

        public void CopyTo(M3UEntry[] array, int arrayIndex)
        {
            _entries.CopyTo(array, arrayIndex);
        }

        public bool Remove(M3UEntry item)
        {
            return _entries.Remove(item);
        }

        public int Count => _entries.Count;

        public bool IsReadOnly => false;

        #endregion

        public M3UEntry Find(Predicate<M3UEntry> match)
        {
            return _entries.Find(match);
        }

        public List<M3UEntry> FindAll(Predicate<M3UEntry> match)
        {
            return _entries.FindAll(match);
        }
    }
}
