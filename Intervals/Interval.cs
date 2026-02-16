using System;
using System.Collections.Generic;

namespace Intervals
{
    /// <summary>
    /// Factory and extension methods for creating <see cref="Interval{T}"/> instances.
    /// </summary>
    /// <example>
    /// <code>
    /// var a = Interval.Create(1, 5);      // [1, 5]
    /// var b = 1.To(5, inclusiveEnd: false); // [1, 5)
    /// var c = 5.From(1);                 // [1, 5]
    /// </code>
    /// </example>
    public static class Interval
    {
        /// <summary>
        /// Creates a new <see cref="Interval{T}"/>.
        /// </summary>
        /// <param name="start">The start of the interval. Use <c>null</c> for an unbounded start.</param>
        /// <param name="end">The end of the interval. Use <c>null</c> for an unbounded end.</param>
        /// <param name="inclusiveStart">Whether the start value is included in the interval. Default is <c>true</c>.</param>
        /// <param name="inclusiveEnd">Whether the end value is included in the interval. Default is <c>true</c>.</param>
        /// <param name="comparer">The comparer to use for ordering values. If <c>null</c>, the default comparer is used.</param>
        /// <returns>A new <see cref="Interval{T}"/> instance.</returns>
        /// <example>
        /// <code>
        /// var interval = Interval.Create(0, 10, inclusiveStart: true, inclusiveEnd: false); // [0, 10)
        /// </code>
        /// </example>
        public static Interval<T> Create<T>(T start, T end, bool inclusiveStart = true, bool inclusiveEnd = true, IComparer<T> comparer = null)
        {
            return new Interval<T>(start, end, inclusiveStart, inclusiveEnd, comparer);
        }

        /// <summary>
        /// Creates a new <see cref="Interval{T}"/> using extension syntax on the start value.
        /// </summary>
        /// <param name="start">The start of the interval. Use <c>null</c> for an unbounded start.</param>
        /// <param name="end">The end of the interval. Use <c>null</c> for an unbounded end.</param>
        /// <param name="inclusiveStart">Whether the start value is included in the interval. Default is <c>true</c>.</param>
        /// <param name="inclusiveEnd">Whether the end value is included in the interval. Default is <c>true</c>.</param>
        /// <param name="comparer">The comparer to use for ordering values. If <c>null</c>, the default comparer is used.</param>
        /// <returns>A new <see cref="Interval{T}"/> instance.</returns>
        /// <example>
        /// <code>
        /// var interval = 1.To(5); // [1, 5]
        /// var exclusive = 1.To(5, inclusiveEnd: false); // [1, 5)
        /// </code>
        /// </example>
        public static Interval<T> To<T>(this T start, T end, bool inclusiveStart = true, bool inclusiveEnd = true, IComparer<T> comparer = null)
        {
            return Create(start, end, inclusiveStart, inclusiveEnd, comparer);
        }

        /// <summary>
        /// Creates a new <see cref="Interval{T}"/> using extension syntax on the end value.
        /// </summary>
        /// <param name="end">The end of the interval. Use <c>null</c> for an unbounded end.</param>
        /// <param name="start">The start of the interval. Use <c>null</c> for an unbounded start.</param>
        /// <param name="inclusiveStart">Whether the start value is included in the interval. Default is <c>true</c>.</param>
        /// <param name="inclusiveEnd">Whether the end value is included in the interval. Default is <c>true</c>.</param>
        /// <param name="comparer">The comparer to use for ordering values. If <c>null</c>, the default comparer is used.</param>
        /// <returns>A new <see cref="Interval{T}"/> instance.</returns>
        /// <example>
        /// <code>
        /// var interval = 5.From(1); // [1, 5]
        /// </code>
        /// </example>
        public static Interval<T> From<T>(this T end, T start, bool inclusiveStart = true, bool inclusiveEnd = true, IComparer<T> comparer = null)
        {
            return Create(start, end, inclusiveStart, inclusiveEnd, comparer);
        }
    }

    /// <summary>
    /// Represents an interval of values between a start and end limit that can be either inclusive or exclusive.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    public class Interval<T> : IComparable<Interval<T>>, IEquatable<Interval<T>>
    {
        private readonly IComparer<T> _comparer;
        private readonly IEqualityComparer<T> _equalityComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Interval&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="start">The start of the interval. Use <c>null</c> for an unbounded start.</param>
        /// <param name="end">The end of the interval. Use <c>null</c> for an unbounded end.</param>
        /// <param name="startInclusive">Whether the start value is included in the interval. Default is <c>true</c>.</param>
        /// <param name="endInclusive">Whether the end value is included in the interval. Default is <c>true</c>.</param>
        /// <param name="comparer">The comparer to use for ordering values. If <c>null</c>, the default comparer is used.</param>
        /// <example>
        /// <code>
        /// var interval = new Interval&lt;int&gt;(0, 10, startInclusive: true, endInclusive: false); // [0, 10)
        /// </code>
        /// </example>
        public Interval(T start, T end, bool startInclusive = true, bool endInclusive = true, IComparer<T> comparer = null)
        {
            _comparer = comparer ?? Comparer<T>.Default;
            _equalityComparer = EqualityComparer<T>.Default;

            if (start != null && end != null && _comparer.Compare(start, end) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "Start value must be less than End value");
            }

            Start = start;
            End = end;
            IsStartIncluded = startInclusive;
            IsEndIncluded = endInclusive;
        }

        /// <summary>
        /// Returns true if the interval includes its start value.
        /// </summary>
        /// <remarks>
        /// A <c>null</c> start is treated as unbounded (negative infinity).
        /// </remarks>
        public bool IsStartIncluded { get; }

        /// <summary>
        /// Returns true if the interval includes its end value.
        /// </summary>
        /// <remarks>
        /// A <c>null</c> end is treated as unbounded (positive infinity).
        /// </remarks>
        public bool IsEndIncluded { get; }

        /// <summary>
        /// Gets the start value of the interval.
        /// </summary>
        /// <value>The start.</value>
        public T Start { get; }

        /// <summary>
        /// Gets the end value of the interval.
        /// </summary>
        /// <value>The end.</value>
        public T End { get; }

        /// <summary>
        /// Returns true if the interval is empty (contains no values).
        /// </summary>
        /// <remarks>
        /// An interval is empty when start equals end and at least one bound is exclusive.
        /// For example, (5,5), [5,5), and (5,5] are all empty intervals.
        /// </remarks>
        public bool IsEmpty
        {
            get
            {
                return Start != null && End != null && _equalityComparer.Equals(Start, End) && (!IsStartIncluded || !IsEndIncluded);
            }
        }

        /// <summary>
        /// Creates a <see cref="Interval&lt;T&gt;"/> object that is made up by the intersection of two intervals.
        /// </summary>
        /// <param name="range">The interval to intersect with.</param>
        /// <returns>A new <see cref="Interval{T}"/> representing the intersection, or <c>null</c> if the intervals do not overlap or either is empty.</returns>
        /// <example>
        /// <code>
        /// var a = 0.To(10);
        /// var b = 5.To(15);
        /// var intersection = a.Intersection(b); // [5, 10]
        /// </code>
        /// </example>
        public Interval<T> Intersection(Interval<T> range)
        {
            if (IsEmpty || range.IsEmpty)
            {
                return null;
            }

            return !Includes(range) ? !range.Includes(this) ? !IncludesStart(range) ? !IncludesEnd(range) ? null : new Interval<T>(Start, range.End, IsStartIncluded, range.IsEndIncluded) : new Interval<T>(range.Start, End, range.IsStartIncluded, IsEndIncluded) : this : range;
        }

        /// <summary>
        /// Determines whether the specified object of type <c>T</c> is contained within this interval.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns>
        ///     <c>true</c> if the specified object is within the interval; otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// var interval = 1.To(5);
        /// bool hasThree = interval.Includes(3); // true
        /// </code>
        /// </example>
        public bool Includes(T obj)
        {
            return (IsStartIncluded ? CompareStartToValue(obj) <= 0 : CompareStartToValue(obj) < 0) && (IsEndIncluded ? CompareEndToValue(obj) >= 0 : CompareEndToValue(obj) > 0);
        }

        private bool IncludesStart(Interval<T> range)
        {
            return ((IsStartIncluded || !range.IsStartIncluded) ? (CompareStartToStart(range.Start) <= 0) : (CompareStartToStart(range.Start) < 0)) &&
                ((IsEndIncluded == range.IsStartIncluded) ? CompareEndToStart(range.Start) >= 0 : CompareEndToStart(range.Start) > 0);
        }

        private bool IncludesEnd(Interval<T> range)
        {
            return ((IsStartIncluded == range.IsEndIncluded) ? (CompareStartToEnd(range.End) <= 0) : (CompareStartToEnd(range.End) < 0)) &&
                ((IsEndIncluded || !range.IsEndIncluded) ? CompareEndToEnd(range.End) >= 0 : CompareEndToEnd(range.End) > 0);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Interval&lt;T&gt;"/> object is contained within this interval.
        /// </summary>
        /// <param name="range">The range to check.</param>
        /// <returns>
        ///     <c>true</c> if the specified interval is within the interval; otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// var outer = 0.To(10);
        /// var inner = 2.To(8);
        /// bool contained = outer.Includes(inner); // true
        /// </code>
        /// </example>
        public bool Includes(Interval<T> range)
        {
            return range.IsEmpty || (!IsEmpty && IncludesStart(range) && IncludesEnd(range));
        }

        /// <summary>
        /// Determines whether this interval overlaps the specified <see cref="Interval&lt;T&gt;"/>.
        /// </summary>
        /// <param name="range">The range to check.</param>
        /// <returns>
        ///     <c>true</c> if the two intervals overlap; otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// var a = 0.To(5, inclusiveEnd: false); // [0, 5)
        /// var b = 5.To(10);                     // [5, 10]
        /// bool overlaps = a.Overlaps(b);        // false
        /// </code>
        /// </example>
        public bool Overlaps(Interval<T> range)
        {
            if (IsEmpty || range.IsEmpty)
            {
                return false;
            }

            return IncludesStart(range) || IncludesEnd(range) || range.Includes(this);
        }

        /// <summary>
        /// Determines if two <see cref="Interval&lt;T&gt;"/> objects are equal.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Interval<T> source, Interval<T> target)
        {
            return Equals(source, target) || (!Equals(source, null) && source.Equals(target));
        }

        /// <summary>
        /// Determines if two <see cref="Interval&lt;T&gt;"/> objects are not equal.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Interval<T> source, Interval<T> target)
        {
            return !(source == target);
        }

        /// <summary>
        /// Compares this <see cref="Interval&lt;T&gt;"/> interval object with the specified <see cref="Interval&lt;T&gt;"/> object
        /// and returns an integer that indicates their relationship to one another in the sort order.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>A value less than 0 if this interval is less than <paramref name="other"/>, 0 if equal, or greater than 0 if this interval is greater than <paramref name="other"/>.</returns>
        public int CompareTo(Interval<T> other)
        {
            return other == null ? 1 : StartEquals(other.Start) ? IsStartIncluded == other.IsStartIncluded ? IsEndIncluded == other.IsEndIncluded ? CompareEndToEnd(other.End) : (IsEndIncluded ? -1 : 1) : (IsStartIncluded ? 1 : -1) : CompareStartToStart(other.Start);
        }

        /// <summary>
        /// Determines whether the <see cref="Interval&lt;T&gt;"/> instances are considered equal.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>
        ///     <c>true</c> if the start and end values are equal; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Interval<T> other)
        {
            return other != null && StartEquals(other.Start) && EndEquals(other.End) && IsStartIncluded == other.IsStartIncluded && IsEndIncluded == other.IsEndIncluded;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this <see cref="Interval&lt;T&gt;"/> instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this <see cref="Interval&lt;T&gt;"/> instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this <see cref="Interval&lt;T&gt;"/> instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object other)
        {
            return other is Interval<T> ? Equals((Interval<T>)other) : false;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="Interval&lt;T&gt;"/> instance.
        /// </summary>
        /// <returns>
        /// A hash code for this <see cref="Interval&lt;T&gt;"/> instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return CombineHashCodes(_equalityComparer.GetHashCode(Start), _equalityComparer.GetHashCode(End), IsStartIncluded.GetHashCode(), IsEndIncluded.GetHashCode());
        }

        private static int CombineHashCodes(int h1, int h2)
        {
            return ((h1 << 5) + h1) ^ h2;
        }

        private static int CombineHashCodes(int h1, int h2, int h3, int h4)
        {
            return CombineHashCodes(CombineHashCodes(h1, h2), CombineHashCodes(h3, h4));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this <see cref="Interval&lt;T&gt;"/> instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String"/> that represents this <see cref="Interval&lt;T&gt;"/> instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Start} to {End}";
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this <see cref="Interval&lt;T&gt;"/> instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String"/> that represents this <see cref="Interval&lt;T&gt;"/> instance.
        /// </returns>
        public string ToString(string format)
        {
            format = format == null ? format : $":{format}";
            return string.Format($"{{0{format}}} to {{1{format}}}", Start, End);
        }

        /// <summary>
        /// Returns an identical range save for the start being excluded.
        /// </summary>
        /// <returns>A new <see cref="Interval{T}"/> with the start excluded, or this instance if already excluded.</returns>
        public Interval<T> ExcludeStart()
        {
            return IsStartIncluded ? Interval.Create(Start, End, false, IsEndIncluded) : this;

        }

        /// <summary>
        /// Returns an identical range save for the start being included.
        /// </summary>
        /// <returns>A new <see cref="Interval{T}"/> with the start included, or this instance if already included.</returns>
        public Interval<T> IncludeStart()
        {
            return IsStartIncluded ? this : Interval.Create(Start, End, true, IsEndIncluded);
        }

        /// <summary>
        /// Returns an identical range save for the end being excluded.
        /// </summary>
        /// <returns>A new <see cref="Interval{T}"/> with the end excluded, or this instance if already excluded.</returns>
        public Interval<T> ExcludeEnd()
        {
            return IsEndIncluded ? Interval.Create(Start, End, IsStartIncluded, false) : this;
        }

        /// <summary>
        /// Returns an identical range save for the end being included.
        /// </summary>
        /// <returns>A new <see cref="Interval{T}"/> with the end included, or this instance if already included.</returns>
        public Interval<T> IncludeEnd()
        {
            return IsEndIncluded ? this : Interval.Create(Start, End, IsStartIncluded, true);
        }

        /// <summary>
        /// Returns a new interval with a different start value, preserving all other settings.
        /// </summary>
        /// <param name="newStart">The new start value. Use <c>null</c> for an unbounded start.</param>
        /// <returns>A new <see cref="Interval{T}"/> with the specified start value.</returns>
        /// <example>
        /// <code>
        /// var interval = 1.To(5); // [1, 5]
        /// var shifted = interval.WithStart(3); // [3, 5]
        /// </code>
        /// </example>
        public Interval<T> WithStart(T newStart)
        {
            return Interval.Create(newStart, End, IsStartIncluded, IsEndIncluded, _comparer);
        }

        /// <summary>
        /// Returns a new interval with a different end value, preserving all other settings.
        /// </summary>
        /// <param name="newEnd">The new end value. Use <c>null</c> for an unbounded end.</param>
        /// <returns>A new <see cref="Interval{T}"/> with the specified end value.</returns>
        /// <example>
        /// <code>
        /// var interval = 1.To(5); // [1, 5]
        /// var shifted = interval.WithEnd(10); // [1, 10]
        /// </code>
        /// </example>
        public Interval<T> WithEnd(T newEnd)
        {
            return Interval.Create(Start, newEnd, IsStartIncluded, IsEndIncluded, _comparer);
        }

        /// <summary>
        /// Returns a new interval with a different comparer, preserving all other settings.
        /// </summary>
        /// <param name="newComparer">The new comparer to use for ordering values.</param>
        /// <returns>A new <see cref="Interval{T}"/> with the specified comparer.</returns>
        /// <example>
        /// <code>
        /// var interval = Interval.Create("a", "z");
        /// var caseInsensitive = interval.WithComparer(StringComparer.OrdinalIgnoreCase);
        /// </code>
        /// </example>
        public Interval<T> WithComparer(IComparer<T> newComparer)
        {
            return Interval.Create(Start, End, IsStartIncluded, IsEndIncluded, newComparer);
        }

        private int CompareStartToValue(T value)
        {
            return Start == null ? -1 : _comparer.Compare(Start, value);
        }

        private int CompareEndToValue(T value)
        {
            return End == null ? 1 : _comparer.Compare(End, value);
        }

        private int CompareStartToStart(T otherStart)
        {
            return Start == null ? (otherStart == null ? 0 : -1) : (otherStart == null ? 1 : _comparer.Compare(Start, otherStart));
        }

        private int CompareStartToEnd(T otherEnd)
        {
            return Start == null ? (otherEnd == null ? 0 : -1) : (otherEnd == null ? 1 : _comparer.Compare(Start, otherEnd));
        }

        private int CompareEndToStart(T otherStart)
        {
            return End == null ? 1 : (otherStart == null ? 1 : _comparer.Compare(End, otherStart));
        }

        private int CompareEndToEnd(T otherEnd)
        {
            return End == null ? (otherEnd == null ? 0 : 1) : (otherEnd == null ? -1 : _comparer.Compare(End, otherEnd));
        }

        private bool StartEquals(T other)
        {
            return _equalityComparer.Equals(Start, other);
        }

        private bool EndEquals(T other)
        {
            return _equalityComparer.Equals(End, other);
        }
    }
}
