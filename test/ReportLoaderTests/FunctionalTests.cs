using InstallationModel;
using NUnit.Framework;
using ReportLoader;
using SimpleInjector;

namespace ReportLoaderTests
{
    /// <summary>
    /// These tests aren't simple unit tests, instead they build complete object assemblages and run them.
    /// </summary>
    [TestFixture]
    public class FunctionalTests
    {
        private const int DefaultApplicationId = 374;

        [Test]
        public void RunWithTinyData()
        {
            const string fileName = @"..\..\..\..\..\test\installation-report.csv";

            // I only believe this to be 190 because I got this result when running the test - wouldn't want it to change unexpectedly though.
            const int expectedLicenseCount = 11;

            var container = new Container();
            PopulateContainer(container);

            var analyser = container.GetInstance<IReportAnalyser>();

            var licensesRequired = DoLicenseReporting(analyser, fileName);

            Assert.That(licensesRequired, Is.EqualTo(expectedLicenseCount));
        }


        [Test]
        [Ignore("Flexera provided sample data files don't fit in github. Put them in the 'test' directory if you want to enable these tests")]
        public void RunWithSmallData()
        {
            const string fileName = @"..\..\..\..\..\test\sample-small.csv";

            // I only believe this to be 190 because I got this result when running the test - wouldn't want it to change unexpectedly though.
            const int expectedLicenseCount = 190;

            var container = new Container();
            PopulateContainer(container);

            var analyser = container.GetInstance<IReportAnalyser>();

            var licensesRequired = DoLicenseReporting(analyser, fileName);

            Assert.That(licensesRequired, Is.EqualTo(expectedLicenseCount));
        }

        // Takes around 30 seconds with .net core release version - over 20 seconds of this time is taken by CSV loading/parsing.
        [Test]
        [Ignore("Flexera provided sample data files don't fit in github. Put them in the 'test' directory if you want to enable these tests")]
        public void RunWithLargeData()
        {
            const string fileName = @"..\..\..\..\..\test\sample-large.csv";

            // I only believe this to be 13927 because I got this result when running the test - wouldn't want it to change unexpectedly though.
            const int expectedLicenseCount = 13927;

            var container = new Container();
            PopulateContainer(container);

            var analyser = container.GetInstance<IReportAnalyser>();

            var licensesRequired = DoLicenseReporting(analyser, fileName);

            Assert.That(licensesRequired, Is.EqualTo(expectedLicenseCount));
        }




        private static int DoLicenseReporting(IReportAnalyser analyser, string fileName)
        {
            var installationFilter = new ApplicationFilter(DefaultApplicationId);

            var licensesRequired = analyser.LoadAndAnalyse(fileName, installationFilter);

            return licensesRequired;
        }


        private void PopulateContainer(Container container)
        {
            container.Register<IInstallationIndexMap, InstallationIndexMap>();
            container.Register<ILicenseAssessor, LicenseAssessor>();
            container.Register<IInstallationCatalog, InstallationCatalog>();

            container.Register<IReportAnalyser, ReportAnalyser>();
            container.Register<IReportLoader, ReportLoader.ReportLoader>();
            container.Register<IReportToModelConverter, ReportToModelConverter>();

            container.Verify();
        }
    }
}
