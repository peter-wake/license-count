using InstallationModel;

namespace ReportLoader
{
    /// <summary>
    /// A data-only object for Installation report parsing.
    /// This represents the values found in the CSV.
    /// The Comment property is not stored, as it is not of interest at this time.
    /// </summary>
    public class InstallationData
    {
        public int ComputerId { get; set; }

        public int UserId { get; set; }

        public int ApplicationId { get; set; }

        public ComputerType ComputerType { get; set; }

        public Installation ToInstallation()
        {
            return new Installation(ComputerId, UserId, ApplicationId, ComputerType);
        }

        // The Comment property isn't mapped, as it has no bearing on the problem requirements.
        // It is currently a good example of YAGNI (you aint gonna need it).
        // If needed at a later date, it could be added - and analysis of correct handling performed.
        // There are non-trivial questions on how to handle comments in the case of duplicate records, equivalent in everything but the comment field.
    }
}
