﻿using System;
using System.Collections.Generic;

namespace At.Matus.StatisticPod
{
    public class StatisticPod
    {

        public StatisticPod(string name = "")
        {
            Name = name.Trim();
            if (string.IsNullOrEmpty(Name))
                Name = Guid.NewGuid().ToString();
            Restart();
        }

        public string Name { get; }
        public long SampleSize { get; private set; }
        public double AverageValue { get; private set; }
        public double MaximumValue { get; private set; }
        public double MinimumValue { get; private set; }
        public double Range => MaximumValue - MinimumValue;
        public double CentralValue => (MaximumValue + MinimumValue) / 2.0;
        public double Variance => (SampleSize > 1) ? squareSum / (SampleSize - 1) : double.NaN;
        public double StandardDeviation => Math.Sqrt(Variance);
        public double SquareSum => (SampleSize > 0) ? squareSum : double.NaN;

        public void Restart()
        {
            SampleSize = 0;
            squareSum = 0.0;
            AverageValue = double.NaN;
            MaximumValue = double.NaN;
            MinimumValue = double.NaN;
        }

        public void Update(double value)
        {
            if (double.IsNaN(value)) return;
            if (SampleSize >= long.MaxValue - 1) return;
            SampleSize++;
            if (SampleSize == 1)
            {
                AverageValue = value;
                MaximumValue = value;
                MinimumValue = value;
            }
            // https://www.johndcook.com/blog/standard_deviation/
            double oldAverage = AverageValue;
            AverageValue += (value - AverageValue) / SampleSize;
            squareSum += (value - oldAverage) * (value - AverageValue); // for (SampleSize==1) squareSum=0 
            if (value > MaximumValue) MaximumValue = value;
            if (value < MinimumValue) MinimumValue = value;
        }

        public void Update(double[] values)
        {
            foreach (var value in values)
                Update(value);
        }

        public void Update(List<double> values)
        {
            foreach (var value in values)
                Update(value);
        }

        public override string ToString()
        {
            if (SampleSize == 0) return $"{Name} : {noDataYet}";
            if (SampleSize == 1) return $"{Name} : {AverageValue}";
            return $"{Name} : {AverageValue} ± {StandardDeviation}";
        }

        private const string noDataYet = "no data yet";
        private double squareSum;

    }

}
