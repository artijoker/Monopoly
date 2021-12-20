using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly {
    abstract class Property : ICell {

        public Property(string name, ConsoleColor color, int price, int rent, int pledge, int buyout) {
            Name = name;
            Color = color;
            Price = price;
            Rent = rent;
            Pledge = pledge;
            Buyout = buyout;
            Owner = null;
            IsPledged = false;

        }
        public string Name { get; }

        public void Action(Player player) {

            if (Owner != null) {
                Purchased(player);
                return;
            }
            else if (IsPledged) {
                Pledged(player);
                return;
            }
            else {
                ForSale(player);
                if (Owner == null) {
                    Console.WriteLine($"{player.Name} отказался покупать {Name}. Улица выставлен на аукцион!");
                    Console.WriteLine();
                    Auction();
                }
            }
        }

        private void Auction() {
            IList<Player> players = Game.Players.ToList();
            int price = Price;
            int index = 0;
            string cmd;
            Console.WriteLine($"Начальная цена лота {price}$");
            while (players.Count > 0) {
                if (index == players.Count)
                    index = 0;
                int stavka;
                while (true) {
                    Console.Write($"{players[index].Name} сделать ставку (да/нет)\n>");
                    cmd = Console.ReadLine().ToLower();
                    if (cmd == "да") {
                        Console.WriteLine($"Игрок: {players[index].Name}");
                        Console.WriteLine($"В кошельке: {players[index].Money}");
                        if (players[index].Money < price) {
                            Console.WriteLine("У вас недостаточно средств для ставки");
                            continue;
                        }
                        while (true) {
                            try {
                                while (true) {
                                    Console.Write("Ставка: ");
                                    stavka = int.Parse(Console.ReadLine());
                                    if (stavka < 0)
                                        Console.WriteLine("Ставка не может быть отрицательной");
                                    else if (stavka > players[index].Money)
                                        Console.WriteLine("Ставка не может превышать текуший баланс вашего счета");
                                    else
                                        break;
                                }
                                break;
                            }
                            catch (FormatException) {
                                Console.WriteLine("Ошибка! Ввод некорректных данных");
                                Console.WriteLine("Нажмите Enter, чтобы продолжить");
                                Console.ReadKey();
                                Console.Clear();
                            }
                        }
                        price += stavka;
                        Console.WriteLine($"Текущая ставка {price}$");
                        if(players.Count == 1) {
                            Console.WriteLine($"{Name} покупает {players.First().Name} за {price}$");
                            players.First().Money -= price;
                            Owner = players.First();
                            players.First().AddProperty(this);
                            Console.WriteLine();
                            return;
                        }
                        index++;
                        break;
                    }
                    else if (cmd == "нет") {
                        Console.WriteLine($"Игрок {players[index].Name} покинул аукцион");
                        players = players.Where(player => player != players[index]).ToList();
                        break;
                    }
                    else {
                        Console.WriteLine($"Ошибка! Команда \"{cmd}\" не найдена");
                    }
                }
            }
            if (players.Count == 0) {
                Console.WriteLine($"Все игроки покинуля аукцион");
                Console.WriteLine($"{Name} остается в свободной продаже");
                Console.WriteLine();
                return;
            }
        }

        private void Purchased(Player player) {
            if (player == Owner) {
                Console.WriteLine("Вы попали на купленную вами клетку");
                Console.WriteLine("Это ваша собственность");
                Console.WriteLine($"Приносит доход в размере {Rent}$");
                Console.WriteLine();
                return;
            }
            Console.WriteLine("Вы попали на клетку купленную другим игроком");
            Console.WriteLine($"{Name} принадлежит игроку {Owner.Name}.");
            Console.WriteLine($"С вас взымается рента в размере {Rent}$");
            player.Money -= Rent;
            Console.WriteLine();
        }

        private void Pledged(Player player) {
            string cmd;
            while (true) {
                Console.WriteLine("Вы попали на клетку купленную другим игроком");
                Console.WriteLine($"{Name} принадлежит игроку {Owner.Name} но находится в залоге у банка");
                Console.WriteLine($"Выкупная цена {Buyout}$");
                Console.Write("Хотите выкупить эту улицу (да/нет)\n>");
                cmd = Console.ReadLine().ToLower();
                if (cmd == "да") {
                    Console.WriteLine($"Игрок {player.Name} выкупает у банка {Name} за {Buyout}$ и становится новым владельцем");
                    player.Money -= Buyout;
                    Owner.RemoveProperty(this);
                    Console.WriteLine($"Игрок {Owner.Name} больше не владеет {Name}");
                    Owner = player;
                    player.AddProperty(this);
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

        private void ForSale(Player player) {
            string cmd;
            while (true) {
                Console.WriteLine("Вы попали на свободную клетку");
                Console.WriteLine($"{Name} Цена {Price}$");
                Console.Write("Хотите купить эту улицу (да/нет)\n>");
                cmd = Console.ReadLine().ToLower();
                if (cmd == "да") {
                    Console.WriteLine($"Игрок {player.Name} купил {Name} за {Price}$");
                    player.Money -= Price;
                    player.AddProperty(this);
                    Owner = player;
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

        public int Print(int cursorLeft) {
            string result;
            if (Owner != null) {
                if (IsPledged)
                    result = Name.Insert(Name.Length, $" Выкупная {Buyout}$");
                else
                    result = Name.Insert(Name.Length, $" Рента {Price}$");
            }
            else
                result = Name.Insert(Name.Length, $" Цена {Rent}$");
            string[] strings = result.Split(' ');
            int max = strings.Max(@string => @string.Length);

            Console.BackgroundColor = Color;
            Console.WriteLine(new string(' ', max));
            Console.BackgroundColor = ConsoleColor.Black;
            int count = 0;
            foreach (var @string in strings) {
                Console.CursorLeft = cursorLeft;
                Console.WriteLine(@string);
                ++count;
            }
            cursorLeft += max + 1;
            Console.CursorTop -= (count + 1);
            return cursorLeft;
        }


        public Player Owner { get; set; }
        public int Price { get; }
        public int Rent { get; set; }
        public abstract int IncreaseRent { get; }
        public abstract bool IsBeSold { get; set; }
        public int Pledge { get; }
        public int Buyout { get; }
        public bool IsPledged { get; set; }
        public ConsoleColor Color { get; }

    }

}
