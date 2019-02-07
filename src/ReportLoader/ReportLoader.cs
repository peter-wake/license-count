using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace ReportLoader
{
    /// <summary>
    /// Load a CSV installation report file and parse to InstallationData data-only objects.
    /// </summary>
    public class ReportLoader : IReportLoader
    {
        // There is no evidence that we will ever need different separator or encoding; don't add functionality until needed.
        private const bool SkipHeaderLine = true;
        private const char DefaultSeparator = ',';
        private static readonly Encoding DefaultEncoding = Encoding.ASCII;

        /// <summary>
        /// Create a new ReportLoader - a wrapper around a CSV parser.
        /// It can load a CSV, and convert the CSV intermediate data objects into model objects - a list of Installation objects.
        /// </summary>
        public ReportLoader()
        {
            var parserOptions = new CsvParserOptions(SkipHeaderLine, DefaultSeparator);
            var mapping = new InstallationDataMapping();
            _parser = new CsvParser<InstallationData>(parserOptions, mapping);
        }

        /// <summary>
        /// Read a CSV file and return the parsed results.
        /// </summary>
        /// <param name="fileName">The pathname to read the CSV file from.</param>
        /// <returns>Parsed objects and read errors as a list of CsvMappingResult.</returns>
        public List<CsvMappingResult<InstallationData>> LoadReportData(string fileName)
        {
            var result =  _parser.ReadFromFile(fileName, DefaultEncoding).ToList();

            return result;
        }


        private readonly CsvParser<InstallationData> _parser;
    }
}
