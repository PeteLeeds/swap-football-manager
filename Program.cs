using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace FootballTeamCSharp
{

    class Program
    {

        static Random rnd = new Random();

        public static string[] firstNames = System.IO.File.ReadAllLines(@"D:\Names\EnglishFirst.txt");
        public static string[] lastNames = System.IO.File.ReadAllLines(@"D:\Names\EnglishLast.txt");
        public static string[] positions = { "GK", "DEF", "DEF", "DEF", "DEF", "MID", "MID", "MID", "MID", "FW", "FW",};

        public static int checkInputForValidInt(int max)
        {
            bool isInt = false;
            int result = 0;
            while (!isInt)
            {
                isInt = true;
                string choice = Console.ReadLine();
                try
                {
                    result = Int16.Parse(choice);
                    if (result > max)
                    {
                        Console.WriteLine("That is not a valid input. Please try again");
                        isInt = false;
                    }
                }
                catch
                {
                    isInt = false;
                    Console.WriteLine("That is not a valid input. Please try again");
                }
            }
            return result;
        }

        public static void menu(Game game)
        {
            Console.WriteLine("Week " + game.Week);
            game.UserTeam.displaySquad(false);
            Console.WriteLine("\nPress 1 to play a match\nPress 2 to protect a player\n" +
                "Press 3 to change the line-up\nPress 4 to view the League\nPlease Select");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                game.next();
            }
            if (choice == "2")
            {
                game.UserTeam.protectPlayer();
            }
            if (choice == "3")
            {
                game.UserTeam.changeLineup();
            }
            if (choice == "4")
            {
                Console.WriteLine(game.UserTeam.League);
                Console.ReadLine();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the name of your team: ");
            List<League> leagues = new List<League>();
            for (int i = 1; i < 5; i++)
            {
                if (i == 3)
                {
                    leagues.Add(new League("League 3", 20, 56, 74, Console.ReadLine()));
                }
                else
                {
                    leagues.Add(new League("League " + i, 20, (80 - i * 8), (98 - i * 8)));
                }
                if (!(i==1))
                {
                    leagues[i - 1].UpperLeague = leagues[i - 2];
                    leagues[i - 2].LowerLeague = leagues[i - 1];
                }
            }
            Game game = new Game(0, leagues);
            while (true)
            {
                menu(game);
            }
        }
    }
}
