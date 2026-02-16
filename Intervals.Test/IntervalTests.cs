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
    }
}
