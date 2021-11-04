using System.ComponentModel.DataAnnotations;

namespace CrossoutLogView.GUI.Helpers
{
    public enum DisplayMode
    {
        [Display(Name = "Avg. Game")] GameAvg,
        [Display(Name = "Avg. Round")] RoundAvg,
        [Display(Name = "Total")] Total
    }
}