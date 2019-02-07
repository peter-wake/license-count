using System.Collections.Generic;
using System.Linq;
using InstallationModel;
using TinyCsvParser.Mapping;

namespace ReportLoader
{
    /// <summary>
    /// Converts the direct output of ReportLoader - tiny CSV objects, reporting load results for each field-set - into Lists of Installation model objects.
    /// </summary>
    public class ReportToModelConverter : IReportToModelConverter
    {
        /// <summary>
        /// Converts a list of CsvMappingResult generics into a list of Installation objects.
        /// Invalid entries are ignored/stripped.
        /// </summary>
        /// <param name="loadResults">The parsed load results and errors.</param>
        /// <returns>The converted list of Installation objects.</returns>
        public List<Installation> ConvertReportData(List<CsvMappingResult<InstallationData>> loadResults)
        {
            var validResults = loadResults.Where(rr => rr.IsValid).Select(rr => rr.Result);

            var installations = validResults.Select(ee => ee.ToInstallation()).ToList();

            return installations;
        }
    }
}
