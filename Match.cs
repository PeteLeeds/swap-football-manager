using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace FootballTeamCSharp
{
    class Match
    {
        Team team1, team2;
        int team1Score = 0, team2Score = 0;
        Random rnd = new Random();

        public Match (Team team1, Team team2)
        {
            this.team1 = team1;
            this.team2 = team2;
        }

        public Team Team1
        {
            get { return this.team1; }
        }

        public Team Team2
        {
            get { return this.team2; }
        }

        public void sim()
        {
            team1.autoSquad();
            team2.autoSquad();
            float difference = team1.AverageSkill - team2.AverageSkill;
            if (difference > 5)
            {
                team1Score = rnd.Next(4);
                team2Score = rnd.Next(1);
            }
            else if (difference > 1)
            {
                team1Score = rnd.Next(3);
                team2Score = rnd.Next(2);
            }
            else if (difference < -5)
            {
                team1Score = rnd.Next(1);
                team2Score = rnd.Next(4);
            }
            else if (difference < -1)
            {
                team1Score = rnd.Next(2);
                team2Score = rnd.Next(3);
            }
            else
            {
                team1Score = rnd.Next(3);
                team2Score = rnd.Next(3);
            }
            outcome();
        }

        public Boolean oneVOne(Player player1, Player player2)
        {
            player1.Fitness--;
            player2.Fitness--;
            return(player1.PlayingSkill + (rnd.Next(20) - 10) > player2.PlayingSkill + (rnd.Next(20) - 10));
        }

        public void inMatchMenu()
        {
            Console.WriteLine("In-Match menu\nPress 1 to make a substitution\n" +
                "Press any other key to return to the game\nPlease Select");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                if (typeof(UserTeam) == team1.GetType()) { team1.Squad.makeSubstitution(); }
                else { team2.Squad.makeSubstitution(); }
                inMatchMenu();
            }
        }

        public void play()
        {
            Console.WriteLine(team1.Name + " vs " + team2.Name);
            Console.ReadLine();
            if (typeof(UserTeam) == team1.GetType())
            {
                team1.setMatchSquad();
                team2.autoSquad();
            }
            else
            {
                team2.setMatchSquad();
                team1.autoSquad();
            }
            team1.resetFitness();
            team2.resetFitness();
            for (int i = 1; i <= 90; i++)
            {
                Console.WriteLine(i);
                if (i % 4 == 0)
                {
                    team1.Squad.tirePlayers();
                    team2.Squad.tirePlayers();
                }
                if (i % 15 == 0 && i != 90)
                {
                    inMatchMenu();
                }
                if (rnd.Next(10) > 7)
                {
                    Player p1 = team1.Squad.random("MID"), p2 = team2.Squad.random("MID");
                    if (oneVOne(p1, p2))
                    {
                        Console.WriteLine(p1.Surname + " has the ball for " + team1.Name);
                        if (opportunity(team2.Squad.Gk, team2.Squad.random("DEF"), p1, team1.Squad.random("FW")))
                        {
                            team1Score++;
                        }
                    }
                    else
                    {
                        Console.WriteLine(p2.Surname + " has the ball for " + team2.Name);
                        if (opportunity(team1.Squad.Gk, team1.Squad.random("DEF"), p2, team2.Squad.random("FW")))
                        {
                            team2Score++;
                        }
                    }

                }
                Thread.Sleep(100);
            }
            Console.WriteLine("Final score: " + team1Score + " - " + team2Score);
            Console.ReadLine();
            outcome();
        }

        public bool opportunity(Player gk, Player def, Player mid, Player att)
        {
            if (oneVOne(mid, def))
            {
                Thread.Sleep(200);
                Console.WriteLine("He kicks the ball to " + att.Surname);
                Thread.Sleep(500);
                if (rnd.Next(100) > att.PlayingSkill)
                {
                    Console.WriteLine("He Missed!");
                    return false;
                }
                else if (oneVOne(att, gk))
                {
                    Console.WriteLine("GOAL!");
                    Thread.Sleep(200);
                    return true;
                }
                else
                {
                    Console.WriteLine(gk.Surname + " got his hands to it.");
                    Thread.Sleep(200);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Great interception from " + def.Surname);
                return false;
            }
        }

        public bool contains(Team team)
            /** Determines whether a match contains a given team */
        {
            if (team1 == team || team2 == team)
            {
                return true;
            }
            return false;
        }

        public void outcome()
        {
            team1.GoalsFor += team1Score;
            team2.GoalsAgainst += team1Score;
            team2.GoalsFor += team2Score;
            team1.GoalsAgainst += team2Score;
            if (team1Score > team2Score)
            {
                team1.Points += 3;
                team1.takePlayer(team2);
            }
            else if (team2Score > team1Score)
            {
                team2.Points += 3;
                team2.takePlayer(team1);
            }
            else
            {
                team1.Points++;
                team2.Points++;
            }
            if (!(team1.GetType() == typeof(UserTeam)))
            {
                team1.autoProtect();
            }
            if (!(team2.GetType() == typeof(UserTeam)))
            {
                team2.autoProtect();
            }
        }
    }
}
