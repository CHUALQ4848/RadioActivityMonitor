using System;

namespace RadioactivityMonitor.Core
{
    public class Sensor : ISensor
    {
        const double Offset = 16;

        public double NextMeasure()
        {
            double value;
            SampleMeasure(out value);

            return Offset + value;
        }

        private static void SampleMeasure(out double measure)
        {
            // placeholder implementation that simulate a real sensor in a real
            // power plant
            Random numberGenerator = new Random();
            measure = 6 * numberGenerator.NextDouble() * numberGenerator.NextDouble();
        }
    }
}
