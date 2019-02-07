using System.Collections.Generic;
using TinyCsvParser.Mapping;

namespace ReportLoader
{
    /// <summary>
    /// An interface for a CSV file loader.
    /// Loads a file by name and returns an ordered list of parsing results.
    /// </summary>
    public interface IReportLoader
    {
        /// <summary>
        /// Load and parse a CSV file.
        /// </summary>
        /// <param name="fileName">The path to the file to be parsed.</param>
        /// <returns>
        /// A list of record-by-record results.
        /// Theoretically, records may span lines, so these don't have a direct correlation to lines, though in practice they usually do.
        /// </returns>
        List<CsvMappingResult<InstallationData>> LoadReportData(string fileName);
    }
}