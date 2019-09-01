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

        public bool Equals(EndPointPair<T> other)
        {
            return Lower.Equals(other.Lower) && Upper.Equals(other.Upper);
        }
    }
}
