# Intervals

A .NET library for working with intervals.

## Usage

```csharp
using Intervals;

var a = Interval.Create(1, 5);                 // [1, 5]
var b = 1.To(5, inclusiveEnd: false);          // [1, 5)
var c = 5.From(1);                              // [1, 5]

var hasThree = a.Includes(3);                  // true
var overlaps = a.Overlaps(4.To(10));           // true
var intersection = a.Intersection(4.To(10));   // [4, 5]
```

## Notes

Intervals can be inclusive or exclusive on either end.
