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
        public double Variance => 0;
        public double StandardDeviation => Math.Sqrt(Variance);

        public void Restart()
        {
            SampleSize = 0;
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
            // https://diego.assencio.com/?index=c34d06f4f4de2375658ed41f70177d59
            // https://www.johndcook.com/blog/standard_deviation/
            AverageValue += (value - AverageValue) / SampleSize;
            if (value > MaximumValue) MaximumValue = value;
            if (value < MinimumValue) MinimumValue = value;
        }

        //TODO modify for StandardDeviation
        public override string ToString() => SampleSize > 0
            ? $"{Name} : {AverageValue} ± {StandardDeviation}"
            : $"{Name} : {noDataYet}";

        private const string noNameSpecified = "<name not specified>";
        private const string noDataYet = "no data yet";

    }

}
