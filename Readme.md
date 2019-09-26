![GitHub](https://img.shields.io/github/license/wgaffa/wgaffa-numbers-interval) ![Nuget](https://img.shields.io/nuget/v/Wgaffa.Numbers.Interval)

# Interval
Interval is a generic library to represent an interval between two objects with support for infinity.

## Install

## Using Interval

### Simple Bounded Intervals
To be able to have an interval the type has to implement IComparable<>.

Create bounded closed intervals.
```csharp
var interval = new Interval<float>(1.5f, 25.2f); // [1.5, 25.2]
```
To create an open bound endpoint you specify it in a new EndPoint<> object.
```csharp
var openEndPoint = new EndPoint<float>(2.5f, false);
var leftBoundOpenRightBoundClosed = new Interval(openEndPoint, 5.5f); // (2.5, 5.5]
var leftBoundClosedRightBoundOpen = new Interval(-2.5f, openEndPoint); // [-2.5, 2.5)
```
### Simple Unbounded Intervals
An endpoint can be positive infinity or negative infinity and to represent that EndPoint<> has properties to retrieve each of those as well as the simpler `infinity`property.

```csharp
var negativeInfinity = EndPoint<float>.NegativeInfinity;
var positiveInfinity = EndPoint<float>.PositiveInfinity;
var contextInfinity = EndPoint<float>.Infinity;

new Interval<float>(negativeInfinity, positiveInfinity); // (-Inf, +Inf)
new Interval<float>(contextInfinity, contextInfinity); // (-Inf, +Inf)
```
Note that using the `Infinity`property you don't have to use `NegativeInfinity` or  `PositiveInfinity` as Interval will use them depending on if they are used as the lower or upper bounds.

### Empty Intervals
An interval is considered empty and the property `IsEmpty` is true when these conditions are met.

Lower bound is bigger than upper bound
```csharp
var emptyInterval = new Interval<float>(10f, 7.5f); [10, 7.5]
```
If lower and upper bounds are equal but one of them is open
```csharp
var emptyInterval = new Interval<float>(new EndPoint<float>(2.5f, false), 2.5f); // (2.5f, 2.5]
```
### Intersect Intervals
Intersecting two intervals is easy

```csharp
var first = new Interval<float>(5f, 10f); // [5, 10]
var second = new Interval<float>(7f, 15f); // [7, 15]
var intersected = first.Intersect(second); // [7, 10]
```
### Union Intervals
You can merge several intervals together and get a collection of `Interval`s.

```csharp
var intervals = new List<Interval<float>>() {
	new Interval<float>(5f, 10f),
	new Interval<float>(17f, 17f),
	new Interval<float>(7f, 20f),
	new Interval<float>(-5f, 2f)
};

var first = new Interval<float>(7f, 8f);
var unionIntervals = first.Union(intervals); // [-5, 2] U [5, 20]
```
