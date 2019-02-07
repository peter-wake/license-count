using System.Collections.Generic;
using System.Linq;
using InstallationModel;
using NUnit.Framework;
using ReportLoader;
using TinyCsvParser.Mapping;

namespace ReportLoaderTests
{
    // Perhaps more so than in other places, some tests here may seem 'non obvious' in their implementation.
    // The goal is to keep tests from becoming brittle by trying to test explicitly ONLY a single logical post condition in each test.
    // This means the tests are by design, trying to avoid non-essential assertions, and trying to test ONLY what must be true for the particular condition.
    // While this is more effort, it leads to test failures that are more indicative of the test's intended purpose, and fewer failures due to peripherally related causes (brittleness).
    // Because the converter deals intimately with three different data types, it is an area I'd expect to soak maintenance effort in future,
    // as any changes to those classes will likely cause test failures - though they may well be *legitimate* failures.
    // This means the tests, by design, taken *individually*, permit many kinds of output that would be wrong, but *between* them should sufficiently constrain correct behaviour.
    [TestFixture]
    public class ReportToModelConverterTests
    {
        private InstallationData _data0;
        private InstallationData _data1;
        private InstallationData _data2;
        private List<CsvMappingResult<InstallationData>> _allValid;
        private List<CsvMappingResult<InstallationData>> _invalidIndex1;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var mappingError = new CsvMappingError { ColumnIndex = 44, Value = "Test error" };

            _data0 = new InstallationData { ComputerId = 03, UserId = 04, ApplicationId = 05, ComputerType = ComputerType.Laptop };
            _data1 = new InstallationData { ComputerId = 13, UserId = 14, ApplicationId = 15, ComputerType = ComputerType.Desktop };
            _data2 = new InstallationData { ComputerId = 23, UserId = 24, ApplicationId = 25, ComputerType = ComputerType.Laptop };

            var valid0 = new CsvMappingResult<InstallationData> { RowIndex = 0, Result = _data0 };
            var valid1 = new CsvMappingResult<InstallationData> { RowIndex = 1, Result = _data1 };
            var valid2 = new CsvMappingResult<InstallationData> { RowIndex = 2, Result = _data2 };

            // Note Result would likely be null in a real error item, (but possibly not). This wild data allows us to check that IsValid is used during conversion, not (null != Result) which would be wrong.
            var invalid1 = new CsvMappingResult<InstallationData> { RowIndex = 1, Error = mappingError, Result = _data1 };
            

            _allValid = new List<CsvMappingResult<InstallationData>> { valid0, valid1, valid2 };

            _invalidIndex1 = new List<CsvMappingResult<InstallationData>> { valid0, invalid1, valid2 };
        }



        [Test]
        public void can_create()
        {
            var converter = new ReportToModelConverter();

            Assert.That(converter, Is.Not.Null);
        }

        [Test]
        public void convert_report_data__empty_input__returns__empty_output()
        {
            var converter = new ReportToModelConverter();

            var emptyInput = new List<CsvMappingResult<InstallationData>>();

            var output = converter.ConvertReportData(emptyInput);

            Assert.That(output, Is.Not.Null);
            Assert.That(output, Is.Empty);
        }

        [Test]
        public void convert_report_data__all_valid_input__returns__expected_result_count()
        {
            var converter = new ReportToModelConverter();

            var output = converter.ConvertReportData(_allValid);

            Assert.That(output, Is.Not.Null);
            Assert.That(output.Count, Is.EqualTo(_allValid.Count));
        }

        [Test]
        public void convert_report_data__all_valid_input__returns__first_converted_row_as_expected()
        {
            var converter = new ReportToModelConverter();

            var output = converter.ConvertReportData(_allValid);

            Assert.That(output, Is.Not.Null);

            var installation = output.First();
            Assert.That(output.Any());

            Assert.That(installation.ComputerId, Is.EqualTo(_data0.ComputerId));
            Assert.That(installation.UserId, Is.EqualTo(_data0.UserId));
            Assert.That(installation.ApplicationId, Is.EqualTo(_data0.ApplicationId));
            Assert.That(installation.ComputerType, Is.EqualTo(_data0.ComputerType));
        }

        [Test]
        public void convert_report_data__all_valid_input__returns__last_converted_row_as_expected()
        {
            var converter = new ReportToModelConverter();

            var output = converter.ConvertReportData(_allValid);

            Assert.That(output, Is.Not.Null);
            Assert.That(output.Any());

            var installation = output.Last();

            Assert.That(installation.ComputerId, Is.EqualTo(_data2.ComputerId));
            Assert.That(installation.UserId, Is.EqualTo(_data2.UserId));
            Assert.That(installation.ApplicationId, Is.EqualTo(_data2.ApplicationId));
            Assert.That(installation.ComputerType, Is.EqualTo(_data2.ComputerType));
        }

        [Test]
        public void convert_report_data__some_invalid_input__returns__only_valid_items()
        {
            var converter = new ReportToModelConverter();

            var output = converter.ConvertReportData(_invalidIndex1);

            Assert.That(output, Is.Not.Null);
            Assert.That(output.Count, Is.EqualTo(_invalidIndex1.Count - 1));

            Assert.That(output.First().ComputerId, Is.Not.EqualTo(_data1.ComputerId));
            Assert.That(output.Last().ComputerId, Is.Not.EqualTo(_data1.ComputerId));
        }
    }
}
