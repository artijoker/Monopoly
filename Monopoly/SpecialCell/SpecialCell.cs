using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly {
    abstract class SpecialCell : ICell {
        public  SpecialCell(string name, ConsoleColor color) {
            Name = name;
            Color = color;
        }

        public string Name { get; }

        public ConsoleColor Color { get; }

        public abstract void Action(Player player);


        public int Print(int cursorLeft) {
            string[] strings = Name.Split(' ');
            int max = strings.Max(@string => @string.Length);
            Console.BackgroundColor = Color;
            Console.WriteLine(new string(' ', max));
            Console.BackgroundColor = ConsoleColor.Black;
            int count = 0;
            foreach (var @string in strings) {
                Console.CursorLeft = cursorLeft;
                Console.WriteLine(@string);
                ++count;
            }
            cursorLeft += max + 1;
            Console.CursorTop -= (count + 1);
            return cursorLeft;
        }
    }
}
