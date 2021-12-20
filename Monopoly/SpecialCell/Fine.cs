using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly {
    class Fine : SpecialCell {
        private int AmountFine  { get; }
        public Fine() : base("Штраф", ConsoleColor.White) {
            AmountFine = 80;
        }

        public override void Action(Player player) {
            Console.WriteLine($"Вы попали на клетку {Name}");
            Console.WriteLine($"Вы должны заплатить штраф в размере {AmountFine}$");
            player.Money -= AmountFine;
            Console.WriteLine($"С вашего счета было списано {AmountFine}$");
            Console.WriteLine();
        }

    }
}
