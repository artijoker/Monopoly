using System;


namespace Monopoly {
    class FreeParking : SpecialCell {
        public FreeParking() : base("Бесплатная парковка", ConsoleColor.White) { }

        public override void Action(Player player) {
            Console.WriteLine($"Вы попали на клетку {Name}");
            Console.WriteLine();
        }


    }
}
