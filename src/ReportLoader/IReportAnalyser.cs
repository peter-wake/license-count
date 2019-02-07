using InstallationModel;

namespace ReportLoader
{
    /// <summary>
    /// An interface for a report analyser.
    /// This is intended to take a file name and an IInstallationFilter and return a license count.
    /// </summary>
    public interface IReportAnalyser
    {
        /// <summary>
        /// Load and analyse an installation report from a file.
        /// </summary>
        /// <param name="fileName">The name of the file to load.</param>
        /// <param name="installationFilter">A filter to determine which Installation items are to be considered.</param>
        /// <returns>The number of licenses required for the installations.</returns>
        int LoadAndAnalyse(string fileName, IInstallationFilter installationFilter);
    }
}