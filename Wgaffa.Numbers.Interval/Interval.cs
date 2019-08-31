using System;

namespace Wgaffa.Numbers
{
    public class Interval<T> : IEquatable<Interval<T>> where T : IComparable<T>
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
            EndPoint<T> max(EndPoint<T> x, EndPoint<T> y) => x.CompareTo(y) > 0 ? x : y;
            EndPoint<T> min(EndPoint<T> x, EndPoint<T> y) => x.CompareTo(y) < 0 ? x : y;

            return new Interval<T>(max(LowerBound.Lower, other.LowerBound.Lower), min(UpperBound.Upper, other.UpperBound.Upper));
        }

        public override string ToString()
        {
            return $"{(LowerBound.Inclusive ? '[' : '(')}{LowerBound}, {UpperBound}{(UpperBound.Inclusive ? ']' : ')')}";
        }

        public bool Equals(Interval<T> other)
        {
            if (other == null)
                return false;

            return LowerBound.Equals(other.LowerBound) &&
                UpperBound.Equals(other.UpperBound);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            return Equals(obj as Interval<T>);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 17;
                hashCode = hashCode * 223 + LowerBound.GetHashCode();
                hashCode = hashCode * 223 + UpperBound.GetHashCode();

                return hashCode;
            }
        }
    }
}
