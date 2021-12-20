using System;

namespace Monopoly {
    static class Dice {
        private static Random generator = new Random();
        public static int Roll() => generator.Next(1, 7);
    }
}
