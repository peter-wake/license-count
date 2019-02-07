using InstallationModel;

namespace ReportLoader
{
    /// <summary>
    /// A high-level class for installation report analysis.
    /// Can be customised by constructing with different worker objects.
    /// Bundles together an IReportLoader, IReportToModelConverter, IInstallationCatalog and ILicenseAssessor.
    /// Take a file name for a report in CSV format, and outputs a license count.
    /// </summary>
    public class ReportAnalyser : IReportAnalyser
    {
        // There are no unit tests for this file, as they would amount to a re-implementation of it.
        // It's simply too trivial to be worth testing with mocks.
        // However, it *is* tested via functional tests in the FunctionalTests class.

        /// <summary>
        /// Construct a ReportAnalyser that can read a report and count licenses.
        /// </summary>
        /// <param name="reportLoader">An IReportLoader to handle the file loading and parsing.</param>
        /// <param name="reportToModelConverter">An IReportToModelConverter to convert load results into model data.</param>
        /// <param name="installationCatalog">An IInstallationCatalog smart container that can perform efficient licesnse assessments.</param>
        /// <param name="licenseAssessor">An ILicenseAssessor to identify per-user license requirements.</param>
        public ReportAnalyser(IReportLoader reportLoader, IReportToModelConverter reportToModelConverter, IInstallationCatalog installationCatalog, ILicenseAssessor licenseAssessor)
        {
            _reportLoader = reportLoader;
            _reportToModelConverter = reportToModelConverter;
            _installationCatalog = installationCatalog;
            _licenseAssessor = licenseAssessor;
        }

        /// <summary>
        /// Loads the input file, converts to an internal model, and organises the data for efficient analysis.
        /// Restricts installations to consider with an IInstallationFilter and passes the results to an ILicenseAssesor for analysis.
        /// License requirements are summed for the input file and returned.
        /// </summary>
        /// <param name="fileName">The report file to load.</param>
        /// <param name="installationFilter">The filter used to identify installations of interest.</param>
        /// <returns>The summed license requirements.</returns>
        public int LoadAndAnalyse(string fileName, IInstallationFilter installationFilter)
        {
            var installationDataCollection = _reportLoader.LoadReportData(fileName);

            var installations = _reportToModelConverter.ConvertReportData(installationDataCollection);

            _installationCatalog.AddInstallationsByUser(installations);
            var licenseCount = _installationCatalog.CountLicensesByUser(installationFilter, _licenseAssessor);

            return licenseCount;
        }

        private readonly IReportLoader _reportLoader;
        private readonly IReportToModelConverter _reportToModelConverter;
        private readonly IInstallationCatalog _installationCatalog;
        private readonly ILicenseAssessor _licenseAssessor;
    }
}
