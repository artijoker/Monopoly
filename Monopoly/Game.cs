using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopoly {

    class Game {
        private readonly Field _field;
        private List<Player> _players;

        private static Field field;
        private static IReadOnlyList<Player> players;

        public static Field Field => field;
        public static IReadOnlyList<Player> Players => players;

        public Game(Field playingField) {
            _field = playingField;
            field = playingField;
        }

        private void CreatingPlayers() {
            int numberOfPlayers;
            while (true) {
                try {
                    while (true) {
                        Console.Write("Введите количество игроков от 2 до 5\n>");
                        numberOfPlayers = int.Parse(Console.ReadLine());
                        if (numberOfPlayers < 2) {
                            Console.WriteLine("В игре не может быть меньше двух игроков");
                        }
                        else if (numberOfPlayers > 5) {
                            Console.WriteLine("В игре не может быть больше пяти игроков");
                        }
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
            Console.WriteLine();
            IList<string> names = new List<string>();
            for (int i = 0; i < numberOfPlayers; i++) {
                while (true) {
                    Console.Write($"Введите имя игрока {i + 1}\n>");
                    string name = Console.ReadLine();
                    if (name == "") {
                        Console.WriteLine("Имя игрока не может быть пустым");
                    }
                    else if (names.Any(item => item == name)) {
                        Console.WriteLine("У игроков не может быть одинаковыми имен");
                    }
                    else {
                        names.Add(name);
                        break;
                    }
                }
            }
            Console.Clear();
            OrderMoves(names);
        }

        private void OrderMoves(IList<string> names) {
            Console.WriteLine("Перед начало игры каждый игрок должен бросить кубики для выяснения очередности ходов");
            Console.WriteLine();
            List<Tuple<int, Player>> playersAndPoints = new List<Tuple<int, Player>>();
            for (int index = 0; index < names.Count; index++) {
                Console.WriteLine($"Игрок {names[index]} бросает кубики");
                Console.WriteLine("Для броска нажмите Enter");
                Console.ReadKey();
                (int, int) roll = RollTheDice();
                Console.WriteLine($"У игрока {names[index]} выпало {roll.Item1} и {roll.Item2}");
                Console.WriteLine();
                playersAndPoints.Add(new Tuple<int, Player>(roll.Item1 + roll.Item2, new Player(names[index])));

            }
            _players = playersAndPoints.OrderByDescending(pair => pair.Item1).Select(pair => pair.Item2).ToList();
            players = _players;
            Console.WriteLine("Порядок очерёдности ходов:");
            for (int index = 0; index < _players.Count; index++)
                Console.WriteLine($"{index + 1}-й игрок {_players[index].Name}");

            Console.WriteLine("Нажмите Enter, чтобы продолжить");
            Console.ReadKey();
            Console.Clear();
        }

        public void RunGame() {
            Console.WindowWidth = Console.LargestWindowWidth - 15;
            CreatingPlayers();
            int index = 0;
            while (_players.Count > 1) {
                if (index == _players.Count) {
                    index = 0;
                }
                PlayerAction(_players[index]);
                Console.WriteLine("Нажмите Enter, чтобы завершить ход");
                Console.ReadKey();
                if (_players[index].Bankrupt == true)
                    _players = _players.Where(player => player.Bankrupt == false).ToList();
                else
                    index++;
            }
            Console.WriteLine($"Игрок {_players[0].Name} победил. На его счету {_players[0].Money}$");
            Console.WriteLine("Чтобы выйти нажмите любую клавишу");
            Console.ReadKey();
            return;
        }

        private void PlayerAction(Player player) {
            Console.Clear();
            GetPlayerActionHelp();
            PrintStreetsFromPosition(player.Position);

            Console.WriteLine($"Игрок: {player.Name}");
            Console.WriteLine($"В кошельке: {player.Money}");
            Console.WriteLine($"Текущая позиция: {_field[player.Position].Name}");
            while (true) {
                Console.Write(">");
                string cmd = Console.ReadLine().ToLower();
                if (cmd == "бросок") {
                    if (player.IsIntoMinus()) {
                        Console.WriteLine($"Ваш текущий баланс составляет {player.Money}$.");
                        Console.WriteLine($"Вы не можете бросить кубики пока ваш баланс отрицательный.");
                        continue;
                    }
                    if (player.IsPrisoner()) {
                        _field[player.Position].Action(player);
                        if (player.Prisoner > 0) {
                            player.Prisoner--;
                            return;
                        }
                    }
                    Move(player);
                    return;
                }
                else if (cmd == "управление") {
                    Management(player);
                    GetPlayerActionHelp();
                    PrintStreetsFromPosition(player.Position);
                }
                else if (cmd == "игрок") {
                    Console.WriteLine($"Игрок: {player.Name}");
                    Console.WriteLine($"В кошельке: {player.Money}");
                    Console.WriteLine();
                }
                else if (cmd == "имущество") {
                    player.PrintAllProperty();
                    Console.WriteLine();
                }
                else if (cmd == "показать") {
                    PrintStreetsFromPosition(player.Position);
                }
                else if (cmd == "карта") {
                    PrintMap();
                }
                else if (cmd == "банкрот") {
                    Console.Write("Вы уверены (да/нет)\n>");
                    cmd = Console.ReadLine().ToLower();
                    if (cmd == "да") {
                        Bankruptcy(player);
                        return;
                    }
                    else if (cmd == "нет") {
                    }
                    else
                        Console.WriteLine($"Ошибка! Команда \"{cmd}\" не найдена");
                }
                else if (cmd == "помощь") {
                    GetPlayerActionHelp();
                }
                else if (cmd == "очистить") {
                    Console.Clear();
                }
                else {
                    Console.WriteLine($"Ошибка! Команда \"{cmd}\" не найдена");
                }
            }
        }

        private void Management(Player player) {
            while (true) {
                GetManagementHelp();
                Console.Write(">");
                string cmd = Console.ReadLine().ToLower();
                if (cmd == "торговать") {
                    Trading.Trade(player);
                }
                else if (cmd == "кредит") {
                    Credit.CreditSecuredProperty(player);
                }
                else if (cmd == "выкуп") {
                    Buyout.BuyoutMortgagedProperty(player);
                }
                else if (cmd == "монополия") {
                    BuySellHouse.BuySell(player);
                }
                else if (cmd == "помощь") {
                    GetManagementHelp();
                }
                else if (cmd == "очистить") {
                    Console.Clear();
                }
                else if (cmd == "назад") {
                    Console.Clear();
                    return;
                }
                else {
                    Console.WriteLine($"Ошибка! Команда \"{cmd}\" не найдена");

                }
            }
        }

        private void Bankruptcy(Player player) {
            Console.WriteLine($"Игрок {player.Name} банкрот");
            Console.WriteLine($"Игрок {player.Name} покидает игру");
            player.Bankrupt = true;
            return;
        }

        private void PrintStreetsFromPosition(int position) {
            Console.WindowWidth = Console.LargestWindowWidth - 15;
            int cursorLeft = Console.CursorLeft;
            int length = position + 12;
            for (int index = position; index <= length; index++) {
                cursorLeft = _field[index].Print(cursorLeft);
                Console.CursorLeft = cursorLeft;
                if (index == _field.FieldSize - 1) {
                    length -= index;
                    index = 0;
                    cursorLeft = _field[index].Print(cursorLeft);
                    Console.CursorLeft = cursorLeft;
                }
            }
            Console.CursorLeft = 0;
            Console.CursorTop += 7;
            Console.WriteLine();
        }
        private void PrintMap() {
            Console.WindowWidth = Console.LargestWindowWidth - 15;
            int cursorLeft = Console.CursorLeft;
            for (int index = 0; index < _field.FieldSize; index++) {
                if (index % 12 == 0 && index != 0) {
                    Console.CursorLeft = 0;
                    Console.CursorTop += 7;
                    cursorLeft = 0;
                }
                cursorLeft = _field[index].Print(cursorLeft);
                Console.CursorLeft = cursorLeft;
            }
            Console.CursorLeft = 0;
            Console.CursorTop += 7;
            Console.WriteLine();
        }

        private void Move(Player player) {
            (int, int) roll;
            int newPosition;
            int count = 0;
            do {
                Console.WriteLine("Для броска нажмите Enter");
                Console.ReadKey();
                roll = RollTheDice();
                Console.WriteLine($"У вас выпало {roll.Item1} и {roll.Item2}");
                
                if (roll.Item1 == roll.Item2) {
                    if (count == 3) {
                        Console.WriteLine("Вы первысили скорость и отпраляетесь в тюрьму");
                        newPosition = 7;
                        player.Position = newPosition;
                        _field[newPosition].Action(player);
                        return;
                    }
                    Console.WriteLine("У вас выпал дубль вы ходите еще раз");
                    count++;
                }
                Console.WriteLine();

                newPosition = player.Position + roll.Item1 + roll.Item2;
                if (newPosition > _field.FieldSize) {
                    newPosition -= _field.FieldSize;
                    player.Position = 0;
                    _field[player.Position].Action(player);
                    player.Position = newPosition;
                    _field[player.Position].Action(player);
                }
                else if (newPosition == _field.FieldSize) {
                    player.Position = 0;
                    _field[player.Position].Action(player);
                }
                else {
                     player.Position = newPosition;
                    _field[newPosition].Action(player);
                }
                if (roll.Item1 == roll.Item2) {
                    PrintStreetsFromPosition(player.Position);
                    Console.WriteLine($"Игрок: {player.Name}");
                    Console.WriteLine($"В кошельке: {player.Money}");
                    Console.WriteLine($"Текущая позиция: {_field[player.Position].Name}");
                }
            } while (roll.Item1 == roll.Item2 && !player.IsPrisoner());

        }

        private (int, int) RollTheDice() => (Dice.Roll(), Dice.Roll());



        private void GetGeneralHelp() {
            Console.WriteLine("помощь - информация о командах");
            Console.WriteLine("очистить - очистить экран");
        }

        private void GetPlayerActionHelp() {
            GetGeneralHelp();
            Console.WriteLine("бросок - бросить кубики и сделать ход, если вы в тюрьме то отсидеть срок");
            Console.WriteLine("управление - управление вашим имуществом");
            Console.WriteLine("игрок - информация о игроке");
            Console.WriteLine("имущество - список имущества которым владеет игрок");
            Console.WriteLine("показать - показать следующие 12 клеток игрового поля");
            Console.WriteLine("карта - карта поля");
            Console.WriteLine("банкрот - объявить что вы банкрот и выйти из игры");
        }

        private void GetManagementHelp() {
            GetGeneralHelp();
            Console.WriteLine("торговать - торговать с другими игроками");
            Console.WriteLine("кредит - кредит под залог недвижимости");
            Console.WriteLine("выкуп - выкупить свою недвижимость у банка");
            Console.WriteLine("монополия - покупка продажа домиков если у вас монополия на один цвет улиц");
            Console.WriteLine("назад - вернуться в прошлое меню");

        }
    }

}

