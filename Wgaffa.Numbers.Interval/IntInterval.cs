using System;

namespace Wgaffa.Numbers
{
    public class IntInterval : Interval<int>
    {
        public IntInterval(EndPoint<int> lower, EndPoint<int> upper) : base(lower, upper)
        {
        }

        protected override bool IsContinuousLeft(Interval<int> other)
        {
            if (other == null)
                return false;

            return (Lower.Inclusive && other.Upper.Inclusive) && Math.Abs(Lower.Value - other.Upper.Value) == 1;
        }

        protected override bool IsContinousRight(Interval<int> other)
        {
            if (other == null)
                return false;

            return (Upper.Inclusive && other.Lower.Inclusive) && Math.Abs(Upper.Value - other.Lower.Value) == 1;
        }
    }
}
