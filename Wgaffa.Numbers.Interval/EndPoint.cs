using System;

namespace Wgaffa.Numbers
{
    public class EndPoint<T> : IComparable<T> where T : IComparable<T>
    {
        public static EndPoint<T> PositiveInfinity = new EndPointInfinity(1);
        public static EndPoint<T> NegativeInfinity = new EndPointInfinity(-1);

        public T Value { get; }

        public EndPoint(T value)
        {
            Value = value;
        }

        public static implicit operator EndPoint<T>(T value)
        {
            return new EndPoint<T>(value);
        }

        public virtual int CompareTo(T other)
        {
            return Value.CompareTo(other);
        }

        class EndPointInfinity : EndPoint<T>
        {
            private int _returnValue;

            public EndPointInfinity(int compareReturn)
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
