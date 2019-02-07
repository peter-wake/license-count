using System;
using InstallationModel;
using NUnit.Framework;

namespace InstallationModelTests
{
    // As these tests are so trivial, there's currently no strong case for adding an interface for Installation and mocking it.
    // Appropriate partition of functionality makes ApplicationFilter easy to test.
    // Tests with real Installation are easier to write and understand than mocks would be, and aren't bound to anything but Installation constructor.
    [TestFixture]
    public class ApplicationFilterTests
    {
        [Test]
        public void can_create()
        {
            var applicationFilter = new ApplicationFilter(0);

            Assert.That(applicationFilter, Is.Not.Null);
        }

        [Test]
        public void filter_true__for_matching_application_id()
        {
            const int targetApplicationId = 17;
            var applicationFilter = new ApplicationFilter(targetApplicationId);

            var installation = new Installation(0, 0, targetApplicationId, ComputerType.Desktop);

            Assert.That(applicationFilter.Filter(installation));
        }

        [Test]
        public void filter_false__for_non_matching_application_id()
        {
            const int targetApplicationId = 17;
            var applicationFilter = new ApplicationFilter(targetApplicationId);

            var installation = new Installation(0, 0, targetApplicationId + 1, ComputerType.Desktop);

            Assert.That(applicationFilter.Filter(installation), Is.False);
        }

        [Test]
        public void filter__throws_null_reference_exception__for_null_installation()
        {
            const int targetApplicationId = 17;
            var applicationFilter = new ApplicationFilter(targetApplicationId);

            Assert.Throws<NullReferenceException>(() => applicationFilter.Filter(null));
        }

        [Test]
        public void filter_false__for_installations_with_user_and_computer_ids__that_happen_to_match_target_application_id()
        {
            const int targetApplicationId = 17;
            var applicationFilter = new ApplicationFilter(targetApplicationId);

            var installation = new Installation(targetApplicationId, targetApplicationId, 0, ComputerType.Desktop);

            Assert.That(applicationFilter.Filter(installation), Is.False);

        }
    }
}
