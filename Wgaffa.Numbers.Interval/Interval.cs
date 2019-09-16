using System;
using System.Collections.Generic;
using System.Linq;

namespace Wgaffa.Numbers
{
    /// <summary>
    /// Represents an interval between two <see cref="EndPoint{T}"/>s.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    public class Interval<T> : IInterval<T>, IEquatable<Interval<T>> where T : IComparable<T>
    {
        private readonly EndPointPair<T> _endPoints;

        /// <summary>
        /// Gets wether the interval is considered empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (Lower.CompareTo(Upper) == 0)
                    return !(Lower.Inclusive && Upper.Inclusive);

                return Lower.IsInsideUpperBounds(Upper) || Upper.IsInsideLowerBounds(Lower);
            }
        }

        /// <summary>
        /// Gets wether the interval holds a single value.
        /// </summary>
        public bool Degenerate => Lower.Value.CompareTo(Upper.Value) == 0 && Lower.Inclusive && Upper.Inclusive && !IsEmpty;

        /// <summary>
        /// Gets the lower/left bound of the interval.
        /// </summary>
        public EndPoint<T> Lower => _endPoints.Lower;

        /// <summary>
        /// Gets the upper/right bound of the interval.
        /// </summary>
        public EndPoint<T> Upper => _endPoints.Upper;

        /// <summary>
        /// Initializes a new instance of the <see cref="Interval{T}"/> class.
        /// </summary>
        /// <param name="lower">The value of the lower/left boundary.</param>
        /// <param name="upper">The value of the upper/right boundary.</param>
        public Interval(EndPoint<T> lower, EndPoint<T> upper)
        {
            _endPoints = new EndPointPair<T>(lower, upper);
        }

        internal Interval(EndPointPair<T> endPointPair)
        {
            _endPoints = endPointPair ?? throw new ArgumentNullException(nameof(endPointPair));
        }

        public virtual bool Contains(T value)
        {
            return Lower.IsInsideLowerBounds(value) && Upper.IsInsideUpperBounds(value);
        }

        /// <summary>
        /// Intersect two intervals.
        /// </summary>
        /// <param name="other">The other interval to intersect with.</param>
        /// <returns>A new interval based on the intersection of the two intervals.</returns>
        public Interval<T> Intersect(Interval<T> other)
        {
            EndPoint<T> max(EndPoint<T> x, EndPoint<T> y) => x.CompareTo(y) > 0 ? x : y;
            EndPoint<T> min(EndPoint<T> x, EndPoint<T> y) => x.CompareTo(y) < 0 ? x : y;

            return new Interval<T>(max(Lower, other.Lower), min(Upper, other.Upper));
        }

        /// <summary>
        /// Union two intervals together.
        /// </summary>
        /// <param name="other">The other interval to union with.</param>
        /// <returns>A new interval type that holds contains each interval.</returns>
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
