namespace Wgaffa.Numbers
{
    public interface IInterval<T>
    {
        /// <summary>
        /// Check if the <paramref name="item"/> exists in the interval.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true if <paramref name="item"/> exists; otherwise, false.</returns>
        bool Contains(T item);
    }
}
