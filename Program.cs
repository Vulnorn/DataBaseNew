using System;
using System.Collections.Generic;

namespace DataBasePlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            const string AddNewPlayerMenu = "1";
            const string ShowPlayerMenu = "2";
            const string BanPlayerMenu = "3";
            const string UnbanPlayerMenu = "4";
            const string RemovePlayerMenu = "5";
            const string ExitMenu = "6";

            bool isWork = true;

            Database database = new Database();

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine($"Выберите пункт в меню:");
                Console.WriteLine($"{AddNewPlayerMenu} - Добавить нового игрока.");
                Console.WriteLine($"{ShowPlayerMenu} - Показать базу с игроками.");
                Console.WriteLine($"{BanPlayerMenu} - Забанить игрока.");
                Console.WriteLine($"{UnbanPlayerMenu} - Разбанить игрока.");
                Console.WriteLine($"{RemovePlayerMenu} - Удалить игрока.");
                Console.WriteLine($"{ExitMenu} - Выход");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case AddNewPlayerMenu:
                        database.CreatePlayer();
                        break;

                    case ShowPlayerMenu:
                        database.ShowAllPlayers();
                        break;

                    case BanPlayerMenu:
                        database.BanPlayer();
                        break;

                    case UnbanPlayerMenu:
                        database.UnbanPlayer();
                        break;

                    case RemovePlayerMenu:
                        database.RemovePlayer();
                        break;

                    case ExitMenu:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Ошибка ввода команды.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }

    class Database
    {
        private List<Player> _players = new List<Player>();

        public int Ids { get; private set; }

        public void CreatePlayer()
        {
            string userInputNamePlayer;
            int levelPlayer;
            string userInputLevelPlayer;
            int minimumLengthNamePlayer = 1;
            int minimumLevelPlayer = 1;
            int maximumLevelPlayer = 100;

            Ids++;

            Console.Clear();
            Console.WriteLine($"Введите ник персонажа");
            userInputNamePlayer = Console.ReadLine();

            while (userInputNamePlayer.Length < minimumLengthNamePlayer)
            {
                Console.Clear();
                Console.Write("Поле логина не может быть пустым. Введите ник персонажа: ");
                userInputNamePlayer = Console.ReadLine();
            }

            do
            {
                Console.Clear();
                Console.Write($"Уровень персонажа не может быть ниже {minimumLevelPlayer} и выше {maximumLevelPlayer}. Введите уровень персонажа: ");
                userInputLevelPlayer = Console.ReadLine();

                TryGetInputValue(userInputLevelPlayer, out levelPlayer);
            }
            while (GetNumberRange(levelPlayer, minimumLevelPlayer, maximumLevelPlayer));

            _players.Add(new Player(userInputNamePlayer, levelPlayer, Ids));
        }

        public void ShowAllPlayers()
        {
            if (_players.Count < 1)
            {
                Console.WriteLine($"База данных пуста. Добавьте Игроков");
                Console.ReadKey();
            }
            else
            {
                for (int i = 0; i < _players.Count; i++)
                {
                    _players[i].ShowInfo();
                }
            }

            Console.ReadKey();
        }

        public void RemovePlayer()
        {
            if (TryGetPlayer(out Player player))
            {
                _players.Remove(player);
                Console.WriteLine("Игрок с таким ID найден и удален.");
                Console.ReadKey();
            }
        }

        public void BanPlayer()
        {
            if (TryGetPlayer(out Player player))
            {
                player.Ban();
                Console.WriteLine("Персонаж получил бан.");
                Console.ReadKey();
            }
        }

        public void UnbanPlayer()
        {
            if (TryGetPlayer(out Player player))
            {
                player.Unban();
                Console.WriteLine("С Персонажа снят бан.");
                Console.ReadKey();
            }
        }

        private bool TryGetPlayer(out Player player)
        {
            player = null;

            Console.WriteLine("Введите уникальный ID игрока");
            string userInput = Console.ReadLine().Trim();

            if (TryGetInputValue(userInput, out int id) == false)
                return false;

            for (int i = 0; i < _players.Count; i++)
            {
                if (id == _players[i].Id)
                {
                    player = _players[i];
                    return true;
                }
            }

            Console.WriteLine("Нет такого игрока.");
            Console.ReadKey();
            return false;
        }

        private bool TryGetInputValue(string value, out int number)
        {
            if (int.TryParse(value, out number) == false)
            {
                Console.WriteLine("Не корректный ввод.");
                Console.ReadKey();
                return false;
            }

            return true;
        }

        private bool GetNumberRange(int number, int minimumLevelPlayer, int maximumLevelPlayer)
        {
            if (number < minimumLevelPlayer || number > maximumLevelPlayer)
                return true;

            return false;
        }
    }

    class Player
    {

        public Player(string name, int level, int id)
        {
            Id = id;
            Name = name;
            Level = level;
            IsBanned = false;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Level { get; private set; }
        public bool IsBanned { get; private set; }

        public void ShowInfo()
        {
            string banStatus = IsBanned ? " Забанен" : "";

            Console.WriteLine($"Уникальный Id Персонажа: {Id} Ник: {Name} Уровень: {Level}{banStatus}");
        }

        public void Ban()
        {
            IsBanned = true;
        }

        public void Unban()
        {
            IsBanned = false;
        }
    }
}