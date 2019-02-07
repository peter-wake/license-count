using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using InstallationModel;
using NUnit.Framework;

namespace InstallationModelTests
{
    // The text says: "In our scenario, each copy of the application (ID 374) allows the user to install the application on to two computers if at least one of them is a laptop."
    //
    // Apart from the obvious case, of Desktop+Laptop, from this wording, it MUST be valid to install TWO LAPTOPS on a single license, because at least one of them is a laptop.
    //
    // This seems odd enough that I would normally seek clarification of the requirements - as I can't do that - I have to read them literally and conclude the lack of an
    // example for two laptops was a deliberate omission.
    [TestFixture]
    public class LicenseAssessorTests
    {
        [Test]
        public void can_create()
        {
            var assessor = new LicenseAssessor();

            Assert.That(assessor, Is.Not.Null);
        }

        [Test]
        public void assess_installation_licenses__empty_list__returns__zero_licenses()
        {
            var assessor = new LicenseAssessor();

            var installations = new Installation[0];

            var licenseCount = assessor.AssessInstallationLicenses(installations);

            Assert.That(licenseCount, Is.Zero);
        }

        [Test]
        public void assess_installation_licenses__null_list__throws_null_reference_exception()
        {
            var assessor = new LicenseAssessor();

            Assert.Throws<NullReferenceException>(() => assessor.AssessInstallationLicenses(null));
        }

        [Test]
        public void assess_installation_licenses__one_desktop__returns__one_license()
        {
            const int expectedLicenses = 1;

            var assessor = new LicenseAssessor();

            var installations = new[] { new Installation(0, 0, 0, ComputerType.Desktop) };

            var licenseCount = assessor.AssessInstallationLicenses(installations);

            Assert.That(licenseCount, Is.EqualTo(expectedLicenses));
        }

        [Test]
        public void assess_installation_licenses__one_laptop__returns__one_license()
        {
            const int expectedLicenses = 1;

            var assessor = new LicenseAssessor();

            var installations = new[] { new Installation(0, 0, 0, ComputerType.Laptop) };

            var licenseCount = assessor.AssessInstallationLicenses(installations);

            Assert.That(licenseCount, Is.EqualTo(expectedLicenses));
        }

        [Test]
        public void assess_installation_licenses__one_laptop_one_desktop__returns__one_license()
        {
            const int expectedLicenses = 1;

            var assessor = new LicenseAssessor();

            var installations = new[] { new Installation(0, 0, 0, ComputerType.Laptop), new Installation(1, 0, 0, ComputerType.Desktop)  };

            var licenseCount = assessor.AssessInstallationLicenses(installations);

            Assert.That(licenseCount, Is.EqualTo(expectedLicenses));
        }

        [Test]
        public void assess_installation_licenses__two_desktops__returns__two_licenses()
        {
            const int expectedLicenses = 2;

            var assessor = new LicenseAssessor();

            var installations = new[] { new Installation(0, 0, 0, ComputerType.Desktop), new Installation(1, 0, 0, ComputerType.Desktop)  };

            var licenseCount = assessor.AssessInstallationLicenses(installations);

            Assert.That(licenseCount, Is.EqualTo(expectedLicenses));
        }

        [Test]
        public void assess_installation_licenses__two_laptops__returns__one_license()
        {
            const int expectedLicenses = 1;

            var assessor = new LicenseAssessor();

            var installations = new[] { new Installation(0, 0, 0, ComputerType.Laptop), new Installation(1, 0, 0, ComputerType.Laptop) };

            var licenseCount = assessor.AssessInstallationLicenses(installations);

            Assert.That(licenseCount, Is.EqualTo(expectedLicenses));
        }

        [Test]
        public void assess_installation_licenses__three_laptops__returns__two_licenses()
        {
            const int expectedLicenses = 2;

            var assessor = new LicenseAssessor();

            var installations = new[] { new Installation(0, 0, 0, ComputerType.Laptop), new Installation(1, 0, 0, ComputerType.Laptop), new Installation(2, 0, 0, ComputerType.Laptop) };

            var licenseCount = assessor.AssessInstallationLicenses(installations);

            Assert.That(licenseCount, Is.EqualTo(expectedLicenses));
        }

        [Test]
        public void assess_installation_licenses__two_desktops__one_laptop__returns__two_licenses()
        {
            const int expectedLicenses = 2;

            var assessor = new LicenseAssessor();

            var installations = new[] { new Installation(0, 0, 0, ComputerType.Desktop), new Installation(1, 0, 0, ComputerType.Desktop), new Installation(2, 0, 0, ComputerType.Laptop) };

            var licenseCount = assessor.AssessInstallationLicenses(installations);

            Assert.That(licenseCount, Is.EqualTo(expectedLicenses));
        }

        [Test]
        public void assess_installation_licenses__two_laptops__one_desktop__returns__two_licenses()
        {
            const int expectedLicenses = 2;

            var assessor = new LicenseAssessor();

            var installations = new[] { new Installation(0, 0, 0, ComputerType.Desktop), new Installation(1, 0, 0, ComputerType.Desktop), new Installation(2, 0, 0, ComputerType.Laptop) };

            var licenseCount = assessor.AssessInstallationLicenses(installations);

            Assert.That(licenseCount, Is.EqualTo(expectedLicenses));
        }

        [Test]
        public void assess_installation_licenses__ten_laptops__eight_desktops__returns__nine_licenses()
        {
            const int laptopCount = 10;
            const int desktopCount = 8;
            const int expectedLicenses = 9;

            var assessor = new LicenseAssessor();

            var laptops = PopulateArray(laptopCount, ComputerType.Laptop);
            var desktops = PopulateArray(desktopCount, ComputerType.Desktop);

            var installations = laptops.Concat(desktops);

            var licenseCount = assessor.AssessInstallationLicenses(installations);

            Assert.That(licenseCount, Is.EqualTo(expectedLicenses));
        }

        [Test]
        public void assess_installation_licenses__eleven_laptops__eight_desktops__returns__ten_licenses()
        {
            const int laptopCount = 11;
            const int desktopCount = 8;
            const int expectedLicenses = 10;

            var assessor = new LicenseAssessor();

            var laptops = PopulateArray(laptopCount, ComputerType.Laptop);
            var desktops = PopulateArray(desktopCount, ComputerType.Desktop);

            var installations = laptops.Concat(desktops);

            var licenseCount = assessor.AssessInstallationLicenses(installations);

            Assert.That(licenseCount, Is.EqualTo(expectedLicenses));
        }

        [Test]
        public void assess_installation_licenses__eleven_desktops__two_laptops__returns__eleven_licenses()
        {
            const int laptopCount = 2;
            const int desktopCount = 11;
            const int expectedLicenses = 11;

            var assessor = new LicenseAssessor();

            var laptops = PopulateArray(laptopCount, ComputerType.Laptop);
            var desktops = PopulateArray(desktopCount, ComputerType.Desktop);

            var installations = laptops.Concat(desktops);

            var licenseCount = assessor.AssessInstallationLicenses(installations);

            Assert.That(licenseCount, Is.EqualTo(expectedLicenses));
        }

        [Test]
        public void assess_intallation_licenses__enumerates__input_enumeration()
        {
            var assessor = new LicenseAssessor();

            var installations = A.Fake<IEnumerable<Installation>>();

            assessor.AssessInstallationLicenses(installations);

            // This doesn't quite PROVE it was enumerated, but it's good evidence.
            // i.e. We know there are no explicit calls to GetEnumerator.
            A.CallTo(() => installations.GetEnumerator()).MustHaveHappened();
        }


        private Installation[] PopulateArray(int count, ComputerType type)
        {
            var outArray = new Installation[count];

            for(var ii = 0; ii < count; ++ii)
            {
                outArray[ii] = new Installation(ii, 0, 0, type);
            }

            return outArray;
        }
    }
}
