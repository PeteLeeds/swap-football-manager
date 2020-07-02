using System;
using System.Collections.Generic;
using System.Text;

namespace FootballTeamCSharp
{
    class Game
    {
        int week;
        List<League> leagues;
        UserTeam userTeam;

        public Game(int week, List<League> leagues) {
            this.week = week;
            this.leagues = leagues;
            userTeam = (UserTeam) leagues[2].Board[0];
            userTeam.IsUserTeam = true;
        }

        public List<League> Leagues
        {
            get { return this.leagues; }
            set { this.leagues = value; }
        }

        public int Week
        {
            get { return this.week; }
            set { this.week = value; }
        }

        public UserTeam UserTeam
        {
            get { return this.userTeam; }
            set { this.userTeam = value; }
        }

        public void next()
        {
            if (Week >= (UserTeam.League.Board.Count - 1) * 2)
            {
                week = 0;
                foreach (League league in leagues)
                {
                    league.promoteRelegate(userTeam);
                }
                foreach (League league in leagues)
                {
                    league.reset();
                }
                Console.WriteLine("The season is complete. The new season starts now!");
                Console.ReadLine();
            }
            else
            {
                foreach (League league in leagues)
                {
                    league.next(week, userTeam);
                }
                week++;
            }
        }
    }
}
