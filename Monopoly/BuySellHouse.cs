using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly {
    class BuySellHouse {

        public static void BuySell(Player player) {
            Console.Clear();
            string cmd;
            if (!player.IsHaveProperty()) {
                Console.WriteLine("У вас нет недвижимости");
                Console.WriteLine();
                return;
            }
            while (true) {
                Console.Write($"Управлять домиками (да/нет)\n>");
                cmd = Console.ReadLine().ToLower();
                if (cmd == "да") {
                    while (true) {
                        if (Game.Field.IsMonopoly(player)) {
                            IReadOnlyList<Street> streets = Game.Field.GetStreetsWhichPlayerHasMonopoly(player);
                            while (true) {
                                Street street = StreetSelection(streets);
                                GetHelpBuySell();

                                while (true) {
                                    Console.WriteLine($"Управление улицей {street.Name}");
                                    Console.Write(">");
                                    cmd = Console.ReadLine().ToLower();
                                    if (cmd == "покупка") {
                                        bool check = streets
                                            .Where(item => item.Color == street.Color)
                                            .All(item => (street.NumberOfHouses + 1 - item.NumberOfHouses) == 0 ||
                                            (street.NumberOfHouses + 1 - item.NumberOfHouses) == 1);
                                        if (check) {
                                            BuyingHouse(player, street);
                                            if (street.NumberOfHouses > 0)
                                                streets.Where(item => item.Color == street.Color)
                                                    .ForEach(item => item.IsBeSold = false);
                                        }
                                        else {
                                            Console.WriteLine("Покупка домов должа быть равномерной.");
                                            Console.WriteLine("Число домиков на улицах одной монополии должно отличаться не более, чем на один.");
                                        }
                                    }
                                    else if (cmd == "продажа") {
                                        bool check = streets.Where(item => item.Color == street.Color)
                                            .All(item => (item.NumberOfHouses - (street.NumberOfHouses - 1)) == 0 ||
                                            (item.NumberOfHouses - (street.NumberOfHouses - 1)) == 1);
                                        if (check) {
                                            HouseSale(street);
                                            if (streets.Where(item => item.Color == street.Color).All(item => item.NumberOfHouses == 0)) {
                                                streets.Where(item => item.Color == street.Color)
                                                    .ForEach(item => item.IsBeSold = true);
                                            }
                                        }
                                        else {
                                            Console.WriteLine("Продажа домов должа быть равномерной.");
                                            Console.WriteLine("Число домиков на улицах одной монополии должно отличаться не более, чем на один.");
                                        }
                                    }
                                    else if (cmd == "назад") {
                                        return;
                                    }
                                    else if (cmd == "выбор") {
                                        break;
                                    }
                                    else if (cmd == "помощь") {
                                        GetHelpBuySell();
                                    }
                                    else if (cmd == "очистить") {
                                        Console.Clear();
                                    }
                                    else {
                                        Console.WriteLine($"Ошибка! Команда \"{cmd}\" не найдена");
                                    }
                                }
                            }
                        }
                        else {
                            Console.WriteLine("У вас нет монополия на один цвет улиц");
                            Console.WriteLine();
                            return;
                        }
                    }
                }
                else if (cmd == "нет")
                    return;
                else
                    Console.WriteLine($"Ошибка! Команда \"{cmd}\" не найдена");
            }
        }

        private static void PrintMonopoly(IReadOnlyList<Street> streets) {
            Console.WriteLine($"{"Название улицы",-18} {"Цвет группы",11} {"Кол-во домов",15}");
            for (int index = 0; index < streets.Count; index++) {
                Console.Write($"{streets[index].Name,-18} ");
                Console.BackgroundColor = streets[index].Color;
                Console.Write($"{" ",11}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine($" {streets[index].NumberOfHouses,15} ");
            }
        }

        private static Street StreetSelection(IReadOnlyList<Street> streets) {
            string name;
            while (true) {
                try {
                    while (true) {
                        Console.WriteLine("Улицы на которые у вас монополия:");
                        PrintMonopoly(streets);
                        Console.Write("Введите название улицы\n>");
                        name = Console.ReadLine();
                        if (streets.Where(item => item.Name == name).FirstOrDefault() is Street street) {
                            return street;
                        }
                        else
                            Console.WriteLine($"Ошибка! Улица \"{name}\" не найдена");
                    }
                }
                catch (FormatException) {
                    Console.WriteLine("Ошибка! Ввод некорректных данных");
                    Console.WriteLine("Нажмите Enter, чтобы продолжить");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        private static void BuyingHouse(Player player, Street street) {
            string cmd;
            Console.WriteLine($"Количество домов на улице {street.Name} составляет {street.NumberOfHouses} шт.");
            if (street.NumberOfHouses == 5) {
                Console.WriteLine("Вы уже собрали отель на этой улице, больше покупать дома нельзя");
                return;
            }
            while (true) {
                Console.WriteLine($"Цена покупки дома: {street.HousePurchase}$");
                Console.Write("Купить дом (да/нет)\n>");

                cmd = Console.ReadLine().ToLower();
                if (cmd == "да") {
                    if (player.Money < street.HousePurchase) {
                        Console.WriteLine("У вас недостаточно средств для покупки");
                        return;
                    }
                    street.BuyHouse();
                    if (street.NumberOfHouses < 5) {
                        Console.WriteLine();
                        Console.WriteLine("Вы купили дом");
                        Console.WriteLine($"С вашего счета списалось {street.HousePurchase}$");
                        Console.WriteLine("Доход от улицы увеличился");
                        Console.WriteLine($"Рента улицы выросла до {street.Rent}$");
                        Console.WriteLine();
                    }
                    else {
                        Console.WriteLine();
                        Console.WriteLine("Поздравляем вы собрали отель");
                        Console.WriteLine($"С вашего счета списалось {street.HousePurchase}$");
                        Console.WriteLine("Улица приносит максимальный доход");
                        Console.WriteLine($"Рента улицы выросла до {street.Rent}$");
                        Console.WriteLine();
                    }
                    return;
                }
                else if (cmd == "нет")
                    return;
                else {
                    Console.WriteLine($"Ошибка! Команда \"{cmd}\" не найдена");
                }
            }

        }

        private static void HouseSale(Street street) {
            string cmd;
            Console.WriteLine($"Количество домов на улице {street.Name} составляет {street.NumberOfHouses} шт.");
            if (street.NumberOfHouses == 0) {
                Console.WriteLine("У вас не домов для продажи");
                return;
            }
            while (true) {
                Console.WriteLine($"Цена продажи дома: {street.HouseSale}$");
                Console.Write("Продать дом (да/нет)\n>");
                cmd = Console.ReadLine().ToLower();
                if (cmd == "да") {
                    street.SellHouse();
                    Console.WriteLine();
                    Console.WriteLine("Вы продали дом");
                    Console.WriteLine($"На вас счет зачисленно {street.HouseSale}$");
                    Console.WriteLine("Доход от улицы понизился");
                    Console.WriteLine($"Рента улицы понизилась до {street.Rent}$");
                    Console.WriteLine();
                    return;
                }
                else if (cmd == "нет")
                    return;
                else {
                    Console.WriteLine($"Ошибка! Команда \"{cmd}\" не найдена");
                }
            }

        }

        private static void GetHelpBuySell() {
            Console.WriteLine("помощь - информация о командах");
            Console.WriteLine("очистить - очистить экран");
            Console.WriteLine("выбор - выбрать другую улицу");
            Console.WriteLine("покупка - купить домик");
            Console.WriteLine("продажа - продать домик");
            Console.WriteLine("назад - вернутся в предыдущее меню");
        }
    }
}
