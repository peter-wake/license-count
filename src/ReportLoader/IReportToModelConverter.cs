using System.Collections.Generic;
using InstallationModel;
using TinyCsvParser.Mapping;

namespace ReportLoader
{
    /// <summary>
    /// An interface for a class that extracts (only) the data from CSV parse results,
    /// converts the data objects into the model representation: Installation - and returns an ordered list of Installation objects.
    /// </summary>
    public interface IReportToModelConverter
    {
        /// <summary>
        /// Convert parsing results into model objects for Installations.
        /// Invalid entries are simply ignored.
        /// (Run a separate check to identify parse errors).
        /// </summary>
        /// <param name="loadResults">The parsing results to convert.</param>
        /// <returns>An ordered list of Installation converted from the valid (only) parse data.</returns>
        List<Installation> ConvertReportData(List<CsvMappingResult<InstallationData>> loadResults);
    }
}