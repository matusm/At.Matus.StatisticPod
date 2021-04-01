StatisticPod
=============

A Simple C# Statistic Library.

## StatisticPod
This class provides basic functionality for recording and processing data values. It is typically used for preprocessing and data reduction of frequently arriving sensor data. 

It is a compainon of [DataSeriesPod](https://github.com/matusm/DataSeriesPod) focused on statistic applications. The most important difference is the implementation of variance and standard deviation of the data series. On the other hand time and date related functionality, which is handy for sensor data, is removed in this class.

## Key features

* Written fully in C#
* Clean API
* Lightweight (small memory footprint)
* No additional dependencies
* Multi-platform (Windows, MacOS, Linux) 

### Constructor

* `StatisticPod(string)`
  Creates a new instance of this class taking a name string as the single argument.

### Methods

* `Update(double)`
  Records the passed value. By passing `double.NaN` the call is without effect. 
  
* `Restart()`
  All data recorded so far is discarded to start over. Typically used after consuming the wanted characteristic values of the recording. `Name` is the only property conserved.

### Properties

* `SampleSize`
  Returns the number of samples recorded since the last `Restart()`.

* `AverageValue`
  Returns the arithmetic mean of all values recorded.

* `Variance`
  Returns the variance of all values recorded.

* `StandardDeviation`
  Returns the standard deviation of all values recorded.

* `MinimumValue`
  Returns the smallest value recorded.

* `MaximumValue`
  Returns the largest value recorded.

* `Range`
  Returns the difference between the smallest and largest value recorded.

* `CentralValue`
  Returns the mid-range in the data set. This is the arithmetic mean of the maximum and minimum of all recorded values.

* `FirstValue`
  Returns the first value recorded following a `Restart()`.

* `MostRecentValue`
  Returns the most recent value recorded.

* `Name`
  Returns the name string as provided during creation of the object.

### Notes

All properties are getters only. A `double.NaN` is returned for properties which are (yet) undefined.

Once instantiated, it is not possible to modify the object's name. 
The string provided in the constructor is trimmed and if empty, a generic name is used. 

The data set recorded during the object's life cycle is never accessible; moreover it is not even stored internally. Only the selected characteristic values are accessible through properties.

The arithmetic mean and the variance are computed in a numerically stable way. For details see https://www.johndcook.com/blog/standard_deviation/ and https://diego.assencio.com/?index=c34d06f4f4de2375658ed41f70177d59

### Usage

The following code fragment of a simple program shows the use of this class.
One could leverage `SampleSize` to escape from the loop. 

```cs
using System;
using At.Matus.StatisticPod;

namespace spTest
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var sp = new StatisticPod("Data ID: 314");
            
            // consume data from an array, list, etc.
            // the valueCollection of this snippet must be defined elsewhere
            foreach (var v in valueCollection)
            {
                sp.Update(v);
            }

            Console.WriteLine("Sample size: {0}", sp.SampleSize);
            Console.WriteLine("Mean value: {0}", sp.AverageValue);
            Console.WriteLine(sp); // ToString() is implemented also
            sp.Restart();
            // you can now reuse the object sp
        }
    }
}
```

