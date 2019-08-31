using System;

namespace Wgaffa.Numbers
{
    public class EndPoint<T> : IComparable<T>, IComparable<EndPoint<T>> where T : IComparable<T>
    {
        public static EndPoint<T> PositiveInfinity = new EndPointPositiveInfinity();
        public static EndPoint<T> NegativeInfinity = new EndPointPositiveInfinity(-1);
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

        public virtual int CompareTo(EndPoint<T> other)
        {
            if (other is EndPointPositiveInfinity inf)
                return inf.Sign * -1;

            if (other is EndPointInfinity)
                return -1;

            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
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
                return 1;
            }

            public override int CompareTo(EndPoint<T> other)
            {
                if (other is EndPointPositiveInfinity inf)
                    return inf.Sign < 0 ? 1 : 0;

                if (other is EndPointInfinity)
                    return 0;

                return 1;
            }

            public override string ToString()
            {
                return "Inf";
            }
        }

        class EndPointPositiveInfinity : EndPoint<T>
        {
            private readonly int _returnValue;
            internal int Sign => _returnValue;
            
            public EndPointPositiveInfinity(int returnValue = 1)
                : base(default)
            {
                _returnValue = returnValue;
            }

            public override int CompareTo(T other)
            {
                return _returnValue;
            }

            public override int CompareTo(EndPoint<T> other)
            {
                if (other is EndPointPositiveInfinity inf)
                    return _returnValue.CompareTo(inf._returnValue);

                if (other is EndPointInfinity)
                    return 0;

                return _returnValue;
            }

            public override string ToString()
            {
                return _returnValue < 0 ? "-Inf" : "+Inf";
            }
        }
    }
}
