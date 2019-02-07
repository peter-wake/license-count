using System.Collections.Generic;

namespace InstallationModel
{
    /// <summary>
    /// An interface for a smart container that indexes Installation objects by UserID, to optimize the process
    /// of performing license calculations on a per-use basis.
    /// </summary>
    public interface IInstallationCatalog
    {
        /// <summary>
        /// Add an enumeration of Installations, and index by user.
        /// </summary>
        /// <param name="installationsToAdd">The installations to add.</param>
        void AddInstallationsByUser(IEnumerable<Installation> installationsToAdd);

        /// <summary>
        /// Calculate license requirements on a per-user basis.
        /// </summary>
        /// <param name="installationFilter">A filter to restrict what Installations are of interest.</param>
        /// <param name="licenseAssessor">An assessor to calculate the license requirements for a set of Installation
        /// (in this case belonging to the same user).</param>
        /// <returns>The number of licenses required for the indexed Installation objects.</returns>
        int CountLicensesByUser(IInstallationFilter installationFilter, ILicenseAssessor licenseAssessor);
    }
}