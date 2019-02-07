using System.Collections.Generic;

namespace InstallationModel
{
    /// <summary>
    /// Interface for license assessors.
    /// An assessor counts the number of licenses required for an enumeration of Installation objects.
    /// </summary>
    public interface ILicenseAssessor
    {
        /// <summary>
        /// Calculate the license requirements for an enumeration of installations.
        /// </summary>
        /// <param name="installations">The enumeration of Installation to calculate a license requirement count for.</param>
        /// <returns>The number of licenses required for the Installations.</returns>
        int AssessInstallationLicenses(IEnumerable<Installation> installations);
    }
}
