using RadioactivityMonitor.Core;
using RadioactivityMonitor.Tests.Fakes;
using Xunit;

namespace RadioactivityMonitor.Tests
{
    public class AlarmTests
    {
        // Test constants - matching the Alarm class thresholds
        private const double LowThreshold = 17;
        private const double HighThreshold = 21;

        #region Constructor Tests

        [Fact]
        public void Constructor_WithNullSensor_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Alarm(null!));
        }

        [Fact]
        public void Constructor_WithValidSensor_CreatesInstance()
        {
            // Arrange
            var sensor = new FakeSensor(19.0);

            // Act
            var alarm = new Alarm(sensor);

            // Assert
            Assert.NotNull(alarm);
            Assert.False(alarm.AlarmOn);
        }

        [Fact]
        public void DefaultConstructor_CreatesInstanceWithDefaultSensor()
        {
            // Act
            var alarm = new Alarm();

            // Assert
            Assert.NotNull(alarm);
            Assert.False(alarm.AlarmOn);
        }

        #endregion

        #region Normal Range Tests

        [Fact]
        public void Check_WhenValueInNormalRange_AlarmRemainsOff()
        {
            // Arrange
            var sensor = new FakeSensor(19.0); // Within range [17, 21]
            var alarm = new Alarm(sensor);

            // Act
            alarm.Check();

            // Assert
            Assert.False(alarm.AlarmOn);
            Assert.Equal(0, alarm.AlarmCount);
        }

        [Theory]
        [InlineData(17.0)]  // Lower boundary
        [InlineData(17.5)]
        [InlineData(19.0)]  // Middle of range
        [InlineData(20.5)]
        [InlineData(21.0)]  // Upper boundary
        public void Check_WhenValueAtBoundariesAndWithinRange_AlarmRemainsOff(double value)
        {
            // Arrange
            var sensor = new FakeSensor(value);
            var alarm = new Alarm(sensor);

            // Act
            alarm.Check();

            // Assert
            Assert.False(alarm.AlarmOn);
            Assert.Equal(0, alarm.AlarmCount);
        }

        #endregion

        #region Low Threshold Tests

        [Fact]
        public void Check_WhenValueBelowLowThreshold_AlarmTurnsOn()
        {
            // Arrange
            var sensor = new FakeSensor(16.0); // Below 17
            var alarm = new Alarm(sensor);

            // Act
            alarm.Check();

            // Assert
            Assert.True(alarm.AlarmOn);
            Assert.Equal(1, alarm.AlarmCount);
        }

        [Theory]
        [InlineData(16.99)]
        [InlineData(16.0)]
        [InlineData(10.0)]
        [InlineData(0.0)]
        public void Check_WhenValueBelowLowThreshold_AlarmTurnsOn_MultipleValues(double value)
        {
            // Arrange
            var sensor = new FakeSensor(value);
            var alarm = new Alarm(sensor);

            // Act
            alarm.Check();

            // Assert
            Assert.True(alarm.AlarmOn);
            Assert.Equal(1, alarm.AlarmCount);
        }

        #endregion

        #region High Threshold Tests

        [Fact]
        public void Check_WhenValueAboveHighThreshold_AlarmTurnsOn()
        {
            // Arrange
            var sensor = new FakeSensor(22.0); // Above 21
            var alarm = new Alarm(sensor);

            // Act
            alarm.Check();

            // Assert
            Assert.True(alarm.AlarmOn);
            Assert.Equal(1, alarm.AlarmCount);
        }

        [Theory]
        [InlineData(21.01)]
        [InlineData(22.0)]
        [InlineData(30.0)]
        [InlineData(100.0)]
        public void Check_WhenValueAboveHighThreshold_AlarmTurnsOn_MultipleValues(double value)
        {
            // Arrange
            var sensor = new FakeSensor(value);
            var alarm = new Alarm(sensor);

            // Act
            alarm.Check();

            // Assert
            Assert.True(alarm.AlarmOn);
            Assert.Equal(1, alarm.AlarmCount);
        }

        #endregion

        #region Multiple Check Tests

        [Fact]
        public void Check_CalledMultipleTimesWithNormalValues_AlarmRemainsOff()
        {
            // Arrange
            var sensor = new FakeSensor(19.0);
            var alarm = new Alarm(sensor);

            // Act
            alarm.Check();
            alarm.Check();
            alarm.Check();

            // Assert
            Assert.False(alarm.AlarmOn);
            Assert.Equal(0, alarm.AlarmCount);
        }

        [Fact]
        public void Check_CalledMultipleTimesWithAbnormalValues_IncrementsAlarmCount()
        {
            // Arrange
            var sensor = new FakeSensor(25.0); // Above threshold
            var alarm = new Alarm(sensor);

            // Act
            alarm.Check();
            alarm.Check();
            alarm.Check();

            // Assert
            Assert.True(alarm.AlarmOn);
            Assert.Equal(3, alarm.AlarmCount);
        }

        [Fact]
        public void Check_OnceAlarmIsOn_RemainsOn()
        {
            // Arrange
            var sensorSequence = new StubSensorSequence(25.0, 19.0, 18.0); // First abnormal, then normal
            var alarm = new Alarm(sensorSequence);

            // Act
            alarm.Check(); // Abnormal - turns on
            alarm.Check(); // Normal - but alarm stays on
            alarm.Check(); // Normal - alarm still on

            // Assert
            Assert.True(alarm.AlarmOn);
            Assert.Equal(1, alarm.AlarmCount); // Only incremented once
        }

        [Fact]
        public void Check_WithSequenceOfValues_CorrectlyTracksAlarmCount()
        {
            // Arrange
            var sensorSequence = new StubSensorSequence(
                19.0,  // Normal
                25.0,  // Abnormal - triggers alarm
                19.0,  // Normal - alarm stays on
                15.0,  // Abnormal - increments count
                20.0,  // Normal - alarm stays on
                30.0   // Abnormal - increments count
            );
            var alarm = new Alarm(sensorSequence);

            // Act
            alarm.Check(); // Normal
            alarm.Check(); // Abnormal
            alarm.Check(); // Normal
            alarm.Check(); // Abnormal
            alarm.Check(); // Normal
            alarm.Check(); // Abnormal

            // Assert
            Assert.True(alarm.AlarmOn);
            Assert.Equal(3, alarm.AlarmCount); // Three abnormal readings
        }

        #endregion

        #region Sensor Interaction Tests

        [Fact]
        public void Check_CallsSensorNextMeasureOnce()
        {
            // Arrange
            var spySensor = new SpySensor(19.0);
            var alarm = new Alarm(spySensor);

            // Act
            alarm.Check();

            // Assert
            Assert.Equal(1, spySensor.CallCount);
        }

        [Fact]
        public void Check_CalledMultipleTimes_CallsSensorEachTime()
        {
            // Arrange
            var spySensor = new SpySensor(19.0);
            var alarm = new Alarm(spySensor);

            // Act
            alarm.Check();
            alarm.Check();
            alarm.Check();

            // Assert
            Assert.Equal(3, spySensor.CallCount);
        }

        #endregion

        #region Reset Functionality Tests

        [Fact]
        public void Reset_WhenAlarmIsOn_TurnsAlarmOff()
        {
            // Arrange
            var sensor = new FakeSensor(25.0);
            var alarm = new Alarm(sensor);
            alarm.Check(); // Trigger alarm

            // Act
            alarm.Reset();

            // Assert
            Assert.False(alarm.AlarmOn);
            Assert.Equal(0, alarm.AlarmCount);
        }

        [Fact]
        public void Reset_WhenAlarmIsOff_RemainsOff()
        {
            // Arrange
            var sensor = new FakeSensor(19.0);
            var alarm = new Alarm(sensor);

            // Act
            alarm.Reset();

            // Assert
            Assert.False(alarm.AlarmOn);
            Assert.Equal(0, alarm.AlarmCount);
        }

        [Fact]
        public void Reset_ResetsAlarmCount()
        {
            // Arrange
            var sensor = new FakeSensor(25.0);
            var alarm = new Alarm(sensor);
            alarm.Check();
            alarm.Check();
            alarm.Check();

            // Act
            alarm.Reset();

            // Assert
            Assert.Equal(0, alarm.AlarmCount);
        }

        #endregion

        #region Edge Cases

        [Theory]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        public void Check_WithExtremeValues_HandlesCorrectly(double value)
        {
            // Arrange
            var sensor = new FakeSensor(value);
            var alarm = new Alarm(sensor);

            // Act
            alarm.Check();

            // Assert
            Assert.True(alarm.AlarmOn); // Extreme values should trigger alarm
        }

        [Fact]
        public void Check_WithNegativeValue_TriggersAlarm()
        {
            // Arrange
            var sensor = new FakeSensor(-5.0);
            var alarm = new Alarm(sensor);

            // Act
            alarm.Check();

            // Assert
            Assert.True(alarm.AlarmOn);
        }

        #endregion

        #region Real Sensor Integration Tests

        [Fact]
        public void RealSensor_GeneratesValuesInExpectedRange()
        {
            // Arrange
            var sensor = new Sensor();
            var values = new List<double>();

            // Act - collect 100 samples
            for (int i = 0; i < 100; i++)
            {
                values.Add(sensor.NextMeasure());
            }

            // Assert - all values should be between 16 and 22 (offset 16 + max 6)
            Assert.All(values, v => Assert.InRange(v, 16.0, 22.0));
        }

        [Fact]
        public void Alarm_WithRealSensor_CanTrigger()
        {
            // Arrange
            var alarm = new Alarm(); // Uses real sensor
            bool alarmTriggered = false;

            // Act - check multiple times
            for (int i = 0; i < 1000; i++)
            {
                alarm.Check();
                if (alarm.AlarmOn)
                {
                    alarmTriggered = true;
                    break;
                }
            }

            // Assert - given the random nature, alarm should trigger at some point
            Assert.True(alarmTriggered, "Alarm should trigger with real sensor over many checks");
        }

        #endregion
    }
}
