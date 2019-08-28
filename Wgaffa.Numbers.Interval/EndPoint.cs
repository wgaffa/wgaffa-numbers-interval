using System;

namespace Wgaffa.Numbers
{
    public class EndPoint<T> : IComparable<T> where T : IComparable<T>
    {
        public static EndPoint<T> PositiveInfinity = new EndPointPositiveInfinity();
        public static EndPoint<T> NegativeInfinity = new EndPointNegativeInfinity();
        public static EndPoint<T> Infinity = new EndPointInfinity();

        internal virtual EndPoint<T> Lower => this;
        internal virtual EndPoint<T> Upper => this;
        internal Func<T, bool> IsInsideUpperBounds;
        internal Func<T, bool> IsInsideLowerBounds;

        public T Value { get; }
        public bool Inclusive { get; }

        public EndPoint(T value, bool inclusive = true)
        {
            Value = value;
            Inclusive = inclusive;

            if (Inclusive)
            {
                IsInsideUpperBounds = (T x) => CompareTo(x) >= 0;
                IsInsideLowerBounds = (T x) => CompareTo(x) <= 0;
            }
            else
            {
                IsInsideUpperBounds = (T x) => CompareTo(x) > 0;
                IsInsideLowerBounds = (T x) => CompareTo(x) < 0;
            }
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
            internal override EndPoint<T> Lower => NegativeInfinity;
            internal override EndPoint<T> Upper => PositiveInfinity;

            public EndPointInfinity()
                : base(default)
            {
            }

            public override int CompareTo(T other)
            {
                return 0;
            }
        }

        class EndPointPositiveInfinity : EndPoint<T>
        {
            public EndPointPositiveInfinity()
                : base(default)
            {
            }

            public override int CompareTo(T other)
            {
                return 1;
            }
        }

        class EndPointNegativeInfinity : EndPoint<T>
        {
            public EndPointNegativeInfinity()
                : base(default)
            {
            }

            public override int CompareTo(T other)
            {
                return -1;
            }
        }
    }
}
