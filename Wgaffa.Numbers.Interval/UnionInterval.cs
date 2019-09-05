using System;
using System.Collections.Generic;
using System.Linq;

namespace Wgaffa.Numbers
{
    public class UnionInterval<T> : IInterval<T> where T : IComparable<T>
    {
        private readonly List<Interval<T>> _intervals = new List<Interval<T>>();

        public IReadOnlyCollection<Interval<T>> Intervals => _intervals.AsReadOnly();

        public UnionInterval(IEnumerable<Interval<T>> intervals)
        {
            if (intervals == null)
                throw new ArgumentNullException(nameof(intervals));

            var endPoints = intervals.Select(x => new EndPointPair<T>(x.Lower, x.Upper));
            var nonEmptyPoints = endPoints.Where(x => x.Lower.IsInsideLowerBounds(x.Upper) && x.Upper.IsInsideUpperBounds(x.Lower));
            var pointsBeingMerged = new List<EndPointPair<T>>(nonEmptyPoints);

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
                        _intervals.Add(new Interval<T>(mergedPoint));

                    var markedForDeletion = overlappingPoints.ToList();
                    foreach (var deleteItem in markedForDeletion)
                    {
                        pointsBeingMerged.Remove(deleteItem);
                    }
                }
            }
        }

        public bool Contains(T item)
        {
            foreach (var interval in _intervals)
            {
                if (interval.Contains(item))
                    return true;
            }

            return false;
        }

        public override string ToString()
        {
            return string.Join(", ", _intervals);
        }
    }
}
