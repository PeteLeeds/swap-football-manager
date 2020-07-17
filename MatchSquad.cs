using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design.Serialization;
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
        List<Player> substitutes;
        List<Player> subIns;

        //Not used yet - for better Match Engine
        List<Player> inDefence;
        List<Player> inMidfield;
        List<Player> inAttack;

        Random rnd = new Random();
        public MatchSquad()
        {
            this.gk = null;
            this.defs = new List<Player>();
            this.mids = new List<Player>();
            this.fws = new List<Player>();
            this.substitutes = new List<Player>();
        }

        public List<Player> returnOutfieldPosition(string pos)
        {
            switch(pos)
            {
                case ("DEF"):
                    return defs;
                case ("MID"):
                    return mids;
                case ("FW"):
                    return fws;
                default:
                    return null;
            }
        }

        public void clear()
        {
            gk = null;
            defs.Clear();
            mids.Clear();
            fws.Clear();
            substitutes.Clear();
        }

        public Player random(string pos)
        {
            List<Player> list = returnOutfieldPosition(pos);
            return list[rnd.Next(list.Count)];
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

        public List<Player> Substitutes
        {
            get { return this.substitutes; }
            set { this.substitutes = value; }
        }

        public List<Player> SubGKs
        /** Returns substitutes that are GKs */
        {
            get
            {
                List<Player> subGKs = new List<Player>();
                foreach(Player sub in substitutes)
                {
                    if (sub.Pos == "GK")
                    {
                        subGKs.Add(sub);
                    }
                }
                return subGKs;
            }
        }

        public List<Player> subOutfields
        {
            get
            {
                List<Player> subOutfields = new List<Player>();
                foreach (Player sub in substitutes)
                {
                    if (sub.Pos != "GK")
                    {
                        subOutfields.Add(sub);
                    }
                }
                return subOutfields;
            }
        }
        public List<Player> PlayersOnField
        {
            get
            {
                List<Player> onField = new List<Player> { gk };
                onField.AddRange(defs);
                onField.AddRange(mids);
                onField.AddRange(fws);
                return onField;
            }
        }

        public void tirePlayers()
        {
            foreach(Player player in PlayersOnField)
            {
                player.Fitness--;
            }
        }

        public void makeSubstitution()
        {
            Console.WriteLine("Please select a player to swap out");
            for (int i = 0; i < 11; i++)
            {
                Console.WriteLine("Press " + (i + 1) + " to select " + PlayersOnField[i] + "(" + PlayersOnField[i].Fitness + "%)");
            }
            Player moveOut = PlayersOnField[Program.checkInputForValidInt(11) - 1];
            List<Player> outList = returnOutfieldPosition(moveOut.Pos);
            if (outList == null)
            {
                //Stops user from swapping out GK if no GK to swap in
                if (SubGKs.Count == 0)
                {
                    Console.WriteLine("You can't swap out this player as you have no GKs on your bench!");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    subIns = SubGKs;
                    gk = null;
                }
            }
            else
            {
                subIns = subOutfields;
                outList.Remove(moveOut);
            }

            Console.WriteLine("Please select a player to swap in");
            for (int i = 0; i < subIns.Count; i++)
            {
                Console.WriteLine("Press " + (i + 1) + " to select " + subIns[i]);
            }
            Player moveIn = subIns[Program.checkInputForValidInt(subIns.Count) - 1];
            List<Player> inList = returnOutfieldPosition(moveIn.Pos);
            substitutes.Remove(moveIn);
            if (inList == null)
            {
                gk = moveIn;
            }
            else
            {
                inList.Add(moveIn);
            }
        }
    }
}
