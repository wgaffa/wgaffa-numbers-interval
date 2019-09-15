﻿using System;

namespace Wgaffa.Numbers
{
    internal class EndPointPair<T> : IEquatable<EndPointPair<T>> where T : IComparable<T>
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
            var continuousLeft = (Lower.Inclusive || other.Upper.Inclusive) && Lower.CompareTo(other.Upper) == 0;
            var continuousRight = (other.Lower.Inclusive || Upper.Inclusive) && Upper.CompareTo(other.Lower) == 0;

            if (continuousRight || continuousLeft)
                return true;

            return Lower.IsInsideLowerBounds(other.Upper) && other.Lower.IsInsideLowerBounds(Upper);
        }

        public EndPointPair<T> Merge(EndPointPair<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (!Overlaps(other))
                throw new InvalidOperationException("Cannot perform a merge on non overlapping pairs");

            EndPoint<T> max(EndPoint<T> left, EndPoint<T> right) => left.CompareTo(right) > 0 ? left : right;
            EndPoint<T> min(EndPoint<T> left, EndPoint<T> right) => left.CompareTo(right) < 0 ? left : right;

            return new EndPointPair<T>(min(Lower, other.Lower), max(Upper, other.Upper));
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
