using System;
using CrossoutLogView.Common;

namespace CrossoutLogView.Statistics
{
    public class Stripe : IStatisticData, ICloneable, IMergable<Stripe>
    {
        public int Ammount;
        public string Name;

        public Stripe()
        {
            Name = null;
            Ammount = 0;
        }

        public Stripe(string name, int ammount)
        {
            Name = name;
            Ammount = ammount;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public Stripe Merge(Stripe other)
        {
            return new Stripe(Name, Ammount + other.Ammount);
        }

        public Stripe Clone()
        {
            return new Stripe(Name, Ammount);
        }

        public override string ToString()
        {
            return string.Concat(nameof(Stripe), " ", Name, " ", Ammount);
        }
    }
}