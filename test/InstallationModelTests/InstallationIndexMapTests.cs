using System.Collections.Generic;
using System.Linq;
using InstallationModel;
using NUnit.Framework;

namespace InstallationModelTests
{
    // In this case TDD aids in partitioning the code because naturally testable blocks fall out.
    // It's not worth over-testing this, as it is doesn't add much functionality to Dictionary, just the add or create for the contained sets.
    //
    // Note: all tests use UserID, because at this point, it doesn't matter if this fails with other fields...
    // (Though looking at the implementation, it's self-evident it won't, as it simply uses whatever you pass as the index, 
    // and Installation is tested to be unique considering all fields, so the resulting sets are index agnostic.
    //
    [TestFixture]
    public class InstallationIndexMapTests
    {
        private const int UserIdA = 0;
        private const int UserIdB = 1;
        private const int UserIdC = 2;
        private const int UserIdD = 4;

        [Test]
        public void can_create()
        {
            var indexMap = new InstallationIndexMap();

            Assert.That(indexMap, Is.Not.Null);
        }

        [Test]
        public void add_installation__number_of_index_entries_presented__matches__number_of_value_sets_returned()
        {
            // No index is lost or gained.
            // The count of returned groups should match the number of unique index values presented in Installations.
            // The computerID is used here to make the Installation items unique.

            var indexMap = new InstallationIndexMap();

            const int uniqueUserIdCount = 3;
            indexMap.AddInstallation(UserIdA, new Installation(0, UserIdA, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdA, new Installation(1, UserIdA, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdB, new Installation(2, UserIdB, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdC, new Installation(2, UserIdC, 0, ComputerType.Desktop));

            Assert.That(indexMap.Values.Count(), Is.EqualTo(uniqueUserIdCount));
        }

        [Test]
        public void add_installation__entries_in_group__all_have_same_index()
        {
            // Group members are determined by index value.
            // All entries in a group should have the same index value.
            // The computerID is used here to make the Installation items unique.
            var indexMap = new InstallationIndexMap();

            indexMap.AddInstallation(UserIdA, new Installation(0, UserIdA, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdA, new Installation(1, UserIdA, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdB, new Installation(2, UserIdB, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdB, new Installation(3, UserIdB, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdB, new Installation(4, UserIdB, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdC, new Installation(5, UserIdC, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdC, new Installation(6, UserIdC, 0, ComputerType.Desktop));

            foreach (var installationSet in indexMap.Values)
            {
                var userId = installationSet.First().UserId;
                Assert.That(installationSet.Any(ii => ii.UserId != userId), Is.False, "Found deviant UserID, doesn't match first in set.");
            }
        }

        [Test]
        public void add_installation__returned_groups__have_unique_index_values()
        {
            // Group indexes are unique.
            // There shouldn't be two or more returned groups with the same index value.
            // The computerID is used here to make the Installation items unique.
            var indexMap = new InstallationIndexMap();

            indexMap.AddInstallation(UserIdA, new Installation(0, UserIdA, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdA, new Installation(1, UserIdA, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdB, new Installation(2, UserIdB, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdB, new Installation(3, UserIdB, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdB, new Installation(4, UserIdB, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdC, new Installation(5, UserIdC, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdC, new Installation(6, UserIdC, 0, ComputerType.Desktop));
            indexMap.AddInstallation(UserIdD, new Installation(7, UserIdD, 0, ComputerType.Desktop));

            var expectedIds = new[] { UserIdA, UserIdB, UserIdC, UserIdD };

            Assert.That(indexMap.Values.Select(gg => gg.First().UserId), Is.EquivalentTo(expectedIds));
        }

        [Test]
        public void add_installation__set_members__are_equivalent_to__unique_presented_installations()
        {
            // No installation is lost or gained.
            // The union of all group members should match the union of all submitted Installation.
            // The computerID is used here to make the Installation items unique.
            var indexMap = new InstallationIndexMap();

            var installations = new[]
            {
                new Installation(0, UserIdA, 0, ComputerType.Desktop),
                new Installation(1, UserIdA, 0, ComputerType.Desktop),
                new Installation(2, UserIdB, 0, ComputerType.Desktop),
                new Installation(3, UserIdB, 0, ComputerType.Desktop),
                new Installation(4, UserIdB, 0, ComputerType.Desktop),
                new Installation(5, UserIdC, 0, ComputerType.Desktop),
                new Installation(6, UserIdC, 0, ComputerType.Desktop),
                new Installation(7, UserIdD, 0, ComputerType.Desktop),
                // Duplicate of above
                new Installation(0, UserIdA, 0, ComputerType.Desktop),
                new Installation(1, UserIdA, 0, ComputerType.Desktop),
                new Installation(2, UserIdB, 0, ComputerType.Desktop),
                new Installation(3, UserIdB, 0, ComputerType.Desktop),
                new Installation(4, UserIdB, 0, ComputerType.Desktop),
                new Installation(5, UserIdC, 0, ComputerType.Desktop),
                new Installation(6, UserIdC, 0, ComputerType.Desktop),
                new Installation(7, UserIdD, 0, ComputerType.Desktop)
            };

            foreach (var installation in installations)
            {
                indexMap.AddInstallation(installation.UserId, installation);
            }

            var retrievedUniqueInstallations = new HashSet<Installation>();
            foreach (var installationSet in indexMap.Values)
            {
                retrievedUniqueInstallations.UnionWith(installationSet);
            }

            var expectedSet = new HashSet<Installation>();
            expectedSet.UnionWith(installations);

            Assert.That(retrievedUniqueInstallations, Is.EquivalentTo(expectedSet));
        }
    }
}
