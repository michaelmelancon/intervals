# Copilot Instructions for Intervals

A .NET library for working with intervals that can be inclusive or exclusive on either end.

## Build, Test, and Development

### Build
```bash
dotnet build
```

### Run All Tests
```bash
dotnet test
```

### Run a Single Test
```bash
dotnet test --filter "FullyQualifiedName~<TestMethodName>"
```
Example:
```bash
dotnet test --filter "FullyQualifiedName~ConstructorAllowsNullStartAndEnd"
```

### Create NuGet Package
Package is automatically generated on build when in Release configuration:
```bash
dotnet build -c Release
```

## Architecture

### Multi-targeting
The library targets multiple frameworks for broad compatibility:
- `netstandard1.0` and `netstandard2.0` - for .NET Standard
- `net35` - for legacy .NET Framework support

Tests target:
- `net48` - .NET Framework 4.8
- `net8.0` - modern .NET

### Core Design
- **Single-file implementation**: All functionality is in `Interval.cs` (approximately 430 lines)
- **Generic type**: `Interval<T>` works with any `IComparable<T>` type
- **Null handling**: `null` start values represent negative infinity, `null` end values represent positive infinity
- **Immutability**: Intervals are immutable; methods like `ExcludeStart()` return new instances

## Key Conventions

### Fluent Extension Methods
The library provides three ways to create intervals:
1. Factory method: `Interval.Create(1, 5)`
2. Start-based extension: `1.To(5)` - reads as "1 to 5"
3. End-based extension: `5.From(1)` - reads as "5 from 1"

All three create the same interval `[1, 5]`.

### Boolean Parameters for Inclusivity
Methods use `inclusiveStart` and `inclusiveEnd` (or `startInclusive`/`endInclusive`) rather than enums:
```csharp
var halfOpen = 1.To(5, inclusiveEnd: false);  // [1, 5)
```

### Ternary-Heavy Implementation
The codebase uses nested ternary operators extensively for compact conditional logic, especially in comparison and overlap detection methods. This is intentional for performance and brevity.

Example from `Intersection()`:
```csharp
return !Includes(range) ? !range.Includes(this) ? !IncludesStart(range) ? 
    !IncludesEnd(range) ? null : new Interval<T>(...) : new Interval<T>(...) : this : range;
```

When modifying existing methods with this pattern, maintain the style for consistency.

### XML Documentation
All public APIs are documented with XML comments including:
- Summary descriptions
- Parameter descriptions with special notes for `null` handling
- `<example>` blocks with code snippets
- `<remarks>` for edge cases (especially null/unbounded intervals)

### Comparison Logic
The library uses a private `IComparer<T>` field for all comparisons, with special handling for `null` values:
- `null` start is always less than any value (negative infinity)
- `null` end is always greater than any value (positive infinity)
- When both are `null`, they're considered equal

### Test Structure
Tests use xUnit with:
- `[Theory]` and `[InlineData]` for parameterized tests
- Descriptive test method names that indicate the scenario (e.g., `OverlapsRange`, `DoesNotOverlapRange`)
- Pairs of positive/negative test cases for most functionality
