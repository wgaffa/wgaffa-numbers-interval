using System;
using System.Collections.Generic;
using System.Linq;

namespace Wgaffa.Numbers
{
    public class Interval<T> : IInterval<T>, IEquatable<Interval<T>> where T : IComparable<T>
    {
        public bool IsEmpty
        {
            get
            {
                if (Lower.CompareTo(Upper) == 0)
                    return !(Lower.Inclusive && Upper.Inclusive);

                return Lower.IsInsideUpperBounds(Upper) || Upper.IsInsideLowerBounds(Lower);
            }
        }

        public bool Degenerate => Lower.Value.CompareTo(Upper.Value) == 0 && Lower.Inclusive && Upper.Inclusive && !IsEmpty;

        public EndPoint<T> Lower { get; }
        public EndPoint<T> Upper { get; }

        public Interval(EndPoint<T> lower, EndPoint<T> upper)
        {
            Lower = lower.Lower ?? throw new ArgumentNullException(nameof(lower));
            Upper = upper.Upper ?? throw new ArgumentNullException(nameof(upper));
        }

        public Interval(EndPointPair<T> endPoint)
        {
            Lower = endPoint.Lower;
            Upper = endPoint.Upper;
        }

        public virtual bool Contains(T value)
        {
            return Lower.IsInsideLowerBounds(value) && Upper.IsInsideUpperBounds(value);
        }

        public Interval<T> Intersect(Interval<T> other)
        {
            EndPoint<T> max(EndPoint<T> x, EndPoint<T> y) => x.CompareTo(y) > 0 ? x : y;
            EndPoint<T> min(EndPoint<T> x, EndPoint<T> y) => x.CompareTo(y) < 0 ? x : y;

            return new Interval<T>(max(Lower, other.Lower), min(Upper, other.Upper));
        }

        public IInterval<T> Union(IEnumerable<Interval<T>> other)
        {
            return new UnionInterval<T>(other.Prepend(this));
        }

        public override string ToString()
        {
            return $"{(Lower.Inclusive ? '[' : '(')}{Lower}, {Upper}{(Upper.Inclusive ? ']' : ')')}";
        }

        public bool Equals(Interval<T> other)
        {
            if (other == null)
                return false;

            return Lower.Equals(other.Lower) && Upper.Equals(other.Upper);
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
                hashCode = hashCode * 223 + Lower.GetHashCode();
                hashCode = hashCode * 223 + Upper.GetHashCode();

                return hashCode;
            }
        }
    }
}
