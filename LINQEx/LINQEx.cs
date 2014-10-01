using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQExtensions
{
    public static class LINQEx
    {
        public static T MinElement<T, S>(this IEnumerable<T> source, Func<T, S> predicate) where S : IComparable
        {
            T result = source.First();
            S min = predicate(result);

            foreach (T element in source.Skip(1))
            {
                var value = predicate(element);
                if (value.CompareTo(min) < 0)
                {
                    result = element;
                    min = value;
                }
            }

            return result;
        }

        public static T MaxElement<T, S>(this IEnumerable<T> source, Func<T, S> predicate) where S : IComparable
        {
            T result = source.First();
            S max = predicate(result);

            foreach (T element in source.Skip(1))
            {
                var value = predicate(element);
                if (value.CompareTo(max) > 0)
                {
                    result = element;
                    max = value;
                }
            }

            return result;
        }

        public static void CheckDescending<T, S>(this IEnumerable<T> source, Func<T, S> predicate) where S : IComparable
        {
            S latestValue = predicate(source.First());

            foreach (T element in source.Skip(1))
            {
                var value = predicate(element);
                if (value.CompareTo(latestValue) > 0)
                    throw new Exception(value.ToString() + " is larger than " + latestValue.ToString());
                latestValue = value;
            }
        }
    }
}
