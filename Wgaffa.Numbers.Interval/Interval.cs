using System;
using System.Collections.Generic;
using System.Linq;

namespace Wgaffa.Numbers
{
    /// <summary>
    /// Represents an interval between two <see cref="EndPoint{T}"/>s.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    public class Interval<T> : IEquatable<Interval<T>> where T : IComparable<T>
    {
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
        public bool IsDegenerate => Lower.Value.CompareTo(Upper.Value) == 0 && Lower.Inclusive && Upper.Inclusive && !IsEmpty;

        /// <summary>
        /// Gets the lower/left bound of the interval.
        /// </summary>
        public EndPoint<T> Lower { get; }

        /// <summary>
        /// Gets the upper/right bound of the interval.
        /// </summary>
        public EndPoint<T> Upper { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Interval{T}"/> class.
        /// </summary>
        /// <param name="lower">The value of the lower/left boundary.</param>
        /// <param name="upper">The value of the upper/right boundary.</param>
        public Interval(EndPoint<T> lower, EndPoint<T> upper)
        {
            if (lower == null)
                throw new ArgumentNullException(nameof(lower));

            if (upper == null)
                throw new ArgumentNullException(nameof(upper));

            Lower = lower.Lower;
            Upper = upper.Upper;
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
        public IEnumerable<Interval<T>> Union(IEnumerable<Interval<T>> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return Merge(other.Prepend(this));
        }

        private IEnumerable<Interval<T>> Merge(IEnumerable<Interval<T>> intervals)
        {
            if (intervals == null)
                throw new ArgumentNullException(nameof(intervals));

            var mergedIntervals = new List<Interval<T>>();

            var endPoints = intervals.Select(x => new Interval<T>(x.Lower, x.Upper));
            var nonEmptyPoints = endPoints.Where(x => x.Lower.IsInsideLowerBounds(x.Upper) && x.Upper.IsInsideUpperBounds(x.Lower));
            var pointsBeingMerged = new List<Interval<T>>(nonEmptyPoints);

            while (pointsBeingMerged.Count > 0)
            {
                var currentPoint = pointsBeingMerged[0];
                pointsBeingMerged.RemoveAt(0);

                var mergeComplete = false;
                var mergedPoint = currentPoint;
                while (!mergeComplete)
                {
                    var overlappingPoints = pointsBeingMerged.Where(x => x.Overlaps(mergedPoint));
                    mergeComplete = overlappingPoints.Count() == 0;

                    if (!mergeComplete)
                        mergedPoint = overlappingPoints.Aggregate(mergedPoint, (a, b) => a.Merge(b));
                    else
                        mergedIntervals.Add(new Interval<T>(mergedPoint.Lower, mergedPoint.Upper));

                    var markedForDeletion = overlappingPoints.ToList();
                    foreach (var deleteItem in markedForDeletion)
                    {
                        pointsBeingMerged.Remove(deleteItem);
                    }
                }
            }

            return mergedIntervals;
        }

        public bool Overlaps(Interval<T> other)
        {
            var continuousLeft = (Lower.Inclusive || other.Upper.Inclusive) && Lower.CompareTo(other.Upper) == 0;
            var continuousRight = (other.Lower.Inclusive || Upper.Inclusive) && Upper.CompareTo(other.Lower) == 0;

            if (continuousRight || continuousLeft)
                return true;

            return Lower.IsInsideLowerBounds(other.Upper) && other.Lower.IsInsideLowerBounds(Upper);
        }

        public Interval<T> Merge(Interval<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (!Overlaps(other))
                throw new InvalidOperationException("Cannot perform a merge on non overlapping pairs");

            EndPoint<T> max(EndPoint<T> left, EndPoint<T> right) => left.CompareTo(right) > 0 ? left : right;
            EndPoint<T> min(EndPoint<T> left, EndPoint<T> right) => left.CompareTo(right) < 0 ? left : right;

            return new Interval<T>(min(Lower, other.Lower), max(Upper, other.Upper));
        }

        #region Overloaded operators
        public static bool operator ==(Interval<T> left, Interval<T> right) => object.Equals(left, right);
        public static bool operator !=(Interval<T> left, Interval<T> right) => !object.Equals(left, right);
        #endregion

        public override string ToString()
        {
            if (IsEmpty)
                return "\u2205";

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
