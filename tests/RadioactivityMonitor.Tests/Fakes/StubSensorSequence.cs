namespace RadioactivityMonitor.Tests.Fakes
{
    /// <summary>
    /// Stub sensor that returns a sequence of values
    /// </summary>
    public class StubSensorSequence : RadioactivityMonitor.Core.ISensor
    {
        private readonly Queue<double> _values;
        private double _lastValue;

        public StubSensorSequence(params double[] values)
        {
            _values = new Queue<double>(values);
            _lastValue = 0;
        }

        public double NextMeasure()
        {
            if (_values.Count > 0)
            {
                _lastValue = _values.Dequeue();
            }
            return _lastValue;
        }
    }
}
