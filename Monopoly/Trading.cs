using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly {
    class Trading {

        public static void Trade(Player offering) {
            while (true) {
                Console.WriteLine("Список игроков:");
                Game.Players.Where(item => item != offering).ForEach(item => Console.WriteLine(item.Name));
                Console.Write("С кем торговать:\n>");
                string name = Console.ReadLine();
                if (Game.Players.Where(item => item.Name == name).FirstOrDefault() is Player requesting) {
                    if (!IsSomethingTrade(offering)) {
                        Console.WriteLine("Вам не чем торговать");
                        return;
                    }
                    if (!IsSomethingTrade(requesting)) {
                        Console.WriteLine($"Игроку {requesting.Name} не чем торговать");
                        return;
                    }
                    Console.WriteLine();
                    Deal(offering, requesting);
                }
                else
                    Console.WriteLine($"Ошибка! Игрок \"{name}\" не найдена");
                return;
            }
        }

        private static bool IsSomethingTrade(Player player) => player.Money != 0 || player.IsHaveProperty();

        private static void Deal(Player player1, Player player2) {
            ITradable offerPlayer1 = PlayerSuggestion(player1);
            Console.WriteLine($"Передайте управление игроку {player2.Name}");
            ITradable offerPlayer2 = PlayerSuggestion(player2);
            if (ConfirmationDeal(player1) && ConfirmationDeal(player2)) {

                if (offerPlayer1.Money.HasValue && offerPlayer2.Property != null) {
                    TransferMoney(player1, player2, offerPlayer1.Money.Value);
                    TransferProperty(player2, player1, offerPlayer2.Property);
                }
                else if (offerPlayer1.Property != null && offerPlayer2.Money.HasValue) {
                    TransferMoney(player2, player1, offerPlayer2.Money.Value);
                    TransferProperty(player1, player2, offerPlayer1.Property);

                }
                else if (offerPlayer1.Property != null && offerPlayer2.Property != null) {
                    TransferProperty(player1, player2, offerPlayer1.Property);
                    TransferProperty(player2, player1, offerPlayer2.Property);
                }
                else {
                    TransferMoney(player1, player2, offerPlayer1.Money.Value);
                    TransferMoney(player2, player1, offerPlayer2.Money.Value);
                }
                Console.WriteLine("Сделка состоялась");
            }
            else 
                Console.WriteLine("Сделка не состоялась");
            
            Console.WriteLine("Нажмите Enter, чтобы продолжить");
            Console.ReadKey();
        }

        private static bool ConfirmationDeal(Player player) {
            string cmd;
            while (true) {
                Console.WriteLine($"Игрок {player.Name} согласен на сделку? (да/нет)");
                cmd = Console.ReadLine().ToLower();
                if (cmd == "да") {
                    return true;
                }
                else if (cmd == "нет")
                    return false;
                else {
                    Console.WriteLine($"Ошибка! Команда \"{cmd}\" не найдена");
                }
            }
        }

        private static ITradable PlayerSuggestion(Player player) {
            ITradable tradable = Offer(player);
            if (tradable.Money.HasValue) {
                Console.WriteLine($"Игрок {player.Name} предлагает {tradable.Money.Value}$");
                Console.WriteLine();
                return tradable;
            }
            else {
                Console.WriteLine($"Игрок {player.Name} предлагает {tradable.Property.Name}");
                Console.WriteLine();
                return tradable;
            }
        }

        private static ITradable Offer(Player player) {
            while (true) {
                GetOfferHelp();
                Console.Write(">");
                string cmd = Console.ReadLine().ToLower();
                if (cmd == "деньги") {
                    if (player.IsIntoMinus()) {
                        Console.WriteLine("У вас отрицательный баланс");
                        continue;
                    }
                    int money = InputMoney(player);
                    return new MoneyTradable(money);
                }
                else if (cmd == "улица") {
                    if (!player.IsHaveProperty()) {
                        Console.WriteLine("У вас нет недвижимости");
                        continue;
                    }
                    Property property = SelectionProperty(player);
                    return new StreetTradable(property);
                }
                else 
                    Console.WriteLine($"Ошибка! Команда \"{cmd}\" не найдена");
            }
        }

        private static int InputMoney(Player player) {
            while (true) {
                try {
                    while (true) {
                        Console.WriteLine($"Ваш баланс:{player.Money}");
                        Console.Write("Введите сумму:\n>");
                        int money = int.Parse(Console.ReadLine());
                        if (money < 0)
                            Console.WriteLine("Сумма денег не может быть отрицательной");
                        else if (money > player.Money)
                            Console.WriteLine("Сумма денег не может превышать ваш текуший баланс");
                        else
                            return money;
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

        private static Property SelectionProperty(Player player) {

            while (true) {
                Console.WriteLine("Список имущества которое можно продать:");
                player.PrintPropertyForSale();
                Console.Write("Введите название недвижимости\n>");
                string name = Console.ReadLine();
                if (player.GetProperty(name) is Property property) {
                    return property;
                }
                else
                    Console.WriteLine($"Ошибка! Недвижимости \"{name}\" не найдена");
            }
        }

        private static void TransferProperty(Player transmitting, Player receiving, Property property) {
            transmitting.RemoveProperty(property);
            Console.WriteLine($"Игрок {transmitting.Name} больше не владеет {property.Name}");
            receiving.AddProperty(property);
            property.Owner = receiving;
            Console.WriteLine($"Игрок {receiving.Name} становится новым владельцем {property.Name}");
            Console.WriteLine();

        }

        private static void TransferMoney(Player transmitting, Player receiving, int amount) {
            transmitting.Money -= amount;
            Console.WriteLine($"Со счета игрока {transmitting.Name} списалось {amount}$");
            receiving.Money += amount;
            Console.WriteLine($"На счет игрока {receiving.Name} поступило {amount}$");
            Console.WriteLine();
        }

        private static void GetOfferHelp() {
            Console.WriteLine("Улица - предложить улицу");
            Console.WriteLine("Деньги - предложить деньги");
        }
    }
}
