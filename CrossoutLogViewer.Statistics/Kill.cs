using System;
using System.Collections.Generic;

namespace CrossoutLogView.Statistics
{
    public class Kill : IStatisticData, IEquatable<Kill>
    {
        public List<Assist> Assists;
        public string Killer;
        public double Time;
        public string Victim;

        public Kill()
        {
            Time = default;
            Killer = Victim = string.Empty;
        }

        public Kill(double time, string killer, string victim, List<Assist> assists)
        {
            Time = time;
            Killer = killer;
            Victim = victim;
            Assists = assists;
        }

        public bool Equals(Kill other)
        {
            return Time == other.Time && Killer == other.Killer && Victim == other.Victim;
        }

        public override bool Equals(object obj)
        {
            return obj is Kill kill && Equals(kill);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Time, Killer, Victim);
        }

        public static bool operator ==(Kill left, Kill right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Kill left, Kill right)
        {
            return !(left == right);
        }

        public static bool IsValidKill(Player killer, Player victim)
        {
            return killer != null && victim != null && !killer.IsBot;
        }

        public override string ToString()
        {
            return string.Concat(nameof(Kill), " ", Time, " ", Killer, " ", Victim, " ", Assists?.Count);
        }
    }
}