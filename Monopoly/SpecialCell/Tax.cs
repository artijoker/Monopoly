using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly {
    class Tax : SpecialCell {
        private int Percent { get; set; }
        public Tax() : base("Налог", ConsoleColor.White) {
            Percent = 10;
        }

        public override void Action(Player player) {
            Console.WriteLine($"Вы попали на клетку {Name}");
            Console.WriteLine($"Вы должны заплатить налог в размере {Percent}% от общей суммы всех денег на вашем счету");
            int tax = (player.Money / 100) * Percent;
            Console.WriteLine($"Налог состаляет {tax}$");
            player.Money -= tax;
            Console.WriteLine($"С вашего счета было списано {tax}$");
            Console.WriteLine();
            Percent++;
        }
    }
}
