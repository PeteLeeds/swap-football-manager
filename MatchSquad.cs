using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;
using System.Text;
using System.Text.RegularExpressions;

namespace FootballTeamCSharp
{
    class MatchSquad
    {
        Player gk;
        List<Player> defs;
        List<Player> mids;
        List<Player> fws;
        Random rnd = new Random();
        public MatchSquad()
        {
            this.gk = null;
            this.defs = new List<Player>();
            this.mids = new List<Player>();
            this.fws = new List<Player>();
        }

        public void clear()
        {
            gk = null;
            defs.Clear();
            mids.Clear();
            fws.Clear();
        }

        public Player random(string pos)
        {
            switch (pos)
            {
                case "DEF":
                    return defs[rnd.Next(defs.Count)];
                case "MID":
                    return mids[rnd.Next(mids.Count)];
                case "FW":
                    return fws[rnd.Next(fws.Count)];
                default:
                    return null;
            }
        }

        public Player Gk { 
            get { return this.gk; }
            set { this.gk = value; }
        }

        public List<Player> Defs
        {
            get { return this.defs; }
            set { this.defs = value; }
        }

        public List<Player> Mids
        {
            get { return this.mids; }
            set { this.mids = value; }
        }

        public List<Player> Fws
        {
            get { return this.fws; }
            set { this.fws = value; }
        }
    }
}
