using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FootballTeamCSharp
{
    class UserTeam : Team
    {

        public UserTeam(string name, List<Player> players, League league) : base(name, players, league) {}

        public UserTeam(string name, int low, int high, League league) : base(name, low, high, league) { }

        public void changeLineup()
        {
            Console.WriteLine("Select one player to swap out");
            displaySquad(true);
            Console.WriteLine("Please Select: ");
            int temp = Program.checkInputForValidInt(Players.Count);
            Console.WriteLine("Select one player to swap in");
            displaySquad(true);
            Console.WriteLine("Please Select: ");
            int temp2 = Program.checkInputForValidInt(Players.Count);
            Player tempP = Players[temp2];
            Players[temp2] = Players[temp];
            Players[temp] = tempP;
        }

        public override void takePlayer(Team otherTeam)
        /** Completes the process of the winning team swapping a player of their choice with the losing team */
        {
            Console.WriteLine("Please select a player to take");
            otherTeam.displaySquad(true);
            int choice = Program.checkInputForValidInt(otherTeam.Players.Count);
            while (otherTeam.Players[choice].Protect)
            {
                Console.WriteLine("This player is protected! Please choose another");
                choice = Program.checkInputForValidInt(otherTeam.Players.Count);
            }
            Console.WriteLine("Please select a player to give");
            displaySquad(true);
            int choice2 = Program.checkInputForValidInt(otherTeam.Players.Count);
            Player temp = otherTeam.Players[choice];
            otherTeam.Players[choice] = Players[choice2];
            Players[choice2] = temp;
        }

        public void protectPlayer()
        {
            foreach (Player player in Players)
            {
                player.Protect = false;
            }
            Console.WriteLine("Please select a player to protect");
            displaySquad(true);
            Console.WriteLine("Please Select\n");
            int choice = Program.checkInputForValidInt(Players.Count - 1);
            Players[choice].Protect = true;
        }
    }
}
