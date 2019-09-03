using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wgaffa.Numbers
{
    public class Interval<T> : IEquatable<Interval<T>> where T : IComparable<T>
    {
        private readonly List<EndPointPair<T>> _endPoints;

        public IReadOnlyCollection<EndPointPair<T>> Bounds => _endPoints.AsReadOnly();

        public bool IsEmpty { get; } = true;

        public Interval(EndPoint<T> lower, EndPoint<T> upper)
            : this(new EndPointPair<T>[] { new EndPointPair<T>(lower, upper) })
        {
        }

        public Interval(IEnumerable<EndPointPair<T>> endPoints)
        {
            if (endPoints == null)
                throw new ArgumentNullException(nameof(endPoints));

            _endPoints = new List<EndPointPair<T>>();

            var pointsBeingMerged = new List<EndPointPair<T>>(endPoints.Where(x => x.Lower.CompareTo(x.Upper) <= 0));
            while (pointsBeingMerged.Count > 0)
            {
                var currentPoint = pointsBeingMerged[0];
                pointsBeingMerged.RemoveAt(0);

                var mergeComplete = false;
                EndPointPair<T> merged = currentPoint;
                while (!mergeComplete)
                {
                    var overlappingPoints = pointsBeingMerged.Where(x => x.Overlaps(merged));
                    mergeComplete = overlappingPoints.Count() == 0;

                    if (!mergeComplete)
                        merged = overlappingPoints.Aggregate(merged, (a, b) => a.Merge(b));
                    else
                        _endPoints.Add(merged);

                    var markedForDeletion = overlappingPoints.ToList();
                    foreach (var item in markedForDeletion)
                    {
                        pointsBeingMerged.Remove(item);
                    }
                }
            }

            foreach (var pair in _endPoints)
            {
                if (pair.Lower.CompareTo(pair.Upper) > 0)
                    continue;

                IsEmpty = false;
                break;
            }
        }

        public virtual bool Contains(T value)
        {
            var foundValue = false;
            foreach (var pair in _endPoints)
            {
                if (pair.Lower.IsInsideLowerBounds(value) && pair.Upper.IsInsideUpperBounds(value))
                {
                    foundValue = true;
                    break;
                }
            }

            return foundValue;
        }

        public Interval<T> Intersect(Interval<T> other)
        {
            EndPoint<T> max(EndPoint<T> x, EndPoint<T> y) => x.CompareTo(y) > 0 ? x : y;
            EndPoint<T> min(EndPoint<T> x, EndPoint<T> y) => x.CompareTo(y) < 0 ? x : y;

            var pair = Bounds.Single();
            var otherPair = other.Bounds.Single();

            return new Interval<T>(max(pair.Lower, otherPair.Lower), min(pair.Upper, otherPair.Upper));
        }

        public Interval<T> Union(IEnumerable<Interval<T>> other)
        {
            var endPointPairs = new List<EndPointPair<T>>(Bounds);
            endPointPairs.AddRange(other.SelectMany(x => x.Bounds).ToList());

            return new Interval<T>(endPointPairs);
        }

        public override string ToString()
        {
            return string.Join(", ", _endPoints);
        }

        public bool Equals(Interval<T> other)
        {
            if (other == null)
                return false;

            return _endPoints.SequenceEqual(other._endPoints);
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
                foreach (var pair in _endPoints)
                {
                    hashCode = hashCode * 223 + pair.Lower.GetHashCode();
                    hashCode = hashCode * 223 + pair.Upper.GetHashCode();
                }

                return hashCode;
            }
        }
    }
}
