using System;
using System.Collections.Generic;

namespace TestCraftParserLib
{
    public static class Extensions
    {
        public static IEnumerable<Derivative<int>> Derivatives(this IEnumerable<int> source)
        {
            return GetDerivatives(source, (next, last) => next - last);
        }

        private static IEnumerable<Derivative<T>> GetDerivatives<T>(IEnumerable<T> source, Func<T, T, T> getDelta)
        {
            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    yield break;
                }

                var last = e.Current;

                while (e.MoveNext())
                {
                    var next = e.Current;
                    var delta = getDelta(next, last);

                    yield return new Derivative<T>(last, delta);

                    last = next;
                }
            }
        }

        // selects items from enumerable until item matches predicate
        public static IEnumerable<string> Until(this IEnumerable<string> source, Predicate<string> until)
        {
            foreach (var str in source)
            {
                if (until(str))
                {
                    yield break;
                }
                yield return str;
            }
        }
    }
}