using TinyCsvParser.Mapping;

namespace ReportLoader
{
    /// <summary>
    /// Sets up a mapping for reading an installation data report CSV with:
    /// numeric ComputerId, numeric UserId, numeric applicationId, string computerType
    /// So far, the numeric values are small enough to fit in int fields.
    /// </summary>
    public class InstallationDataMapping : CsvMapping<InstallationData>
    {
        /// <summary>
        /// Create a mapping for fields, 0, 1, 2, and 3 of a CSV.
        /// Uses ComputerTypeConverter to map string values to enum ComputerType
        /// </summary>
        public InstallationDataMapping()
        {
            var computerTypeConverter = new ComputerTypeConverter();

            MapProperty(0, ii => ii.ComputerId);
            MapProperty(1, ii => ii.UserId);
            MapProperty(2, ii => ii.ApplicationId);
            MapProperty(3, ii => ii.ComputerType, computerTypeConverter);
        }

        // This is tested implicitly in the ReportLoader CSV parser. It's pretty meaningless until passed into a CsvParser.
    }
}
