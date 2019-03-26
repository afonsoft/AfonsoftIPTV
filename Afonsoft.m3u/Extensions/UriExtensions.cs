using System;

namespace Afonsoft.m3u.Extensions
{
    internal static class UriExtensions
    {
        public static Uri MakeAbsoluteUri(this Uri u, Uri uri)
        {
            return new Uri(u, uri);
        }
    }
}
