using System;

namespace Wgaffa.Numbers
{
    public class EndPoint<T> : IComparable<T> where T : IComparable<T>
    {
        public static EndPoint<T> PositiveInfinity = new EndPointInfinity(1);
        public static EndPoint<T> NegativeInfinity = new EndPointInfinity(-1);
        public static EndPoint<T> Infinity = new EndPointInfinity();

        internal virtual EndPoint<T> Lower => Value;
        internal virtual EndPoint<T> Upper => Value;

        public T Value { get; }

        public EndPoint(T value)
        {
            Value = value;
        }

        public static implicit operator EndPoint<T>(T value)
        {
            return new EndPoint<T>(value);
        }

        public static implicit operator T(EndPoint<T> other) => other.Value;

        public virtual int CompareTo(T other)
        {
            return Value.CompareTo(other);
        }

        class EndPointInfinity : EndPoint<T>
        {
            private int _returnValue;
            EndPoint<T> _lower = new EndPointInfinity(-1);
            EndPoint<T> _upper = new EndPointInfinity(1);

            internal override EndPoint<T> Lower => _lower;
            internal override EndPoint<T> Upper => _upper;

            public EndPointInfinity(int compareReturn = 0)
                : base(default)
            {
                _returnValue = compareReturn;
            }

            public override int CompareTo(T other)
            {
                return _returnValue;
            }
        }
    }
}
