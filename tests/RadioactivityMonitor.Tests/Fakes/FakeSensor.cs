namespace RadioactivityMonitor.Tests.Fakes
{
    /// <summary>
    /// Fake sensor implementation for testing that returns a fixed value
    /// </summary>
    public class FakeSensor : RadioactivityMonitor.Core.ISensor
    {
        private readonly double _fixedValue;

        public FakeSensor(double fixedValue)
        {
            _fixedValue = fixedValue;
        }

        public double NextMeasure()
        {
            return _fixedValue;
        }
    }
}
