using System;
using System.Diagnostics.CodeAnalysis;
using CrossoutLogView.GUI.Models;
using CrossoutLogView.Statistics;

namespace CrossoutLogView.GUI.Helpers
{
    public readonly struct GameFilter : IEquatable<GameFilter>
    {
        public readonly GameMode GameModes;
        public readonly DateTime StartLimit;
        public readonly DateTime EndLimit;

        public GameFilter(GameMode gameModes = GameMode.All, DateTime startLimit = default, DateTime endLimit = default)
        {
            GameModes = gameModes;
            StartLimit = startLimit;
            EndLimit = endLimit;
        }

        public bool Filter(object obj)
        {
            if (obj is PlayerGameModel pgcm) return Filter(pgcm.Game.Game);
            if (obj is GameModel gm) return Filter(gm.Game);
            if (obj is Game game)
                return (game.Mode & GameModes) == game.Mode
                       && (StartLimit == default || game.Start >= StartLimit)
                       && (EndLimit == default || game.End <= EndLimit);
            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is GameFilter filter && Equals(filter);
        }

        public bool Equals([AllowNull] GameFilter other)
        {
            return GameModes == other.GameModes && StartLimit == other.StartLimit && EndLimit == other.EndLimit;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GameModes, StartLimit, EndLimit);
        }

        public static bool operator ==(GameFilter left, GameFilter right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GameFilter left, GameFilter right)
        {
            return !(left == right);
        }
    }
}