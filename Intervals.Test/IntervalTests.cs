using Intervals;
using Xunit;

namespace Intervals.Test
{
    public class IntervalTests
    {
        [Fact]
        public void ConstructorAllowsNullStartAndEnd()
        {
            var startUnbounded = new Interval<int?>(null, 5);
            var endUnbounded = new Interval<int?>(0, null);
            var fullyUnbounded = new Interval<int?>(null, null);

            Assert.NotNull(startUnbounded);
            Assert.NotNull(endUnbounded);
            Assert.NotNull(fullyUnbounded);
        }

        [Fact]
        public void GetHashCodeAllowsNullStartAndEnd()
        {
            _ = new Interval<int?>(null, 5).GetHashCode();
            _ = new Interval<int?>(0, null).GetHashCode();
            _ = new Interval<int?>(null, null).GetHashCode();
        }
        [Theory]
        [InlineData(1, 5, true, true, 1, 5, true, true)]
        [InlineData(1, 5, true, true, 1, 5, false, false)]
        [InlineData(1, 5, true, true, 5, 10, true, true)]
        [InlineData(1, 5, true, true, 0, 3, true, true)]
        [InlineData(1, 5, true, true, 3, 4, true, true)]
        [InlineData(1, 5, true, true, 3, 6, true, true)]
        [InlineData(1, 5, true, true, 0, 6, false, false)]
        public void OverlapsRange(int sourceStart, int sourceEnd, bool sourceInclusiveStart, bool sourceInclusiveEnd, int targetStart, int targetEnd, bool targetInclusiveStart, bool targetInclusiveEnd)
        {
            Assert.True(sourceStart.To(sourceEnd, sourceInclusiveStart, sourceInclusiveEnd).Overlaps(targetStart.To(targetEnd, targetInclusiveStart, targetInclusiveEnd)));
        }

        [Theory]
        [InlineData(1, 5, true, false, 5, 10, true, true)]
        [InlineData(1, 5, true, true, 5, 10, false, true)]
        [InlineData(1, 5, true, true, 6, 10, true, true)]
        public void DoesNotOverlapRange(int sourceStart, int sourceEnd, bool sourceInclusiveStart, bool sourceInclusiveEnd, int targetStart, int targetEnd, bool targetInclusiveStart, bool targetInclusiveEnd)
        {
            Assert.False(sourceStart.To(sourceEnd, sourceInclusiveStart, sourceInclusiveEnd).Overlaps(targetStart.To(targetEnd, targetInclusiveStart, targetInclusiveEnd)));
        }

        [Theory]
        [InlineData(1, 5, true, true, 1, 5, true, true)]
        [InlineData(1, 5, true, true, 1, 5, false, false)]
        [InlineData(1, 5, false, false, 1, 5, false, false)]
        [InlineData(1, 5, true, true, 2, 4, true, true)]
        public void IncludesRange(int sourceStart, int sourceEnd, bool sourceInclusiveStart, bool sourceInclusiveEnd, int targetStart, int targetEnd, bool targetInclusiveStart, bool targetInclusiveEnd)
        {
            Assert.True(sourceStart.To(sourceEnd, sourceInclusiveStart, sourceInclusiveEnd).Includes(targetStart.To(targetEnd, targetInclusiveStart, targetInclusiveEnd)));
        }

        [Theory]
        [InlineData(1, 5, true, true, 1)]
        [InlineData(1, 5, true, true, 3)]
        [InlineData(1, 5, false, false, 3)]
        [InlineData(1, 5, true, true, 5)]
        public void IncludesValue(int sourceStart, int sourceEnd, bool sourceInclusiveStart, bool sourceInclusiveEnd, int target)
        {
            Assert.True(sourceStart.To(sourceEnd, sourceInclusiveStart, sourceInclusiveEnd).Includes(target));
        }

        [Theory]
        [InlineData(1, 5, false, false, 1, 5, true, true)]
        [InlineData(1, 5, true, true, 1, 6, true, true)]
        [InlineData(1, 5, true, true, 0, 5, true, true)]
        public void DoesNotIncludeRange(int sourceStart, int sourceEnd, bool sourceInclusiveStart, bool sourceInclusiveEnd, int targetStart, int targetEnd, bool targetInclusiveStart, bool targetInclusiveEnd)
        {
            Assert.False(sourceStart.To(sourceEnd, sourceInclusiveStart, sourceInclusiveEnd).Includes(targetStart.To(targetEnd, targetInclusiveStart, targetInclusiveEnd)));
        }

        [Theory]
        [InlineData(1, 5, false, false, 0)]
        [InlineData(1, 5, false, false, 1)]
        [InlineData(1, 5, false, false, 5)]
        [InlineData(1, 5, false, false, 6)]
        public void DoesNotIncludeValue(int sourceStart, int sourceEnd, bool sourceInclusiveStart, bool sourceInclusiveEnd, int target)
        {
            Assert.False(sourceStart.To(sourceEnd, sourceInclusiveStart, sourceInclusiveEnd).Includes(target));
        }

        [Fact]
        public void ExcludeEndOfRangeWithInclusiveEndCreatesIdenticalRangeExceptWithExclusiveEnd()
        {
            Assert.Equal(0.To(5, inclusiveEnd: false), 0.To(5, inclusiveEnd: true).ExcludeEnd());
        }

        [Fact]
        public void ExcludeEndOfRangeWithExclusiveEndReturnsItself()
        {
            var original = 0.To(5, inclusiveEnd: false);
            Assert.Same(original, original.ExcludeEnd());
        }

        [Fact]
        public void IncludeEndOfRangeWithExclusiveEndCreatesIdenticalRangeExceptWithInclusiveEnd()
        {
            Assert.Equal(0.To(5, inclusiveEnd: true), 0.To(5, inclusiveEnd: false).IncludeEnd());
        }

        [Fact]
        public void IncludeEndOfRangeWithInclusiveEndReturnsItself()
        {
            var original = 0.To(5, inclusiveEnd: true);
            Assert.Same(original, original.IncludeEnd());
        }

        [Fact]
        public void ExcludeStartOfRangeWithInclusiveStartCreatesIdenticalRangeExceptWithExclusiveStart()
        {
            Assert.Equal(0.To(5, inclusiveStart: false), 0.To(5, inclusiveStart: true).ExcludeStart());
        }

        [Fact]
        public void ExcludeStartOfRangeWithExclusiveStartReturnsItself()
        {
            var original = 0.To(5, inclusiveStart: false);
            Assert.Same(original, original.ExcludeStart());
        }

        [Fact]
        public void IncludeStartOfRangeWithExclusiveStartCreatesIdenticalRangeExceptWithInclusiveStart()
        {
            Assert.Equal(0.To(5, inclusiveStart: true), 0.To(5, inclusiveStart: false).IncludeStart());
        }

        [Fact]
        public void IncludeStartOfRangeWithInclusiveStartReturnsItself()
        {
            var original = 0.To(5, inclusiveStart: true);
            Assert.Same(original, original.IncludeStart());
        }

        [Theory]
        [InlineData(0, 5, true, true)]
        [InlineData(0, 5, true, false)]
        [InlineData(0, 5, false, true)]
        [InlineData(0, 5, false, false)]
        public void ToMethodIsAConstructorAlias(int start, int end, bool inclusiveStart, bool inclusiveEnd)
        {
            Assert.Equal(new Interval<int>(start, end, inclusiveStart, inclusiveEnd), start.To(end, inclusiveStart, inclusiveEnd));
        }


        [Theory]
        [InlineData(0, 5, true, true)]
        [InlineData(0, 5, true, false)]
        [InlineData(0, 5, false, true)]
        [InlineData(0, 5, false, false)]
        public void FromMethodIsAConstructorAlias(int start, int end, bool inclusiveStart, bool inclusiveEnd)
        {
            Assert.Equal(new Interval<int>(start, end, inclusiveStart, inclusiveEnd), end.From(start, inclusiveStart, inclusiveEnd));
        }


        [Theory]
        [InlineData(0, 5, true, true)]
        [InlineData(0, 5, true, false)]
        [InlineData(0, 5, false, true)]
        [InlineData(0, 5, false, false)]
        public void CreateMethodIsAConstructorAlias(int start, int end, bool inclusiveStart, bool inclusiveEnd)
        {
            Assert.Equal(new Interval<int>(start, end, inclusiveStart, inclusiveEnd), Interval.Create(start, end, inclusiveStart, inclusiveEnd));
        }


        [Fact]
        public void TheIntersectionOfARangeAndASuperRangeIsTheOriginalRange()
        {
            var original = 1.To(5);
            Assert.Same(original, original.Intersection(0.To(6)));
        }

        [Fact]
        public void TheIntersectionOfARangeAndASubRangeIsTheSubRange()
        {
            var subrange = 1.To(5);
            Assert.Same(subrange, 0.To(6).Intersection(subrange));
        }

        [Theory]
        [InlineData(1, 5, true, true, 0, 4, true, true, 1, 4, true, true)]
        [InlineData(1, 5, false, false, 0, 4, false, false, 1, 4, false, false)]
        [InlineData(1, 5, true, true, 2, 6, true, true, 2, 5, true, true)]
        [InlineData(0, 5, true, false, 1, 5, true, true, 1, 5, true, false)]
        [InlineData(1, 6, false, true, 1, 5, true, true, 1, 5, false, true)]
        public void CreateIntersectionOfTwoRanges(int aStart, int aEnd, bool aInclusiveStart, bool aInclusiveEnd, int bStart, int bEnd, bool bInclusiveStart, bool bInclusiveEnd, int cStart, int cEnd, bool cInclusiveStart, bool cInclusiveEnd)
        {
            var expected = Interval.Create(cStart, cEnd, cInclusiveStart, cInclusiveEnd);
            var actual = Interval.Create(aStart, aEnd, aInclusiveStart, aInclusiveEnd)
                .Intersection(Interval.Create(bStart, bEnd, bInclusiveStart, bInclusiveEnd));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 5, true, true, 3)]
        [InlineData(1, 5, false, true, 0)]
        [InlineData(1, 5, true, false, 3)]
        public void WithStartCreatesNewIntervalWithDifferentStart(int start, int end, bool inclusiveStart, bool inclusiveEnd, int newStart)
        {
            var original = start.To(end, inclusiveStart, inclusiveEnd);
            var result = original.WithStart(newStart);
            Assert.Equal(newStart, result.Start);
            Assert.Equal(end, result.End);
            Assert.Equal(inclusiveStart, result.IsStartIncluded);
            Assert.Equal(inclusiveEnd, result.IsEndIncluded);
        }

        [Theory]
        [InlineData(1, 5, true, true, 10)]
        [InlineData(1, 5, false, true, 3)]
        [InlineData(1, 5, true, false, 3)]
        public void WithEndCreatesNewIntervalWithDifferentEnd(int start, int end, bool inclusiveStart, bool inclusiveEnd, int newEnd)
        {
            var original = start.To(end, inclusiveStart, inclusiveEnd);
            var result = original.WithEnd(newEnd);
            Assert.Equal(start, result.Start);
            Assert.Equal(newEnd, result.End);
            Assert.Equal(inclusiveStart, result.IsStartIncluded);
            Assert.Equal(inclusiveEnd, result.IsEndIncluded);
        }

        [Fact]
        public void WithStartThrowsWhenNewStartIsGreaterThanEnd()
        {
            var interval = 1.To(5);
            Assert.Throws<System.ArgumentOutOfRangeException>(() => interval.WithStart(6));
        }

        [Fact]
        public void WithEndThrowsWhenNewEndIsLessThanStart()
        {
            var interval = 1.To(5);
            Assert.Throws<System.ArgumentOutOfRangeException>(() => interval.WithEnd(0));
        }

        [Fact]
        public void WithComparerCreatesNewIntervalWithDifferentComparer()
        {
            var interval = Interval.Create("a", "z");
            var result = interval.WithComparer(System.StringComparer.OrdinalIgnoreCase);
            Assert.Equal("a", result.Start);
            Assert.Equal("z", result.End);
            Assert.True(result.Includes("M"));
        }

        [Fact]
        public void WithStartPreservesComparer()
        {
            var comparer = System.Collections.Generic.Comparer<int>.Create((a, b) => b.CompareTo(a));
            var interval = new Interval<int>(5, 1, true, true, comparer);
            var result = interval.WithStart(4);
            Assert.Equal(4, result.Start);
            Assert.Equal(1, result.End);
        }

        [Fact]
        public void WithEndPreservesComparer()
        {
            var comparer = System.Collections.Generic.Comparer<int>.Create((a, b) => b.CompareTo(a));
            var interval = new Interval<int>(5, 1, true, true, comparer);
            var result = interval.WithEnd(0);
            Assert.Equal(5, result.Start);
            Assert.Equal(0, result.End);
        }

        // Constructor validation

        [Fact]
        public void ConstructorThrowsWhenStartIsGreaterThanEnd()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new Interval<int>(10, 1));
        }

        [Fact]
        public void ConstructorAllowsEqualStartAndEnd()
        {
            var interval = new Interval<int>(5, 5);
            Assert.Equal(5, interval.Start);
            Assert.Equal(5, interval.End);
        }

        // IsEmpty

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void IsEmptyWhenStartEqualsEndAndBoundIsExclusive(bool inclusiveStart, bool inclusiveEnd)
        {
            Assert.True(new Interval<int>(5, 5, inclusiveStart, inclusiveEnd).IsEmpty);
        }

        [Fact]
        public void IsNotEmptyWhenStartEqualsEndAndBothBoundsInclusive()
        {
            Assert.False(new Interval<int>(5, 5, true, true).IsEmpty);
        }

        [Fact]
        public void IsNotEmptyWhenStartDiffersFromEnd()
        {
            Assert.False(1.To(5).IsEmpty);
        }

        [Fact]
        public void IsNotEmptyWhenUnbounded()
        {
            Assert.False(new Interval<int?>(null, null).IsEmpty);
        }

        // Empty interval behavior

        [Fact]
        public void EmptyIntervalDoesNotOverlapNonEmptyInterval()
        {
            var empty = new Interval<int>(5, 5, false, false);
            Assert.False(empty.Overlaps(1.To(10)));
        }

        [Fact]
        public void NonEmptyIntervalDoesNotOverlapEmptyInterval()
        {
            var empty = new Interval<int>(5, 5, false, false);
            Assert.False(1.To(10).Overlaps(empty));
        }

        [Fact]
        public void EmptyIntervalDoesNotOverlapEmptyInterval()
        {
            var a = new Interval<int>(5, 5, false, false);
            var b = new Interval<int>(5, 5, true, false);
            Assert.False(a.Overlaps(b));
        }

        [Fact]
        public void NonEmptyIntervalIncludesEmptyInterval()
        {
            var empty = new Interval<int>(5, 5, false, false);
            Assert.True(1.To(10).Includes(empty));
        }

        [Fact]
        public void EmptyIntervalDoesNotIncludeNonEmptyInterval()
        {
            var empty = new Interval<int>(5, 5, false, false);
            Assert.False(empty.Includes(1.To(10)));
        }

        [Fact]
        public void IntersectionWithEmptyIntervalIsNull()
        {
            var empty = new Interval<int>(5, 5, false, false);
            Assert.Null(1.To(10).Intersection(empty));
        }

        [Fact]
        public void IntersectionOfEmptyIntervalWithNonEmptyIsNull()
        {
            var empty = new Interval<int>(5, 5, false, false);
            Assert.Null(empty.Intersection(1.To(10)));
        }

        // Unbounded interval behavior

        [Fact]
        public void UnboundedStartIncludesAllLowerValues()
        {
            var interval = new Interval<int?>(null, 5);
            Assert.True(interval.Includes(-1000));
            Assert.True(interval.Includes(0));
            Assert.True(interval.Includes(5));
        }

        [Fact]
        public void UnboundedEndIncludesAllUpperValues()
        {
            var interval = new Interval<int?>(0, null);
            Assert.True(interval.Includes(0));
            Assert.True(interval.Includes(1000));
        }

        [Fact]
        public void FullyUnboundedIncludesAnyValue()
        {
            var interval = new Interval<int?>(null, null);
            Assert.True(interval.Includes(-1000));
            Assert.True(interval.Includes(0));
            Assert.True(interval.Includes(1000));
        }

        [Fact]
        public void UnboundedIntervalOverlapsBoundedInterval()
        {
            var unbounded = new Interval<int?>(null, null);
            var bounded = new Interval<int?>(1, 5);
            Assert.True(unbounded.Overlaps(bounded));
            Assert.True(bounded.Overlaps(unbounded));
        }

        [Fact]
        public void UnboundedIntervalIncludesBoundedInterval()
        {
            var unbounded = new Interval<int?>(null, null);
            var bounded = new Interval<int?>(1, 5);
            Assert.True(unbounded.Includes(bounded));
        }

        [Fact]
        public void IntersectionOfBoundedAndUnboundedIsBounded()
        {
            var unbounded = new Interval<int?>(null, null);
            var bounded = new Interval<int?>(1, 5);
            var intersection = unbounded.Intersection(bounded);
            Assert.Same(bounded, intersection);
        }

        // CompareTo

        [Fact]
        public void CompareToNullReturnsPositive()
        {
            Assert.True(1.To(5).CompareTo(null) > 0);
        }

        [Fact]
        public void CompareToEqualIntervalReturnsZero()
        {
            Assert.Equal(0, 1.To(5).CompareTo(1.To(5)));
        }

        [Fact]
        public void CompareToSortsLowerStartFirst()
        {
            Assert.True(1.To(5).CompareTo(2.To(5)) < 0);
            Assert.True(2.To(5).CompareTo(1.To(5)) > 0);
        }

        [Fact]
        public void CompareToSortsByEndWhenStartsEqual()
        {
            Assert.True(1.To(3).CompareTo(1.To(5)) < 0);
            Assert.True(1.To(5).CompareTo(1.To(3)) > 0);
        }

        [Fact]
        public void CompareToSortsByStartInclusivityWhenStartsEqual()
        {
            var inclusive = 1.To(5, inclusiveStart: true);
            var exclusive = 1.To(5, inclusiveStart: false);
            Assert.True(inclusive.CompareTo(exclusive) > 0);
            Assert.True(exclusive.CompareTo(inclusive) < 0);
        }

        [Fact]
        public void CompareToSortsByEndInclusivityWhenEndsEqual()
        {
            var inclusive = 1.To(5, inclusiveEnd: true);
            var exclusive = 1.To(5, inclusiveEnd: false);
            Assert.True(inclusive.CompareTo(exclusive) < 0);
            Assert.True(exclusive.CompareTo(inclusive) > 0);
        }

        // Equals

        [Fact]
        public void EqualsReturnsTrueForIdenticalIntervals()
        {
            Assert.True(1.To(5).Equals(1.To(5)));
        }

        [Fact]
        public void EqualsReturnsFalseForDifferentStarts()
        {
            Assert.False(1.To(5).Equals(2.To(5)));
        }

        [Fact]
        public void EqualsReturnsFalseForDifferentEnds()
        {
            Assert.False(1.To(5).Equals(1.To(6)));
        }

        [Fact]
        public void EqualsReturnsFalseForDifferentInclusivity()
        {
            Assert.False(1.To(5, inclusiveStart: true).Equals(1.To(5, inclusiveStart: false)));
            Assert.False(1.To(5, inclusiveEnd: true).Equals(1.To(5, inclusiveEnd: false)));
        }

        [Fact]
        public void EqualsReturnsFalseForNull()
        {
            Assert.False(1.To(5).Equals((Interval<int>)null));
        }

        [Fact]
        public void EqualsObjectReturnsTrueForEqualInterval()
        {
            object other = 1.To(5);
            Assert.True(1.To(5).Equals(other));
        }

        [Fact]
        public void EqualsObjectReturnsFalseForNonInterval()
        {
            Assert.False(1.To(5).Equals("not an interval"));
        }

        // Operators == and !=

        [Fact]
        public void EqualityOperatorReturnsTrueForEqualIntervals()
        {
            Assert.True(1.To(5) == 1.To(5));
        }

        [Fact]
        public void EqualityOperatorReturnsFalseForDifferentIntervals()
        {
            Assert.False(1.To(5) == 2.To(6));
        }

        [Fact]
        public void EqualityOperatorReturnsTrueForBothNull()
        {
            Interval<int> a = null;
            Interval<int> b = null;
            Assert.True(a == b);
        }

        [Fact]
        public void EqualityOperatorReturnsFalseWhenOneIsNull()
        {
            Interval<int> a = 1.To(5);
            Interval<int> b = null;
            Assert.False(a == b);
            Assert.False(b == a);
        }

        [Fact]
        public void InequalityOperatorReturnsTrueForDifferentIntervals()
        {
            Assert.True(1.To(5) != 2.To(6));
        }

        [Fact]
        public void InequalityOperatorReturnsFalseForEqualIntervals()
        {
            Assert.False(1.To(5) != 1.To(5));
        }

        // GetHashCode

        [Fact]
        public void GetHashCodeIsEqualForEqualIntervals()
        {
            Assert.Equal(1.To(5).GetHashCode(), 1.To(5).GetHashCode());
        }

        [Fact]
        public void GetHashCodeDiffersForDifferentIntervals()
        {
            Assert.NotEqual(1.To(5).GetHashCode(), 2.To(6).GetHashCode());
        }

        // ToString

        [Fact]
        public void ToStringReturnsStartToEnd()
        {
            Assert.Equal("1 to 5", 1.To(5).ToString());
        }

        [Fact]
        public void ToStringWithFormatAppliesFormat()
        {
            var interval = Interval.Create(1.5, 3.5);
            Assert.Equal("1.50 to 3.50", interval.ToString("F2"));
        }

        [Fact]
        public void ToStringWithNullFormatReturnsDefaultFormat()
        {
            Assert.Equal("1 to 5", 1.To(5).ToString(null));
        }

        // Intersection returns null for non-overlapping

        [Fact]
        public void IntersectionOfNonOverlappingRangesReturnsNull()
        {
            Assert.Null(1.To(3).Intersection(5.To(10)));
        }
    }
}
