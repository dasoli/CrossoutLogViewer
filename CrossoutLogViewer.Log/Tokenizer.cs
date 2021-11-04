using System;
using System.Globalization;

namespace CrossoutLogView.Log
{
    public class Tokenizer
    {
        public Tokenizer()
        {
            CurrentString = string.Empty;
        }

        public string CurrentString { get; private set; }
        public byte CurrentByte => byte.Parse(CurrentString, CultureInfo.InvariantCulture.NumberFormat);
        public short CurrentInt16 => short.Parse(CurrentString, CultureInfo.InvariantCulture.NumberFormat);
        public int CurrentInt32 => int.Parse(CurrentString, CultureInfo.InvariantCulture.NumberFormat);
        public long CurrentInt64 => long.Parse(CurrentString, CultureInfo.InvariantCulture.NumberFormat);
        public double CurrentSingle => float.Parse(CurrentString, CultureInfo.InvariantCulture.NumberFormat);

        public int CurrentHex =>
            int.Parse(CurrentString, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);

        public int Position { get; private set; }

        public bool First(ReadOnlySpan<char> source, ReadOnlySpan<char> terminationPattern)
        {
            var index = source.IndexOf(terminationPattern, StringComparison.InvariantCultureIgnoreCase);
            if (index == -1) return false;
            Position = index + terminationPattern.Length;
            CurrentString = string.Empty;
            return true;
        }

        public void End(ReadOnlySpan<char> source)
        {
            CurrentString = source.Slice(Position).Trim().ToString();
            Position = source.Length;
        }

        public bool MoveNext(ReadOnlySpan<char> source, ReadOnlySpan<char> terminationPattern)
        {
            var target = source.Slice(Position);
            var index = target.IndexOf(terminationPattern, StringComparison.InvariantCultureIgnoreCase);
            if (index == -1) return false;
            CurrentString = target.Slice(0, index).Trim().ToString();
            Position += index + terminationPattern.Length;
            return true;
        }

        public void Reset()
        {
            CurrentString = string.Empty;
            Position = 0;
        }
    }
}