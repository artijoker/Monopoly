using System;

namespace Monopoly {
    class Prison : SpecialCell {
        private int ExitPrice { get; }
        public Prison() : base("Тюрьма", ConsoleColor.White) {
            ExitPrice = 350;
        }

        public override void Action(Player player) {
            if (player.IsPrisoner()) {
                Console.WriteLine("Вы находитесь в тюрьме");
                Console.WriteLine($"Чтобы выйти отсидете срок или заплатите {ExitPrice}$");
                SelectActions(player);
                return;
            }
            player.Prisoner = 3;
            Console.WriteLine($"Вы попали на клетку {Name}");
            Console.WriteLine("Вы не можете бросать кубики следующие 3 хода");
            Console.WriteLine();
        }

        private void SelectActions(Player player) {
            SelectActionsHelp();
            string cmd;
            while (true) {
                Console.Write(">");
                cmd = Console.ReadLine().ToLower();
                if (cmd == "сидеть") {
                    if (player.Prisoner == 1)
                        Console.WriteLine($"Осталось отсидеть {player.Prisoner} ход");
                    else
                        Console.WriteLine($"Осталось отсидеть {player.Prisoner} хода");
                    Console.WriteLine();
                    return;
                }
                else if (cmd == "заплатить") {
                    if (player.Money > ExitPrice) {
                        Console.WriteLine($"Вас выпустили досрочно");
                        Console.WriteLine($"С вашего счета списано {ExitPrice}$");
                        Console.WriteLine();
                        player.Prisoner = 0;
                        player.Money -= ExitPrice;
                        return;
                    }
                    else 
                        Console.WriteLine("У вас на счету недостаточно денег чтобы заплатить");
                }
                else 
                    Console.WriteLine($"Ошибка! Команда \"{cmd}\" не найдена");
            }
        }
        private void SelectActionsHelp() {
            Console.WriteLine("сидеть - сидеть срок");
            Console.WriteLine($"заплатить - заплатить {ExitPrice}$ за досрочное освобождение");
        }
    }
}
