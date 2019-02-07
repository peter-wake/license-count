using System.IO;
using System.Linq;
using InstallationModel;
using NUnit.Framework;

namespace ReportLoaderTests
{
    // There are tests here for corrupted fields, even though the requirements say they will not have to be "considered".
    //
    // It's simply nonsensical to try and write a test suite that thoroughly defines correct behaviour, with single-purpose tests, and then leave a giant hole in the post-conditions,
    // by not testing for read errors. This would leave a substantial chunk of how we handle TinyCSV behaviour unchecked.
    //
    // However, there is no 'production' code for handling errors other than ignoring them; though the handling of errors would probably be
    // a substantial part of a 'real' implementation.
    [TestFixture]
    public class ReportLoaderTests
    {
        private const string TestFilePath = @".\basic-test.csv";
        private const string TestFilePathBadComputerId = @".\bad-field-0.csv";
        private const string TestFilePathBadUserId = @".\bad-field-1.csv";
        private const string TestFilePathBadApplicationId = @".\bad-field-2.csv";
        private const string TestFilePathBadComputerType = @".\bad-field-3.csv";

        [Test]
        public void can_create()
        {
            var reportLoader = new ReportLoader.ReportLoader();

            Assert.That(reportLoader, Is.Not.Null);
        }

        [Test]
        public void load_report_data__can_parse_csv__no_exception()
        {
            var reportLoader = new ReportLoader.ReportLoader();

            reportLoader.LoadReportData(TestFilePath);
        }

        [Test]
        public void load_report_data__no_file__throws()
        {
            var reportLoader = new ReportLoader.ReportLoader();

            Assert.Throws<FileNotFoundException>(() => reportLoader.LoadReportData("no-such-file"));
        }

        [Test]
        public void load_report_data__parse_csv__all_valid__returns_expected_result_count()
        {
            const int expectedRowCount = 3;

            var reportLoader = new ReportLoader.ReportLoader();

            var readResults = reportLoader.LoadReportData(TestFilePath);

            Assert.That(readResults, Is.Not.Null);
            Assert.That(readResults.Count, Is.EqualTo(expectedRowCount));
        }

        [Test]
        public void load_report_data__parse_csv__all_valid__returns_all_valid_objects()
        {
            var reportLoader = new ReportLoader.ReportLoader();

            var readResults = reportLoader.LoadReportData(TestFilePath);

            Assert.That(readResults, Is.Not.Null);
            Assert.That(readResults.Any());

            int index = 0;
            foreach (var csvMappingResult in readResults)
            {
                Assert.That(csvMappingResult.IsValid, $"Parsed entry {index} not valid");
                Assert.That(csvMappingResult.Result, Is.Not.Null, $"Parsed entry {index} has a null data object");
                index += 1;
            }
        }

        [Test]
        public void load_report_data__parse_csv__all_valid__first_object_as_expected()
        {
            // Expecting: 1,1091,606,DESKTOP,...
            const int expectedComputerId = 1;
            const int expectedUserId = 1091;
            const int expectedApplicationId = 606;
            const ComputerType expectedComputerType = ComputerType.Desktop;


            var reportLoader = new ReportLoader.ReportLoader();

            var readResults = reportLoader.LoadReportData(TestFilePath);

            Assert.That(readResults, Is.Not.Null);
            Assert.That(readResults.Any());

            var data = readResults.First().Result;

            Assert.That(data.ComputerId, Is.EqualTo(expectedComputerId));
            Assert.That(data.UserId, Is.EqualTo(expectedUserId));
            Assert.That(data.ApplicationId, Is.EqualTo(expectedApplicationId));
            Assert.That(data.ComputerType, Is.EqualTo(expectedComputerType));
        }

        [Test]
        public void load_report_data__parse_csv__all_valid__last_object_as_expected()
        {
            // Expecting: 3,4380,188,Laptop,...
            const int expectedComputerId = 3;
            const int expectedUserId = 4380;
            const int expectedApplicationId = 188;
            const ComputerType expectedComputerType = ComputerType.Laptop;

            var reportLoader = new ReportLoader.ReportLoader();

            var readResults = reportLoader.LoadReportData(TestFilePath);

            Assert.That(readResults, Is.Not.Null);
            Assert.That(readResults.Any());

            var data = readResults.Last().Result;

            Assert.That(data.ComputerId, Is.EqualTo(expectedComputerId));
            Assert.That(data.UserId, Is.EqualTo(expectedUserId));
            Assert.That(data.ApplicationId, Is.EqualTo(expectedApplicationId));
            Assert.That(data.ComputerType, Is.EqualTo(expectedComputerType));
        }

        [Test]
        public void load_report_data__parse_csv__bad_computer_id__row_invalid()
        {
            var reportLoader = new ReportLoader.ReportLoader();

            var readResults = reportLoader.LoadReportData(TestFilePathBadComputerId);

            Assert.That(readResults, Is.Not.Null);
            Assert.That(readResults.Any());

            Assert.That(readResults.Last().IsValid, Is.False);
        }

        [Test]
        public void load_report_data__parse_csv__bad_user_id__row_invalid()
        {
            var reportLoader = new ReportLoader.ReportLoader();

            var readResults = reportLoader.LoadReportData(TestFilePathBadUserId);

            Assert.That(readResults, Is.Not.Null);
            Assert.That(readResults.Any());

            Assert.That(readResults.Last().IsValid, Is.False);
        }

        [Test]
        public void load_report_data__parse_csv__bad_application_id__row_invalid()
        {
            var reportLoader = new ReportLoader.ReportLoader();

            var readResults = reportLoader.LoadReportData(TestFilePathBadApplicationId);

            Assert.That(readResults, Is.Not.Null);
            Assert.That(readResults.Any());

            Assert.That(readResults.Last().IsValid, Is.False);
        }

        [Test]
        public void load_report_data__parse_csv__bad_computer_type__row_invalid()
        {
            var reportLoader = new ReportLoader.ReportLoader();

            var readResults = reportLoader.LoadReportData(TestFilePathBadComputerType);

            Assert.That(readResults, Is.Not.Null);
            Assert.That(readResults.Any());

            Assert.That(readResults.Last().IsValid, Is.False);
        }

    }
}
