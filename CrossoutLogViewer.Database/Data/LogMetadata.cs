using System;
using CrossoutLogView.Common;

namespace CrossoutLogView.Database.Data
{
    public readonly struct LogMetadata : IEquatable<LogMetadata>
    {
        public readonly string Path;
        public readonly long DateTime;

        public LogMetadata(string path, long dateTime)
        {
            Path = path;
            DateTime = dateTime;
        }

        public static LogMetadata Parse(string path)
        {
            var norm = PathUtility.NormalizePath(path);
            return new LogMetadata(norm,
                PathUtility.ParseCrossoutLogDirectoryName(PathUtility.GetDirectoryName(path)).Ticks);
        }

        public override bool Equals(object obj)
        {
            return obj is LogMetadata metadata && Equals(metadata);
        }

        public bool Equals(LogMetadata other)
        {
            return Path == other.Path && DateTime == other.DateTime;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Path, DateTime);
        }

        public static bool operator ==(LogMetadata left, LogMetadata right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LogMetadata left, LogMetadata right)
        {
            return !(left == right);
        }
    }
}