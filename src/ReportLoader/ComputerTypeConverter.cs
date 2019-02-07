using System;
using InstallationModel;
using TinyCsvParser.TypeConverter;

namespace ReportLoader
{
    /// <summary>
    /// Converts a string value to a ComputerType
    /// </summary>
    public class ComputerTypeConverter : NonNullableConverter<ComputerType>
    {

        /// <summary>
        /// Creates a default converter, for string to ComputerType conversions.
        /// Defaults to using OrdinalIgnoreCase as the comparator.
        /// Ordinal may not be sufficient to match some types of files, but we have *yet* to see such files.
        /// For example, unicode CSV may contain characters that are mappable to ANSI, and visually indistinguishable from their ANSI equivalents on most displays, but are not the same codes as ANSI.
        /// </summary>
        public ComputerTypeConverter() : this(StringComparison.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Construct a ComputerTypeConverter. Takes a string comparator.
        /// </summary>
        /// <param name="stringComparison">The string comparator to use for conversion/parsing; should be case insensitive for reliable operation.</param>
        public ComputerTypeConverter(StringComparison stringComparison)
        {
            _desktopValue = nameof(ComputerType.Desktop);
            _laptopValue = nameof(ComputerType.Laptop);
            _stringComparison = stringComparison;
        }

        /// <summary>
        /// Internals of TryConvert(string value, out ComputerType result) method.
        /// Due to design of the NonNullableConverter, overriding this method is the correct way to implement TryConvert
        /// </summary>
        /// <param name="value">A string value to parse/convert.</param>
        /// <param name="result">out ComputerType will have the result of the parse/convert, if there is one, otherwise defaults to Desktop.</param>
        /// <returns>True if the input was a valid ComputerType string, false if not.</returns>
        protected override bool InternalConvert(string value, out ComputerType result)
        {
            result = ComputerType.Desktop;

            var trimmedValue = value.Trim();

            if (string.Equals(_desktopValue, trimmedValue, _stringComparison))
            {
                return true;
            }

            if (string.Equals(_laptopValue, trimmedValue, _stringComparison))
            {
                result = ComputerType.Laptop;
                return true;
            }

            return false;
        }

        private readonly string _desktopValue;
        private readonly string _laptopValue;
        private readonly StringComparison _stringComparison;
    }
}
