using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly {
    static class IListExtensions {
        private static Random generator = new Random();

        public static void Shuffle<T>(this IList<T> collection) {
            for (int i = collection.Count - 1; i > 0; i--) {
                int j = generator.Next(i + 1);
               
                T temp = collection[j];
                collection[j] = collection[i];
                collection[i] = temp;
            }
        }
    }
}
