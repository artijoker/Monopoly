using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly {
    interface  ICell {
        string Name { get; }
        ConsoleColor Color { get; }
        void Action(Player player);
        int Print(int cursorLeft);
    }
}
