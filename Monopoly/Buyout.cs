using System;

namespace Monopoly {
    class Buyout {
        public static void BuyoutMortgagedProperty(Player player) {
            Console.Clear();
            string cmd;
            if (!player.IsHaveProperty()) {
                Console.WriteLine("У вас нет недвижимости");
                Console.WriteLine();
                return;
            }
            if (!player.IsMortgagedProperty()) {
                Console.WriteLine("У вас нет заложенной недвижимости");
                Console.WriteLine();
                return;
            }
            Console.Write("Выкупить имущество (да/нет)\n>");
            while (true) {
                cmd = Console.ReadLine().ToLower();
                if (cmd == "да") {
                    Property property = SelectionMortgagedProperty(player);
                    while (true) {
                        Console.Write($"Вы хотите выкупить {property.Name} (да/нет)\n>");
                        cmd = Console.ReadLine().ToLower();
                        if (cmd == "да") {
                            if (player.Money < property.Buyout) {
                                Console.WriteLine("У вас недостаточно средств чтобы выкупить эту недвижимость");
                                return;
                            }
                            property.IsPledged = false;
                            player.Money -= property.Buyout;
                            Console.WriteLine($"Вы погасили догл перед банком в размере {property.Buyout}$");
                            Console.WriteLine($"{property.Name} вновь является вашей собственностью и теперь приносит прыбыль");
                            Console.WriteLine();
                            return;
                        }
                        else if (cmd == "нет")
                            return;
                        else
                            Console.WriteLine($"Ошибка! Команда \"{cmd}\" не найдена");
                    }
                }
                else if (cmd == "нет")
                    return;
                else
                    Console.WriteLine($"Ошибка! Команда \"{cmd}\" не найдена");
            }
        }

        static private Property SelectionMortgagedProperty(Player player) {
            while (true) {
                Console.WriteLine("Список имущества которое находится в залоге у банка");
                player.PrintMortgagedProperty();
                Console.Write("Введите название недвижимости\n>");
                string name = Console.ReadLine();
                if (player.GetProperty(name) is Property property) 
                    return property;
                else 
                    Console.WriteLine($"Ошибка! Недвижимости \"{name}\" не найдена");
            }
        }
    }
}
