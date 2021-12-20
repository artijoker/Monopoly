using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly {
    class Imprison : SpecialCell {
        public int NewPosition { get; }
        public Imprison() : base("Марш в тюрьму", ConsoleColor.White) {
            NewPosition = 7;
        }

        public override void Action(Player player) {
            Console.WriteLine($"Вы попали на клетку {Name}");
            Console.WriteLine($"Вы отпраляетесь в тюрьму");
            Console.WriteLine();
            player.Position = NewPosition;
            Game.Field[player.Position].Action(player);
        }
    }
}
