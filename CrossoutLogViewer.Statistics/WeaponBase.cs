namespace CrossoutLogView.Statistics
{
    public abstract class WeaponBase
    {
        public double ArmorDamage;
        public double CriticalDamage;
        public string Name;

        protected WeaponBase()
        {
            Name = string.Empty;
        }
    }
}