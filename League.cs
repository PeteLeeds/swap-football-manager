using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FootballTeamCSharp
{
    class League
    {
        List<Team> board;
        string name;
        List<List<Match>> schedule = new List<List<Match>>();
        League upperLeague = null, lowerLeague = null;

        public League(List<Team> board, string name) {
            this.board = board;
            this.name = name;
            setSchedule();
        }

        public League(string name, int noTeams, int high, int low)
        {
            this.name = name;
            this.board = new List<Team>();
            for (int i = 0; i < noTeams; i++)
            {
                board.Add(new Team(name + " Team " + i, high, low, this));
            }
            setSchedule();
        }

        public League(string name, int noTeams, int high, int low, string userTeam)
        {
            this.name = name;
            this.board = new List<Team>();
            board.Add(new UserTeam(userTeam, high, low, this));
            for (int i = 0; i < noTeams - 1; i++)
            {
                board.Add(new Team(name + " Team " + i, high, low, this));
            }
            setSchedule();
        }

        public void setSchedule()
        {
            schedule.Clear();
            for (int i = 0; i < (board.Count - 1) * 2; i++)
            {
                schedule.Add(new List<Match>());
                for (int j = 0; j < board.Count / 2; j++) {
                    schedule[i].Add(new Match(board[j], board[(board.Count / 2) + j]));
                }
                Team temp = board[board.Count / 2 - 1];
                Team temp2 = board[board.Count / 2];
                for (int j = (board.Count - 1) / 2; j > 1; j--) {
                    if (j < board.Count / 2)
                    {
                        board[j] = board[j - 1];
                    }
                }
                for (int j = board.Count / 2; j < board.Count - 1; j++ )
                {
                    board[j] = board[j + 1];
                }
                board[1] = temp2;
                board[board.Count - 1] = temp;
            }
        }

        public List<Team> Board
        {
            get { return this.board; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public List<List<Match>> Schedule
        {
            get { return this.schedule; }
            set { this.schedule = value; }
        }

        public League UpperLeague
        {
            get { return this.upperLeague; }
            set { this.upperLeague = value; }
        }

        public League LowerLeague
        {
            get { return this.lowerLeague; }
            set { this.lowerLeague = value; }
        }

        public void update()
        {
            board = board.OrderByDescending(o => o.Points).ThenByDescending(o => o.GoalDifference).ToList();
        }

        public override string ToString()
        {
            string league = name + "\n";
            for (int i = 0; i < board.Count; i++)
            {
                if (i == 3 && !(upperLeague == null))
                {
                    league = league + "----------------------\n";
                }
                if (i == 17 && !(lowerLeague == null))
                {
                    league = league + "----------------------\n";
                }
                league = league + (i+1) + " " + board[i] + "\n";
            }
            return league;
        }

        public void next(int week, Team userTeam)
        {
            foreach (Match match in Schedule[week])
            {
                if (match.contains(userTeam))
                {
                    match.play();
                }
                else
                {
                    match.sim();
                }
            }
            update();
        }

        public void promoteRelegate(UserTeam userTeam)
        {
            List<Team> promotedTeams = board.GetRange(0, 3);
            List<Team> relegatedTeams = board.GetRange(17, 3);
            if (!(upperLeague == null))
            {
                upperLeague.Board.AddRange(promotedTeams);
                board.RemoveAll(promotedTeams.Contains);
                if (promotedTeams.Contains(userTeam))
                {
                    userTeam.League = upperLeague;
                }
            }
            if (!(lowerLeague == null))
            {
                lowerLeague.Board.AddRange(relegatedTeams);
                board.RemoveAll(relegatedTeams.Contains);
                if (relegatedTeams.Contains(userTeam))
                {
                    userTeam.League = lowerLeague;
                }
            }
        }

        public void reset()
        {
            setSchedule();
            foreach(Team team in board)
            {
                team.reset();
            }
        }
    }
}
