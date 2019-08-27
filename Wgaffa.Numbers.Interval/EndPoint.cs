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
            private readonly int _returnValue;
            
            internal override EndPoint<T> Lower => NegativeInfinity;
            internal override EndPoint<T> Upper => PositiveInfinity;

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
