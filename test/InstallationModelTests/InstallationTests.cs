using System.Runtime.CompilerServices;
using InstallationModel;
using NUnit.Framework;

namespace InstallationModelTests
{
    [TestFixture]
    public class InstallationTests
    {
        // Most basic test - check the Installation() default constructor doesn't throw.
        [Test]
        public void installation_can_create()
        {
            var installation = new Installation(0, 0, 0, ComputerType.Desktop);
            Assert.That(installation, Is.Not.Null);
        }

        [Test]
        public void equals__identical_inputs__is_true()
        {
            var lhs = new Installation(3, 7, 11, ComputerType.Desktop);

            var rhs = new Installation(3, 7, 11, ComputerType.Desktop);

            Assert.That(lhs.Equals(rhs));
        }

        [Test]
        public void equals__null_other__is_false()
        {
            var lhs = new Installation(0, 0, 0, ComputerType.Desktop);

            Assert.That(lhs.Equals(null), Is.False);
        }

        [Test]
        public void equals__other_differs_by_computer_id__is_false()
        {
            var lhs = new Installation( 1, 0, 0, ComputerType.Desktop );
            var rhs = new Installation( 2, 0, 0, ComputerType.Desktop);

            Assert.That(lhs.Equals(rhs), Is.False);
        }

        [Test]
        public void equals__other_differs_by_user_id__is_false()
        {
            var lhs = new Installation(0, 1, 0, ComputerType.Desktop);
            var rhs = new Installation(0, 2, 0, ComputerType.Desktop);

            Assert.That(lhs.Equals(rhs), Is.False);
        }

        [Test]
        public void equals__other_differs_by_application_id__is_false()
        {
            var lhs = new Installation(0, 0, 1, ComputerType.Desktop);
            var rhs = new Installation(0, 0, 2, ComputerType.Desktop);

            Assert.That(lhs.Equals(rhs), Is.False);
        }

        [Test]
        public void equals__other_differs_by_computer_type__is_false()
        {
            var lhs = new Installation(0, 0, 0, ComputerType.Desktop);
            var rhs = new Installation(0, 0, 0, ComputerType.Laptop);

            Assert.That(lhs.Equals(rhs), Is.False);
        }

        [Test]
        public void get_hash_code__for_some_installation__is_not_suspiciously_identical_to_object_default()
        {
            // The ID values have no special meaning, just something non-zero to avoid "zero is special" bugs.
            var installation = new Installation(33, 14, 9, ComputerType.Laptop);
            Assert.That(installation.GetHashCode(), Is.Not.EqualTo(RuntimeHelpers.GetHashCode(installation)));
        }

        [Test]
        public void get_hash_code__for_some_installation__is_not_suspiciously_zero()
        {
            var installation = new Installation(1, 2, 3, ComputerType.Laptop);
            Assert.That(installation.GetHashCode(), Is.Not.Zero);
        }

        // It would be *nice* to have a test for distribution of the hash codes, but at this point it looks like the reward vs effort isn't justified.
        // If hashing of the objects proves to be a performance bottleneck, then this would become a different matter.
        // For now, I'll take Resharper on trust that it can make a passable hash for a bunch of integer fields, as it has a fair track record.
    }
}