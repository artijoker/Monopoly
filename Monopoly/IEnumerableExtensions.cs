using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Monopoly {
    static class IEnumerableExtensions {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action) {
            foreach (var item in collection)
                action(item);
        }
    }
}

