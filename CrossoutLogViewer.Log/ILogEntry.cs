using System;
using System.Linq;

namespace CrossoutLogView.Log
{
    public interface ILogEntry
    {
        /// <summary>
        ///     All types that implement this interface, excluding itslef.
        /// </summary>
        public static readonly Type[] Implementations = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(ILogEntry).IsAssignableFrom(p) && p.Name != nameof(ILogEntry)).ToArray();

        public long TimeStamp { get; set; }
    }
}