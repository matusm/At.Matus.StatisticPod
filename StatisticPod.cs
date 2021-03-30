using System;

namespace At.Matus.StatisticPod
{
    public class StatisticPod
    {
        public StatisticPod(string name = noNameSpecified)
        {
            Name = name.Trim();
            if (string.IsNullOrEmpty(Name))
                Name = noNameSpecified;
            Restart();
        }

        public string Name { get; }
        public long SampleSize { get; private set; }
        public double AverageValue { get; private set; }
        public double FirstValue { get; private set; }
        public double MostRecentValue { get; private set; }
        public double MaximumValue { get; private set; }
        public double MinimumValue { get; private set; }
        public double Range => MaximumValue - MinimumValue;
        public double CentralValue => (MaximumValue + MinimumValue) / 2.0;
        public double Variance => (SampleSize > 1) ? squareSum / (SampleSize - 1) : double.NaN;
        public double StandardDeviation => Math.Sqrt(Variance);

        public void Restart()
        {
            SampleSize = 0;
            squareSum = 0.0;
            AverageValue = double.NaN;
            MaximumValue = double.NaN;
            MinimumValue = double.NaN;
            FirstValue = double.NaN;
            MostRecentValue = double.NaN;
        }

        public void Update(double value)
        {
            if (double.IsNaN(value)) return;
            if (SampleSize >= long.MaxValue - 1) return;
            SampleSize++;
            if (SampleSize == 1)
            {
                FirstValue = value;
                AverageValue = value;
                MaximumValue = value;
                MinimumValue = value;
            }
            MostRecentValue = value;
            // https://www.johndcook.com/blog/standard_deviation/
            double oldAverage = AverageValue;
            AverageValue += (value - AverageValue) / SampleSize;
            squareSum += (value - oldAverage) * (value - AverageValue); // TODO only if (SampleSize != 1) ?
            if (value > MaximumValue) MaximumValue = value;
            if (value < MinimumValue) MinimumValue = value;
        }

        public override string ToString()
        {
            if (SampleSize == 0) return $"{Name} : {noDataYet}";
            if (SampleSize == 1) return $"{Name} : {AverageValue}";
            return $"{Name} : {AverageValue} ± {StandardDeviation}";
        }

        private const string noNameSpecified = "<name not specified>";
        private const string noDataYet = "no data yet";

        private double squareSum;

    }

}
