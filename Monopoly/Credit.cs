using System;

namespace Monopoly {
    class Credit {
        public static void CreditSecuredProperty(Player player) {
            Console.Clear();
            string cmd;
            if (!player.IsHaveProperty()) {
                Console.WriteLine("У вас нет недвижимости");
                Console.WriteLine();
                return;
            }
            if(!player.IsPledgeProperty()) {
                Console.WriteLine("У вас нет недвижимости под залог");
                Console.WriteLine();
                return;
            }

            Console.Write("Взять кредит (да/нет)\n>");
            while (true) {
                cmd = Console.ReadLine().ToLower();
                if (cmd == "да") {
                    Property property = SelectionNotMortgagedProperty(player);
                    while (true) {
                        Console.Write($"Вы хотите заложить {property.Name} (да/нет)\n>");
                        cmd = Console.ReadLine().ToLower();
                        if (cmd == "да") {
                            property.IsPledged = true;
                            player.Money += property.Pledge;
                            Console.WriteLine($"Вам выдали кредин в размере {property.Pledge}$");
                            Console.WriteLine($"{property.Name} теперь находится в залоге у банка и больше не приносит прибыль");
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

        static private Property SelectionNotMortgagedProperty(Player player) {
            while (true) {
                Console.WriteLine("Список имущества под залог которого можно взять кредит");
                player.PrintPropertyThatCanBePledged();
                Console.Write("Введите название недвижимости\n>");
                string name = Console.ReadLine();
                if (player.GetProperty(name) is Property property) {
                    return property;
                }
                else
                    Console.WriteLine($"Ошибка! Недвижимости \"{name}\" не найдена");

            }
        }
    }

}
