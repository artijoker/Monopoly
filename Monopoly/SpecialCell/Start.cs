using System;

namespace Monopoly {
    class Start : SpecialCell {
        private int Salary { get; }
        public Start() : base("Старт", ConsoleColor.Green) {
            Salary = 200;
        }

        public override void Action(Player player) {
            player.Money += Salary;
            Console.WriteLine($"Вы попали на клетку {Name}");
            Console.WriteLine($"Вы получаете зарплату {Salary}$ за пройденный круг") ;
            Console.WriteLine();
        }
    }
}
