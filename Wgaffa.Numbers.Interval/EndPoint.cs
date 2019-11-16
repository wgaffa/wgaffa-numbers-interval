using System;

namespace Wgaffa.Numbers
{
    /// <summary>
    /// Represents a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    [Serializable]
    public class EndPoint<T> : IComparable<T>, IComparable<EndPoint<T>>, IEquatable<EndPoint<T>> where T : IComparable<T>
    {
        /// <summary>
        /// The positive infinity value of the chosen type.
        /// </summary>
        public static readonly EndPoint<T> PositiveInfinity = new EndPointPositiveInfinity();

        /// <summary>
        /// The Negative infinity value of the chosen type.
        /// </summary>
        public static readonly EndPoint<T> NegativeInfinity = new EndPointPositiveInfinity(-1);

        /// <summary>
        /// The infinity value of the chosen type.
        /// </summary>
        /// <remarks>
        /// This is either <see cref="PositiveInfinity" /> or <see cref="NegativeInfinity"/> based on context.
        /// </remarks>
        public static readonly EndPoint<T> Infinity = new EndPointInfinity();

        internal virtual EndPoint<T> Lower => this;
        internal virtual EndPoint<T> Upper => this;
        internal Func<EndPoint<T>, bool> IsAfter;
        internal Func<EndPoint<T>, bool> IsBefore;

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets wether this endpoint is inclusive.
        /// </summary>
        public bool Inclusive { get; }

        /// <summary>
        /// Initializes a new instances of <see cref="EndPoint{T}"/> class.
        /// </summary>
        /// <param name="value">The value of this endpoint.</param>
        /// <param name="inclusive">Is the endpoint inclusive?</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        public EndPoint(T value, bool inclusive = true)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Value = value;
            Inclusive = inclusive;

            if (Inclusive)
            {
                IsAfter = (EndPoint<T> x) => CompareTo(x) >= 0;
                IsBefore = (EndPoint<T> x) => CompareTo(x) <= 0;
            }
            else
            {
                IsAfter = (EndPoint<T> x) => CompareTo(x) > 0;
                IsBefore = (EndPoint<T> x) => CompareTo(x) < 0;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "<Pending>")]
        public static implicit operator EndPoint<T>(T value)
        {
            return new EndPoint<T>(value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "<Pending>")]
        public static implicit operator T(EndPoint<T> other)
        {
            if (other == null)
                return default;

            return other.Value;
        }

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

        #region Overloaded operations
        public static bool operator <(EndPoint<T> left, EndPoint<T> right) => EndPoint<T>.CompareTo(left, right) < 0;
        public static bool operator >(EndPoint<T> left, EndPoint<T> right) => EndPoint<T>.CompareTo(left, right) > 0;
        public static bool operator <=(EndPoint<T> left, EndPoint<T> right) => EndPoint<T>.CompareTo(left, right) <= 0;
        public static bool operator >=(EndPoint<T> left, EndPoint<T> right) => EndPoint<T>.CompareTo(left, right) >= 0;
        public static bool operator ==(EndPoint<T> left, EndPoint<T> right) => object.Equals(left, right);
        public static bool operator !=(EndPoint<T> left, EndPoint<T> right) => !object.Equals(left, right);
        private static int CompareTo(EndPoint<T> left, EndPoint<T> right)
        {
            if (ReferenceEquals(left, right))
                return 0;

            if (left is null)
                return -1;

            if (right is null)
                return 1;

            return left.CompareTo(right);
        }
        #endregion

        #region Factory Methods
        /// <summary>
        /// Create a closed (inclusive) <see cref="EndPoint{T}" /> of type T.
        /// </summary>
        /// <param name="value">The value to use for the closed bound.</param>
        /// <returns>Returns a new closed <see cref="EndPoint{T}" /> with the new value.</returns>
        public static EndPoint<T> Closed(T value) => new EndPoint<T>(value);

        /// <summary>
        /// Create an open (non inclusive) <see cref="EndPoint{T}" /> of type T.
        /// </summary>
        /// <param name="value">The value to use for the open bound.</param>
        /// <returns>Returns a new open <see cref="EndPoint{T}" /> with the new value.</returns>
        public static EndPoint<T> Open(T value) => new EndPoint<T>(value, false);
        #endregion

        public override string ToString()
        {
            return Value.ToString();
        }

        public bool Equals(EndPoint<T> other)
        {
            if (other == null)
                return false;

            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            return Equals(obj as EndPoint<T>);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 17;
                hashCode = hashCode * 223 + Value.GetHashCode();
                hashCode = hashCode * 223 + Inclusive.GetHashCode();

                return hashCode;
            }
        }

        class EndPointInfinity : EndPoint<T>
        {
            internal override EndPoint<T> Lower => NegativeInfinity;
            internal override EndPoint<T> Upper => PositiveInfinity;

            public EndPointInfinity()
                : base(default, false)
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
                : base(default, false)
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
