namespace RadioactivityMonitor.Tests.Fakes
{
    /// <summary>
    /// Spy sensor implementation that tracks the number of calls
    /// </summary>
    public class SpySensor : RadioactivityMonitor.Core.ISensor
    {
        private readonly double _returnValue;
        public int CallCount { get; private set; }

        public SpySensor(double returnValue)
        {
            _returnValue = returnValue;
            CallCount = 0;
        }

        public double NextMeasure()
        {
            CallCount++;
            return _returnValue;
        }
    }
}
