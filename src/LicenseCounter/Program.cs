using System;
using System.Reflection;
using InstallationModel;
using ReportLoader;
using SimpleInjector;

namespace LicenseCounter
{
    /// <summary>
    /// Application to calculate the license requirements for a licensing scheme defined as follows:
    /// 
    /// Each copy of the application (ID 374) allows the user to install the application on to two computers if at least one of them is a laptop.
    ///
    /// A report on existing installations, in CSV format, is loaded, parsed and used to calculate license requirements.
    /// 
    /// </summary>
    static class Program
    {
        // There are no unit-tests for this class, but it can be tested by running it, which could be automated into the CI system.
        // If we knew more about the intended use cases, we could get better defaults from a configuration file, if present.
        // Doing that feels like scope creep at this point.
        private const string DefaultFileName = "installation-report.csv";
        private const int FileNameSpecifiedArgumentCount = 1;
        private const int DefaultApplicationId = 374;
        private const int ErrorCode = -1;

        /// <summary>
        /// Load a CSV report file containing information about software installations and calculate license requirements.
        /// Prints the number of licenses required, if they could be calculated. Also returns them as the return code.
        /// Currently only supports one ApplicationID.
        /// Allows the report file name to be specified on the command line.
        /// Uses the basic, built-in .Net command line parser, and supports a single argument, or none, in which case the
        /// default file name is used.
        /// </summary>
        /// <param name="commandlineArguments">If an argument is provided, it specifies the installation report file name.</param>
        /// <returns>An integer indicating the number of licenses required, or a negative value, indicating an error occurred.</returns>
        static int Main(string[] commandlineArguments)
        {
            var fileName = DefaultFileName;

            try
            {
                var container = new Container();
                new InjectionContainerPopulator().Populate(container);

                if (FileNameSpecifiedArgumentCount == commandlineArguments.Length)
                {
                    fileName = commandlineArguments[0];
                }
                // It's reasonable to expect an extension to allow ApplicationID to be specified on command line, but it's not in the requirements.
                // Could handle it here. But it might never happen.
                else if (commandlineArguments.Length > FileNameSpecifiedArgumentCount)
                {
                    PrintUseGuide();
                    return ErrorCode;
                }

                var analyser = container.GetInstance<IReportAnalyser>();

                return DoLicenseReporting(analyser, fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}");
                PrintUseGuide();
                return ErrorCode;
            }
        }

        /// <summary>
        /// Make a filter for the target application and pass it with the fileName into Load + Analyse process.
        /// Print and return the number of required licenses.
        /// Throws exceptions on problems reading input file.
        /// </summary>
        /// <param name="analyser">The IReportAnalyser to use to calculate the license requirements.</param>
        /// <param name="fileName">The filename to load the installation report CSV from.</param>
        /// <returns>The required number of licenses.</returns>
        private static int DoLicenseReporting(IReportAnalyser analyser, string fileName)
        {
            var installationFilter = new ApplicationFilter(DefaultApplicationId);

            var licensesRequired = analyser.LoadAndAnalyse(fileName, installationFilter);

            Console.WriteLine($"{licensesRequired}");

            return licensesRequired;
        }

        private static void PrintUseGuide()
        {
            Console.WriteLine($"Usage:\n  dotnet {Assembly.GetExecutingAssembly().GetName().Name}.dll [<installation-report-file.CSV>]");
            Console.WriteLine("    You must run from the directory where the DLL is located, or use 'dotnet run' from the project directory.");
        }
    }
}
