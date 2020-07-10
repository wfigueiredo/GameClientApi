using System;
using System.Collections.Generic;
using System.Linq;

namespace GameClientApi.Infrastructure.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => !enumerable?.Any() ?? true;
    }
}
