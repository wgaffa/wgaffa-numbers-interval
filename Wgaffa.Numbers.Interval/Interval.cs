using System;

namespace Wgaffa.Numbers
{
    public class Interval<T> where T : IComparable<T>
    {
        public EndPoint<T> LowerBound { get; }
        public EndPoint<T> UpperBound { get; }
        public bool IsEmpty => LowerBound.Lower.CompareTo(UpperBound.Upper) > 0;

        public Interval(EndPoint<T> lower, EndPoint<T> upper)
        {
            LowerBound = lower ?? throw new ArgumentNullException(nameof(lower));
            UpperBound = upper ?? throw new ArgumentNullException(nameof(upper));
        }

        public virtual bool Contains(T value)
        {
            return LowerBound.Lower.IsInsideLowerBounds(value) && UpperBound.Upper.IsInsideUpperBounds(value);
        }

        public Interval<T> Intersect(Interval<T> other)
        {
            T max(T x, T y) => x.CompareTo(y) > 0 ? x : y;
            T min(T x, T y) => x.CompareTo(y) < 0 ? x : y;

            return new Interval<T>(max(LowerBound.Value, other.LowerBound.Value), min(UpperBound.Value, other.UpperBound.Value));
        }

        public override string ToString()
        {
            return $"{(LowerBound.Inclusive ? '[' : '(')}{LowerBound}, {UpperBound}{(UpperBound.Inclusive ? ']' : ')')}";
        }
    }
}
