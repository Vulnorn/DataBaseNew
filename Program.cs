using System;
using System.Collections.Generic;

namespace DataBasePlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            const string CommandAddNewPlayer = "1";
            const string CommandShowPlayer = "2";
            const string CommandBanPlayer = "3";
            const string CommandUnbanPlayer = "4";
            const string CommandRemovePlayer = "5";
            const string CommandExit = "6";

            bool isWork = true;

            Database database = new Database();

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine($"Выберите пункт в меню:");
                Console.WriteLine($"{CommandAddNewPlayer} - Добавить нового игрока.");
                Console.WriteLine($"{CommandShowPlayer} - Показать базу с игроками.");
                Console.WriteLine($"{CommandBanPlayer} - Забанить игрока.");
                Console.WriteLine($"{CommandUnbanPlayer} - Разбанить игрока.");
                Console.WriteLine($"{CommandRemovePlayer} - Удалить игрока.");
                Console.WriteLine($"{CommandExit} - Выход");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandAddNewPlayer:
                        database.CreateNewPlayer();
                        break;

                    case CommandShowPlayer:
                        database.ShowAllPlayeres();
                        break;

                    case CommandBanPlayer:
                        database.BanPlayer();
                        break;

                    case CommandUnbanPlayer:
                        database.UnbanPlayer();
                        break;

                    case CommandRemovePlayer:
                        database.RemovePlayer();
                        break;

                    case CommandExit:
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

        public void CreateNewPlayer()
        {
            string userInputNamePlayer = null;
            string userInputLevelPlayer;
            int minimumLengthNamePlayer = 1;
            int levelPlayer = 0;
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

            Console.Clear();
            Console.WriteLine($"Введите уровень персонажа. Он не должен быть ниже {minimumLevelPlayer} и {maximumLevelPlayer}");
            userInputLevelPlayer = Console.ReadLine();

            levelPlayer = VerifyInput(userInputLevelPlayer);

            while (levelPlayer < minimumLevelPlayer || levelPlayer > maximumLevelPlayer)
            {
                Console.Clear();
                Console.Write($"Уровень персонажа не может быть ниже {minimumLevelPlayer} и выше {maximumLevelPlayer}. Введите уровень персонажа: ");
                userInputLevelPlayer = Console.ReadLine();

                levelPlayer = VerifyInput(userInputLevelPlayer);
            }

            _players.Add(new Player(userInputNamePlayer, levelPlayer,Ids));
        }


        public void ShowAllPlayeres()
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
            }
        }

        public void UnbanPlayer()
        {
            if (TryGetPlayer(out Player player))
            {
                player.Unban();
            }
        }

        private bool TryGetPlayer(out Player player)
        {
            int id;
            player = null;

            Console.WriteLine("Введите уникальный ID игрока");
            string userInput = Console.ReadLine().Trim();

            if (int.TryParse(userInput, out id) == false)
            {
                Console.WriteLine("Не корректный ввод.");
                Console.ReadKey();
                return false;
            }

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

        private int VerifyInput(string userInputLevelPlayer)
        {
            int levelPlayer;
            if (int.TryParse(userInputLevelPlayer, out levelPlayer) == false)
            {
                Console.WriteLine("Не корректный ввод.");
                Console.ReadKey();
            }

            return levelPlayer;
        }
    }

    class Player
    {

        public Player(string name, int level, int ids)
        {
            Id = ids;
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
            Console.WriteLine($"Уникальный Id Персонажа: {Id} Ник: {Name} Уровень: {Level}{(IsBanned ? " Забанен" : "")}");
        }

        public void Ban()
        {
            IsBanned = true;
            Console.WriteLine("Персонаж получил бан.");
            Console.ReadKey();
        }

        public void Unban()
        {
            IsBanned = false;
            Console.WriteLine("С Персонажа снят бан.");
            Console.ReadKey();
        }
    }
}