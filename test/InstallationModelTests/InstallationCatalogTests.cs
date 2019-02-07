using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using InstallationModel;
using NUnit.Framework;

namespace InstallationModelTests
{
    [TestFixture]
    public class InstallationCatalogTests
    {
        private const int UserIdA = 0;
        private const int UserIdB = 1;
        private const int UserIdC = 2;

        [SetUp]
        public void SetUp()
        {
            _installA = new Installation(0, UserIdA, 0, ComputerType.Laptop);
            _installB = new Installation(0, UserIdB, 1, ComputerType.Laptop);
            _installC = new Installation(0, UserIdC, 2, ComputerType.Laptop);
        }


        [Test]
        public void can_create()
        {
            // Unlike the self-contained classes in the application, InstallationCatalog is built on IInstallationIndexMap, so we can mock that for testing.
            var mapIndex = A.Fake<IInstallationIndexMap>();
            var catalog = new InstallationCatalog(mapIndex);

            Assert.That(catalog, Is.Not.Null);
        }

        [Test]
        public void add_installations_by_user__adds_passed_installations__with_user_as_index()
        {
            // Use ApplicationID to make Installations unique, instead of ComputerID as in other tests, so we check both scenarios (though it should be irrelevant, which is the point).

            var installations = new[] { _installA, _installB, _installC };

            var mapIndex = A.Fake<IInstallationIndexMap>();
            var catalog = new InstallationCatalog(mapIndex);

            catalog.AddInstallationsByUser(installations);

            A.CallTo(() => mapIndex.AddInstallation(UserIdA, _installA)).MustHaveHappenedOnceExactly();
            A.CallTo(() => mapIndex.AddInstallation(UserIdB, _installB)).MustHaveHappenedOnceExactly();
            A.CallTo(() => mapIndex.AddInstallation(UserIdC, _installC)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void count_licenses_by_user__retrieves__index_map_value()
        {
            var filter = A.Fake<IInstallationFilter>();
            var assessor = A.Fake<ILicenseAssessor>();
            var mapIndex = A.Fake<IInstallationIndexMap>();
            var catalog = new InstallationCatalog(mapIndex);

            // No point testing the result here, it's simply a consequence of default FakeItEasy mock behaviours.
            catalog.CountLicensesByUser(filter, assessor);

            A.CallTo(() => mapIndex.Values).MustHaveHappened();
        }

        [Test]
        public void count_licenses_by_user__filters_each_installation__if_value_set_enumerated()
        {
            var setA = new HashSet<Installation> { _installA };
            var setB = new HashSet<Installation> { _installB };
            var setC = new HashSet<Installation> { _installC };
            var userSets = new[] { setA, setB, setC };

            var filter = A.Fake<IInstallationFilter>();
            var assessor = A.Fake<ILicenseAssessor>();
            var mapIndex = A.Fake<IInstallationIndexMap>();
            var catalog = new InstallationCatalog(mapIndex);

            A.CallTo(() => mapIndex.Values).Returns(userSets);

            // This is required so the enumeration IS evaluated, otherwise it will be lazily ignored, and filters won't be called.
            // Tests for the assessor should show that it WILL be evaluated in a functioning system.
            A.CallTo(() => assessor.AssessInstallationLicenses(A <IEnumerable<Installation>>._))
                .Invokes(callObject => (callObject.Arguments[0] as IEnumerable<Installation>)?.ToList());

            catalog.CountLicensesByUser(filter, assessor);

            A.CallTo(() => filter.Filter(_installA)).MustHaveHappenedOnceExactly();
            A.CallTo(() => filter.Filter(_installB)).MustHaveHappenedOnceExactly();
            A.CallTo(() => filter.Filter(_installC)).MustHaveHappenedOnceExactly();
        }



        [Test]
        public void count_licenses_by_user__calls_assessor__and__sums__assessor_results()
        {
            const int expectedResult = 3; // Should match number of sets returned by mapIndex.Value

            var setA = new HashSet<Installation> { _installA };
            var setB = new HashSet<Installation> { _installB };
            var setC = new HashSet<Installation> { _installC };
            var userSets = new[] { setA, setB, setC };

            var filter = A.Fake<IInstallationFilter>();
            var assessor = A.Fake<ILicenseAssessor>();
            var mapIndex = A.Fake<IInstallationIndexMap>();
            var catalog = new InstallationCatalog(mapIndex);

            A.CallTo(() => mapIndex.Values).Returns(userSets);
            A.CallTo(() => assessor.AssessInstallationLicenses(A<IEnumerable<Installation>>._)).Returns(1);

            var result = catalog.CountLicensesByUser(filter, assessor);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private Installation _installA;
        private Installation _installB;
        private Installation _installC;
    }
}
