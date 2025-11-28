namespace RadioactivityMonitor.Core
{
    /// <summary>
    /// Interface for radioactivity sensor measurements
    /// </summary>
    public interface ISensor
    {
        /// <summary>
        /// Gets the next radioactivity measurement
        /// </summary>
        /// <returns>The measured radioactivity value</returns>
        double NextMeasure();
    }
}
