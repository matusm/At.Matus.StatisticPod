
using System;

namespace At.Matus.StatisticPod
{
    public class CovariancePod
    {
        private readonly StatisticPod xPod = new StatisticPod("x");
        private readonly StatisticPod yPod = new StatisticPod("y");
        private readonly StatisticPod zPod = new StatisticPod("x+y");

        public CovariancePod(string name = "")
        {
            Name = name.Trim();
            if (string.IsNullOrEmpty(Name))
                Name = Guid.NewGuid().ToString();
            Restart();
        }

        public string Name { get; }
        public long SampleSize => zPod.SampleSize;
        public double AverageValueOfX => xPod.AverageValue;
        public double AverageValueOfY => yPod.AverageValue;
        public double StandardDeviationOfX => xPod.StandardDeviation;
        public double StandardDeviationOfY => yPod.StandardDeviation;
        public double Covariance => (zPod.Variance - xPod.Variance - yPod.Variance) / 2;
        public double CorrelationCoefficient => Covariance / (xPod.StandardDeviation * yPod.StandardDeviation);

        // to expose all properties of the StatisticPod objects
        public StatisticPod XPod => xPod;
        public StatisticPod YPod => yPod;

        public void Restart()
        {
            xPod.Restart();
            yPod.Restart();
            zPod.Restart();
        }

        public void Update(double x, double y)
        {
            if (double.IsNaN(x) || double.IsNaN(y))
                return; // forces all three pods are of equal sample size
            xPod.Update(x);
            yPod.Update(y);
            zPod.Update(x + y);
        }

        public override string ToString()
        {
            if (SampleSize == 0) return $"{Name} : {noDataYet}";
            if (SampleSize == 1) return $"{Name} : ({AverageValueOfX}, {AverageValueOfY})";
            return $"{Name} : ({AverageValueOfX} ± {StandardDeviationOfX}, {AverageValueOfY} ± {StandardDeviationOfY}) r={CorrelationCoefficient}";
        }

        private const string noDataYet = "no data yet";
    }
}
