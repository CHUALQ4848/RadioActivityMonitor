namespace RadioactivityMonitor.Core
{
    /// <summary>
    /// Alarm class to monitor radioactivity levels in a nuclear power plant
    /// </summary>
    public class Alarm
    {
        private const double LowThreshold = 17;
        private const double HighThreshold = 21;

        private readonly ISensor _sensor;
        private bool _alarmOn = false;
        private long _alarmCount = 0;

        /// <summary>
        /// Default constructor using real Sensor
        /// </summary>
        public Alarm() : this(new Sensor())
        {
        }

        /// <summary>
        /// Constructor with dependency injection for sensor
        /// </summary>
        /// <param name="sensor">The sensor to use for measurements</param>
        public Alarm(ISensor sensor)
        {
            _sensor = sensor ?? throw new ArgumentNullException(nameof(sensor));
        }

       

        /// <summary>
        /// Checks the current radioactivity level and triggers alarm if out of range
        /// </summary>
        public void Check()
        {
            double value  = _sensor.NextMeasure();

            // Fixed bug: Changed | (bitwise OR) to || (logical OR)
            if (value < LowThreshold || value > HighThreshold)
            {
                _alarmOn = true;
                _alarmCount += 1;
            }
        }

        /// <summary>
        /// Gets whether the alarm is currently triggered
        /// </summary>
        public bool AlarmOn
        {
            get { return _alarmOn; }
        }

        /// <summary>
        /// Gets the number of times the alarm has been triggered (NEW)
        /// </summary>
        public long AlarmCount
        {
            get { return _alarmCount; }
        }

        /// <summary>
        /// Resets the alarm state (NEW)
        /// </summary>
        public void Reset()
        {
            _alarmOn = false;
            _alarmCount = 0;
        }
    }
}
