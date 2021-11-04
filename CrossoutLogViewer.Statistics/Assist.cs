using CrossoutLogView.Log;

namespace CrossoutLogView.Statistics
{
    public class Assist : IStatisticData
    {
        public string Assistant;
        public DamageFlag DamageFlags;
        public double Elapsed;
        public double TotalDamage;
        public Weapon Weapon;

        public Assist()
        {
            Assistant = string.Empty;
            Weapon = default;
            Elapsed = TotalDamage = 0.0;
            DamageFlags = DamageFlag.None;
        }

        public Assist(string assistant, Weapon weapon, double elapsed, double totalDamage, DamageFlag damageFlags)
        {
            Assistant = assistant;
            Weapon = weapon;
            Elapsed = elapsed;
            TotalDamage = totalDamage;
            DamageFlags = damageFlags;
        }

        public bool IsCriticalDamage => (DamageFlags & DamageFlag.HUD_IMPORTANT) == DamageFlag.HUD_IMPORTANT;

        public override string ToString()
        {
            return string.Concat(nameof(Assist), " ", Assistant, " ", TotalDamage, " ", DamageFlags);
        }
    }
}