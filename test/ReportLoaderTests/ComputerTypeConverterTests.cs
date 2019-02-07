using System;
using InstallationModel;
using NUnit.Framework;
using ReportLoader;

namespace ReportLoaderTests
{
    [TestFixture]
    public class ComputerTypeConverterTests
    {
        [Test]
        public void can_create()
        {
            var converter = new ComputerTypeConverter();

            Assert.That(converter, Is.Not.Null);
        }

        [Test]
        public void desktop_input_clean__result__is_desktop()
        {
            var converter = new ComputerTypeConverter();

            const string desktopToken = "desktop";

            converter.TryConvert(desktopToken, out var result);

            Assert.That(result, Is.EqualTo(ComputerType.Desktop));
        }

        [Test]
        public void laptop_input_clean__result__is_laptop()
        {
            var converter = new ComputerTypeConverter();

            const string laptopToken = "laptop";

            converter.TryConvert(laptopToken, out var result);

            Assert.That(result, Is.EqualTo(ComputerType.Laptop));
        }

        [Test]
        public void desktop_input_clean__return__true()
        {
            var converter = new ComputerTypeConverter();

            const string desktopToken = "desktop";

            Assert.That(converter.TryConvert(desktopToken, out var result));
        }

        [Test]
        public void laptop_input_clean__return__true()
        {
            var converter = new ComputerTypeConverter();

            const string laptopToken = "desktop";

            Assert.That(converter.TryConvert(laptopToken, out var result));
        }

        [Test]
        public void strange_input__return__false()
        {
            var converter = new ComputerTypeConverter();

            const string garbageToken = "surface 2";

            Assert.That(converter.TryConvert(garbageToken, out var result), Is.False);
        }

        [Test]
        public void desktop_input_dirty__result__is_desktop()
        {
            var converter = new ComputerTypeConverter();

            const string desktopToken = " DESKTop  ";

            converter.TryConvert(desktopToken, out var result);

            Assert.That(result, Is.EqualTo(ComputerType.Desktop));
        }

        [Test]
        public void laptop_input_dirty__result__is_laptop()
        {
            var converter = new ComputerTypeConverter();

            const string laptopToken = " LaptoP  ";

            converter.TryConvert(laptopToken, out var result);

            Assert.That(result, Is.EqualTo(ComputerType.Laptop));
        }

        [Test]
        public void non_default_conversion__uses_passed_comparator()
        {
            var converter = new ComputerTypeConverter(StringComparison.Ordinal);

            const string laptopToken = "LAPTOP";

            Assert.That(converter.TryConvert(laptopToken, out var result), Is.False);
        }
    }
}
