using System;
using System.Collections.Generic;
using CrossoutLogView.Common;
using CrossoutLogView.Log;
using static CrossoutLogView.Common.Strings;

namespace CrossoutLogView.Statistics
{
    public class Weapon : WeaponBase, IStatisticData, ICloneable, IMergable<Weapon>
    {
        public int Uses;

        public Weapon()
        {
        }

        public Weapon(string name, double criticalDamage, double armorDamage)
        {
            Name = name;
            ArmorDamage = armorDamage;
            CriticalDamage = criticalDamage;
            Uses = 1;
        }

        public Weapon(string name, double criticalDamage, double armorDamage, int uses) : this(name, criticalDamage,
            armorDamage)
        {
            Uses = uses;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public Weapon Merge(Weapon other)
        {
            return new Weapon(Name,
                CriticalDamage + other.CriticalDamage,
                ArmorDamage + other.ArmorDamage,
                Uses + other.Uses);
        }

        public static List<Weapon> ParseWeapons(IEnumerable<Damage> damageLog)
        {
            var weapons = new List<Weapon>();
            foreach (var dmg in damageLog)
            {
                var weaponName = TrimName(dmg.Weapon).ToString();
                var weapon = weapons.Find(x => NameEquals(x.Name, weaponName));

                double criticalDamage, armorDamage;
                if (dmg.IsCriticalDamage())
                {
                    criticalDamage = dmg.DamageAmmount;
                    armorDamage = 0.0;
                }
                else
                {
                    criticalDamage = 0.0;
                    armorDamage = dmg.DamageAmmount;
                }

                if (weapon == null)
                {
                    weapons.Add(new Weapon(weaponName, criticalDamage, armorDamage));
                }
                else
                {
                    weapon.CriticalDamage += criticalDamage;
                    weapon.ArmorDamage += armorDamage;
                }
            }

            return weapons;
        }

        public Weapon Clone()
        {
            return new Weapon(Name, CriticalDamage, ArmorDamage);
        }

        public override string ToString()
        {
            return string.Concat(nameof(Weapon), " ", Name, " ", ArmorDamage + CriticalDamage, " ", Uses);
        }
    }
}