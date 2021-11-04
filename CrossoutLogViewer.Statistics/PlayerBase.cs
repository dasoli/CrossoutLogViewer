using System;
using System.Collections.Generic;

namespace CrossoutLogView.Statistics
{
    public abstract class PlayerBase
    {
        public double ArmorDamageDealt;
        public double ArmorDamageTaken;
        public int Assists;
        public double CriticalDamageDealt;
        public double CriticalDamageTaken;
        public int Deaths;
        public int Kills;
        public string Name;
        public double Score;
        public List<Stripe> Stripes;
        public int UserID;
        public List<Weapon> Weapons;

        protected PlayerBase()
        {
            Name = string.Empty;
            UserID = -1;
            Score = 0;
            ArmorDamageDealt = 0;
            CriticalDamageDealt = 0;
            CriticalDamageTaken = 0;
            ArmorDamageTaken = 0;
            Kills = 0;
            Assists = 0;
            Deaths = 0;
            Weapons = new List<Weapon>();
            Stripes = new List<Stripe>();
        }

        public static void Merge(PlayerBase master, PlayerBase other)
        {
            //increment stats
            master.Score += other.Score;
            master.ArmorDamageDealt += other.ArmorDamageDealt;
            master.CriticalDamageDealt += other.CriticalDamageDealt;
            master.ArmorDamageTaken += other.ArmorDamageTaken;
            master.CriticalDamageTaken += other.CriticalDamageTaken;
            master.Kills += other.Kills;
            master.Assists += other.Assists;
            master.Deaths += other.Deaths;
            //merge weapons
            foreach (var weapon in other.Weapons)
            {
                var myWeapon = master.Weapons.Find(x => x.Name.Equals(weapon.Name, StringComparison.InvariantCulture));
                if (myWeapon == null)
                {
                    myWeapon = weapon.Clone();
                    master.Weapons.Add(myWeapon);
                }
                else
                {
                    myWeapon.ArmorDamage += weapon.ArmorDamage;
                    myWeapon.CriticalDamage += weapon.CriticalDamage;
                }
            }

            //merge stripes
            foreach (var stripe in other.Stripes)
            {
                var myStripe = master.Stripes.Find(x => x.Name.Equals(stripe.Name, StringComparison.InvariantCulture));
                if (myStripe == null)
                {
                    myStripe = stripe.Clone();
                    master.Stripes.Add(myStripe);
                }
                else
                {
                    myStripe.Ammount += stripe.Ammount;
                }
            }
        }
    }
}