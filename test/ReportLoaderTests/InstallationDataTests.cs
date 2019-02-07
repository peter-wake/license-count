using InstallationModel;
using NUnit.Framework;
using ReportLoader;

namespace ReportLoaderTests
{
    [TestFixture]
    public class InstallationDataTests
    {
        [Test]
        public void can_create()
        {
            var installationData = new InstallationData();
            Assert.That(installationData, Is.Not.Null);
        }

        [Test]
        public void can_convert__to_installation()
        {
            var installationData = new InstallationData();

            var installation = installationData.ToInstallation();

            // ReSharper disable once IsExpressionAlwaysTrue
            Assert.That(installation is Installation, Is.Not.Null);
        }

        [Test]
        public void convert_to_installation__preserves_computer_id()
        {
            const int computerId = 3;

            var installationData = new InstallationData { ComputerId = computerId };

            var installation = installationData.ToInstallation();

            Assert.That(installation.ComputerId, Is.EqualTo(computerId));
        }

        [Test]
        public void convert_to_installation__preserves_user_id()
        {
            const int userId = 7;

            var installationData = new InstallationData { UserId = userId };

            var installation = installationData.ToInstallation();

            Assert.That(installation.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void convert_to_installation__preserves_application_id()
        {
            const int applicationId = 11;

            var installationData = new InstallationData { ApplicationId = applicationId };

            var installation = installationData.ToInstallation();

            Assert.That(installation.ApplicationId, Is.EqualTo(applicationId));
        }

        [Test]
        public void convert_to_installation__preserves_computer_type()
        {
            const ComputerType computerType = ComputerType.Laptop;

            var installationData = new InstallationData { ComputerType = computerType };

            var installation = installationData.ToInstallation();

            Assert.That(installation.ComputerType, Is.EqualTo(computerType));
        }

        [Test]
        public void convert_to_installation__combined_properties_correct()
        {
            const int computerId = 1;
            const int userId = 2;
            const int applicationId = 3;
            const ComputerType computerType = ComputerType.Desktop;

            var installationData =
                new InstallationData { ComputerId = computerId, UserId = userId, ApplicationId = applicationId, ComputerType = computerType };

            var installation = installationData.ToInstallation();

            Assert.That(installation.ComputerId, Is.EqualTo(computerId));
            Assert.That(installation.UserId, Is.EqualTo(userId));
            Assert.That(installation.ApplicationId, Is.EqualTo(applicationId));
            Assert.That(installation.ComputerType, Is.EqualTo(computerType));
        }
    }
}