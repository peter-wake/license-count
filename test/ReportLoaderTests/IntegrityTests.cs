using System.Collections.Generic;
using System.Linq;
using InstallationModel;
using NUnit.Framework;
using ReportLoader;

namespace ReportLoaderTests
{
    // The requirements say that there is no need to consider parsing errors or conflicts in data - data integrity issues...
    //
    // The production code is explicitly kept clean of "out of scope" behaviour.
    // However, it seems prudent to confirm whether there are parse errors loading the test CSVs, or whether there are apparent conflicts.
    //
    // If we are to believe the data is clean, such errors could be canaries for issues like incorrect parsing of computer types or IDs.

    [TestFixture]
    [Ignore("Flexera provided sample data files don't fit in github. Put them in the 'test' directory if you want to enable these tests")]
    public class IntegrityTests
    {
        [Test]
        public void is_small_test_data_parsed_correctly()
        {
            const string fileName = @"..\..\..\..\..\test\sample-small.csv";

            var reportLoader = new ReportLoader.ReportLoader();

            var mappingResults = reportLoader.LoadReportData(fileName);

            Assert.That(mappingResults.Any(rr => !rr.IsValid), Is.False);
        }

        [Test]
        public void is_large_test_data_parsed_correctly()
        {
            const string fileName = @"..\..\..\..\..\test\sample-large.csv";

            var reportLoader = new ReportLoader.ReportLoader();

            var mappingResults = reportLoader.LoadReportData(fileName);

            Assert.That(mappingResults.Any(rr => !rr.IsValid), Is.False);
        }

        [Test]
        public void is_small_test_data_conflict_free()
        {
            const string fileName = @"..\..\..\..\..\test\sample-small.csv";

            var reportLoader = new ReportLoader.ReportLoader();

            var mappingResults = reportLoader.LoadReportData(fileName);

            var reportToModelConverter = new ReportToModelConverter();

            var installations = reportToModelConverter.ConvertReportData(mappingResults);


            var installationIndexMap = new InstallationIndexMap();
            // This is a variant catalog, where we index by ComputerID.
            var installationCatalog = new InstallationCatalogByComputer(installationIndexMap);


            installationCatalog.AddInstallationsByComputer(installations);

            foreach (var installationSet in installationIndexMap.Values)
            {
                // As the set contains only items with the same ComputerId, if we have multiple ComputerTypes, it's a concern.

                if (installationSet.Any())
                {
                    var firstComputerType = installationSet.First().ComputerType;

                    Assert.That(installationSet.Any(ii => ii.ComputerType != firstComputerType), Is.False);
                }
            }
        }


        [Test]
        public void is_large_test_data_conflict_free()
        {
            const string fileName = @"..\..\..\..\..\test\sample-large.csv";

            var reportLoader = new ReportLoader.ReportLoader();

            var mappingResults = reportLoader.LoadReportData(fileName);

            var reportToModelConverter = new ReportToModelConverter();

            var installations = reportToModelConverter.ConvertReportData(mappingResults);


            var installationIndexMap = new InstallationIndexMap();
            // This is a variant catalog, where we index by ComputerID.
            var installationCatalog = new InstallationCatalogByComputer(installationIndexMap);


            installationCatalog.AddInstallationsByComputer(installations);

            foreach (var installationSet in installationIndexMap.Values)
            {
                // As the set contains only items with the same ComputerId, if we have multiple ComputerTypes, it's a concern.

                if (installationSet.Any())
                {
                    var firstComputerType = installationSet.First().ComputerType;

                    Assert.That(installationSet.Any(ii => ii.ComputerType != firstComputerType), Is.False);
                }
            }
        }


        private class InstallationCatalogByComputer
        {
            public InstallationCatalogByComputer(IInstallationIndexMap indexMap)
            {
                _indexMap = indexMap;
            }

            public void AddInstallationsByComputer(IEnumerable<Installation> installationsToAdd)
            {
                foreach (var installation in installationsToAdd)
                {
                    _indexMap.AddInstallation(installation.ComputerId, installation);
                }
            }

            private readonly IInstallationIndexMap _indexMap;
        }
    }
}
