using System;
using CrossoutLogView.Common;

namespace CrossoutLogView.Log
{
    public class Damage : ILogEntry
    {
        public string Attacker;
        public double DamageAmmount;
        public DamageFlag DamageFlags;
        public string Victim;
        public string Weapon;

        public Damage(long timeStamp, string victim, string attacker, string weapon, double damageAmmount,
            DamageFlag damageFlags)
        {
            TimeStamp = timeStamp;
            Victim = victim;
            Attacker = attacker;
            Weapon = weapon;
            DamageAmmount = damageAmmount;
            DamageFlags = damageFlags;
        }

        public Damage()
        {
        }

        public long TimeStamp { get; set; }

        public static bool TryParse(in ReadOnlySpan<char> logLine, in DateTime logDate, out Damage deserialized)
        {
            deserialized = default;
            if (logLine.Length < 30) return false;
            var parser = new Tokenizer();
            if (!parser.First(logLine, "Damage. Victim: ")) return false;
            if (!parser.MoveNext(logLine, ", attacker: ")) return false;
            var victim = parser.CurrentString;
            if (!parser.MoveNext(logLine, ", weapon '")) return false;
            var attacker = parser.CurrentString;
            if (!parser.MoveNext(logLine, "', damage: ")) return false;
            var weapon = parser.CurrentString;
            if (!parser.MoveNext(logLine, " ")) return false;
            var damage = parser.CurrentSingle;
            parser.End(logLine);
            var damageFlags = DamageFlagsUtility.FromString(parser.CurrentString);
            var timeStamp = TimeConverter.FromString(logLine, logDate);
            deserialized = new Damage(timeStamp, victim, attacker, weapon, damage, damageFlags);
            return true;
        }

        public bool IsCriticalDamage()
        {
            return (DamageFlags & DamageFlag.HUD_IMPORTANT) == DamageFlag.HUD_IMPORTANT;
        }
    }
}