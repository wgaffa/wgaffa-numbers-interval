using System;

namespace Wgaffa.Numbers
{
    public class EndPointPair<T> : IEquatable<EndPointPair<T>> where T : IComparable<T>
    {
        private readonly EndPoint<T> _lower;
        private readonly EndPoint<T> _upper;

        public EndPoint<T> Lower => _lower.Lower;
        public EndPoint<T> Upper => _upper.Upper;

        public EndPointPair(EndPoint<T> lower, EndPoint<T> upper)
        {
            _lower = lower ?? throw new ArgumentNullException(nameof(lower));
            _upper = upper ?? throw new ArgumentNullException(nameof(upper));
        }

        public bool Overlaps(EndPointPair<T> other)
        {
            EndPoint<T> max(EndPoint<T> x, EndPoint<T> y) => x.CompareTo(y) > 0 ? x : y;
            EndPoint<T> min(EndPoint<T> x, EndPoint<T> y) => x.CompareTo(y) < 0 ? x : y;

            var lowerBound = max(Lower, other.Lower);
            var upperBound = min(Upper, other.Upper);

            if (lowerBound.CompareTo(upperBound) <= 0)
                return true;

            return false;
        }

        public bool Equals(EndPointPair<T> other)
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

            return Equals(obj as EndPointPair<T>);
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

        public override string ToString()
        {
            return $"{(Lower.Inclusive ? '[' : '(')}{Lower}, {Upper}{(Upper.Inclusive ? ']' : ')')}";
        }
    }
}
