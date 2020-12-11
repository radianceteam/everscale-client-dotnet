using System.Collections.Generic;
using System.Linq;

namespace TonSdk.Tests
{
    internal static class ListExtensions
    {
        public static T RemoveFirst<T>(this List<T> list)
        {
            if (!list.Any())
            {
                return default;
            }
            var e = list[0];
            list.RemoveAt(0);
            return e;
        }
    }
}
