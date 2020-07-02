using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Xml.Serialization;

namespace FootballTeamCSharp
{
    class Team
    {
        string name;
        List<Player> players;
        MatchSquad squad = new MatchSquad();
        int points = 0, goalsFor = 0, goalsAgainst = 0;
        bool isUserTeam = false;
        Random rnd = new Random();
        League league;

        public Team(string name, List<Player> players, League league) {
            this.name = name;
            this.players = players;
            this.league = league;
            autoProtect();
        }

        public Team(string name, int high, int low, League league)
        {
            this.name = name;
            this.league = league;
            //Adds first 11 players
            players = new List<Player>();
            for (int i = 0; i < 11; i++)
            {
                players.Add(new Player(high, low, Program.positions[i]));
            }
            //Adds substitutes
            for (int i = 0; i < 5; i++)
            {
                players.Add(new Player(high, low, Program.positions[rnd.Next(Program.positions.Length)]));
            }
            autoProtect();
            autoSquad();
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public List<Player> Players
        {
            get { return this.players; }
            set { this.players = value; }
        }

        public void displaySquad(bool selection)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (selection)
                {
                    Console.Write("Press " + i + " to select ");
                }
                Console.WriteLine(players[i]);
                if (i == 10)
                {
                    Console.WriteLine("------------------");
                }
            }
        }

        public MatchSquad Squad
        {
            get { return this.squad; }
            set { this.squad = value; }
        }

        public int Points
        {
            get { return this.points; }
            set { this.points = value; }
        }

        public int GoalsFor
        {
            get { return this.goalsFor; }
            set { this.goalsFor = value; }
        }

        public int GoalsAgainst
        {
            get { return this.goalsAgainst; }
            set { this.goalsAgainst = value; }
        }

        public int GoalDifference
        {
            get { return this.goalsFor - goalsAgainst; }
        }

        public bool IsUserTeam
        {
            get { return this.isUserTeam; }
            set { this.isUserTeam = value; }
        }

        public League League
        {
            get { return this.league; }
            set { this.league = value; }
        }

        public float AverageSkill
        {
            get
            {
                int skill = 0;
                for(int i = 0; i < 11; i++)
                {
                    skill += players[i].Skill;
                }
                return skill / 11;
            }
        }

        public void autoProtect()
        {
            List<Player> temp = players = players.OrderByDescending(o => o.Skill).ToList();
            temp[0].Protect = true;
            for (int i = 1; i < temp.Count; i++)
            {
                temp[i].Protect = false;
            }
        }

        public void autoSquad()
            /** Automatically picks the best possible squad for a match*/
        {
            squad.clear();
            players = players.OrderByDescending(o => o.Skill).ToList();
            int confirmed = 0, index = 0;
            while (confirmed < 11 && index < players.Count)
            {
                switch (players[index].Pos)
                {
                    case "GK":
                        if (squad.Gk == null) {
                            squad.Gk = players[index];
                            confirmed++;
                        }
                        break;
                    case "DEF":
                        if (squad.Defs.Count < 4)
                        {
                            squad.Defs.Add(players[index]);
                            confirmed++;
                        }
                        break;
                    case "MID":
                        if (squad.Mids.Count < 4)
                        {
                            squad.Mids.Add(players[index]);
                            confirmed++;
                        }
                        break;
                    case "FW":
                        if (squad.Fws.Count < 2)
                        {
                            squad.Fws.Add(players[index]);
                            confirmed++;
                        }
                        break;
                }
                index++;
            }
            if (confirmed < 11)
            {
                if (squad.Gk == null)
                {
                    squad.Gk = new Player(50, 70, "GK");
                }
                while (squad.Defs.Count < 4)
                {
                    squad.Defs.Add(new Player(50, 70, "DEF"));
                }
                while (squad.Mids.Count < 4)
                {
                    squad.Mids.Add(new Player(50, 70, "MID"));
                }
                while (squad.Fws.Count < 2)
                {
                    squad.Fws.Add(new Player(50, 70, "FW"));
                }
            }
            sortTeam();
        }

        public void sortTeam()
            /** Sorts the MatchSquad above the rest of the team */
        {
            List<Player> matchSquad = new List<Player> { squad.Gk };
            matchSquad.AddRange(squad.Defs);
            matchSquad.AddRange(squad.Mids);
            matchSquad.AddRange(squad.Fws);
            List<Player> reserves = new List<Player>();
            for (int i = 0; i < players.Count; i++)
            {
                if (!(matchSquad.Contains(players[i]))) {
                    reserves.Add(players[i]);
                }
            }
            players = matchSquad;
            players.AddRange(reserves);
        }

        public void setMatchSquad()
        {
            squad.clear();
            for (int i = 0; i < 11; i++)
            {
                switch (players[i].Pos) {
                    case "GK":
                        squad.Gk = players[i];
                        break;
                    case "DEF":
                        squad.Defs.Add(players[i]);
                        break;
                    case "MID":
                        squad.Mids.Add(players[i]);
                        break;
                    case "FW":
                        squad.Fws.Add(players[i]);
                        break;
                }
            }
        }

        public virtual void takePlayer(Team otherTeam)
            /** Completes the process of the winning team swapping a player of their choice with the losing team */
        {
            List<Player> temp = otherTeam.Players.OrderByDescending(o => o.Skill).ToList();
            bool isProtected = true;
            Player takenPlayer = new Player("Temp","Temp",0,"Temp");
            while (isProtected)
            {
                isProtected = true;
                takenPlayer = temp[rnd.Next(4)];
                isProtected = takenPlayer.Protect;
            }
            Player givenPlayer = new Player("Temp", "Temp", 0, "Temp");
            temp = players.OrderBy(o => o.Skill).ToList();
            int index = 0;
            while (!(temp[index].Pos == takenPlayer.Pos)) {
                index++;
            }
            givenPlayer = temp[index];
            otherTeam.Players[otherTeam.Players.IndexOf(takenPlayer)] = givenPlayer;
            players[players.IndexOf(givenPlayer)] = takenPlayer;
            if (otherTeam.isUserTeam)
            {
                Console.WriteLine("You lost! " + name + " have swapped " + takenPlayer.Name + " for " + givenPlayer.Name);
                Console.ReadLine();
            }
        }

        public override string ToString()
        {
            return name + " | " + points + " | " + GoalDifference;
        }

        public void reset()
        {
            points = 0;
            goalsFor = 0;
            goalsAgainst = 0;
        }
    }
}
