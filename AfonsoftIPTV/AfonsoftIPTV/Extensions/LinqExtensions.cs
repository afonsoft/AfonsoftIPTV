using System;
using System.Collections.Generic;
using System.Linq;


namespace AfonsoftIPTV.Extensions
{
    public static class LinqExtensions
    {
        public static T FirstOrDefaultFromMany<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector,
            Predicate<T> condition)
        {
            if (source == null || !source.Any())
                return default(T);

            var attempt = source.FirstOrDefault(t => condition(t));
            if (!Equals(attempt, default(T)))
                return attempt;

            return source.SelectMany(childrenSelector).FirstOrDefaultFromMany(childrenSelector, condition);
        }
    }
}
