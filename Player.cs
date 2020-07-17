using System;
using System.Collections.Generic;
using System.Text;

namespace FootballTeamCSharp
{
    class Player
    {
        string firstName, surname, pos;
        int skill, fitness;
        bool protect = false;
        Random rnd = new Random();

        public Player(string firstName, string surname, int skill, string pos) {
            this.firstName = firstName;
            this.surname = surname;
            this.skill = skill;
            this.pos = pos;
        }

        public Player(int low, int high, string pos)
        {
            this.firstName = Program.firstNames[rnd.Next(Program.firstNames.Length)];
            this.surname = Program.lastNames[rnd.Next(Program.lastNames.Length)];
            this.skill = rnd.Next(high - low) + low;
            this.pos = pos;
        }

        public string FirstName
        {
            get { return this.firstName; }
            set { this.firstName = value; }
        }

        public string Surname
        {
            get { return this.surname; }
            set { this.surname = value; }
        }

        public string Name
        {
            get { return this.firstName + " " + this.surname; }
        }

        public string Pos
        {
            get { return this.pos; }
            set { this.pos = value; }
        }

        public int Skill
        {
            get { return this.skill; }
            set { this.skill = value; }
        }

        public bool Protect
        {
            get { return this.protect; }
            set { this.protect = value; }
        }

        public int Fitness
        {
            get { return this.fitness; }
            set { if ((pos != "GK" || rnd.Next(2) == 1 || value == 100)) {
                    this.fitness = value;
                }
            }
        }

        public int PlayingSkill
        {
            get { return (this.Skill * this.Fitness / 100); }
        }

        public override string ToString()
        {
            string extra = "";
            if (protect) {
                extra = " - Protected Player";
            }
            return (pos + " " + firstName + " " + surname + " " + skill + extra);
        }

    }
}
