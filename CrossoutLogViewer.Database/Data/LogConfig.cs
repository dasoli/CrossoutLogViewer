using System;

namespace CrossoutLogView.Database.Data
{
    public struct LogConfig : IEquatable<LogConfig>
    {
        public string Path;
        public long DateTime;
        public long Position;
        public long FileSize;

        public LogConfig(string path, long dateTime, long position, long fileSize)
        {
            Path = path;
            DateTime = dateTime;
            Position = position;
            FileSize = fileSize;
        }

        public override bool Equals(object obj)
        {
            return obj is LogConfig state && Equals(state);
        }

        public bool Equals(LogConfig other)
        {
            return Path == other.Path && DateTime == other.DateTime && Position == other.Position &&
                   FileSize == other.FileSize;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Path, DateTime, Position, FileSize);
        }

        public static bool operator ==(LogConfig left, LogConfig right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LogConfig left, LogConfig right)
        {
            return !(left == right);
        }
    }
}