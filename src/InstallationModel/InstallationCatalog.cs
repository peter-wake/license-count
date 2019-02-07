using System.Collections.Generic;
using System.Linq;

namespace InstallationModel
{
    /// <summary>
    /// A smart container for installations that allows installation-use queries to be performed with reasonable efficiency.
    /// Installations reside in a dictionary that indexes them by UserId - which appears to be the sole licensing mechanism
    /// for the product(s) we are interested in.
    ///
    /// While it smells a bit of YAGNI, I believe the "future" requirement to test against more than one application ID is
    /// so glaringly obvious, that we MUST assume it will be requested. A pure extreme programming style approach would
    /// say not to consider this, but sometimes a LITTLE pragmatism is called for.
    ///
    /// It's tempting to make this flexible using delegates, but it's not needed - keep it simple for now.
    /// </summary>
    public class InstallationCatalog : IInstallationCatalog
    {
        /// <summary>
        /// Construct a new InstallationCatalog with an index mapper that will group the Installations - by user in this case.
        /// </summary>
        /// <param name="indexMap">An index mapper to group installations by user.</param>
        public InstallationCatalog(IInstallationIndexMap indexMap)
        {
            _indexMap = indexMap;
        }

        /// <summary>
        /// Add a collection of Installation to the catalog.
        /// The Installation objects will be grouped by UserId.
        /// </summary>
        /// <param name="installationsToAdd"></param>
        public void AddInstallationsByUser(IEnumerable<Installation> installationsToAdd)
        {
            foreach (var installation in installationsToAdd)
            {
                _indexMap.AddInstallation(installation.UserId, installation);
            }
        }

        /// <summary>
        /// Counts licenses that are tracked on a per-user basis.
        /// Filters the installations for each user, then assesses how many licenses are required for that installation set.
        /// The installs for all users are summed and returned.
        /// </summary>
        /// <param name="installationFilter">Filter used to isolate the installations to consider; applied to each user's installations prior to assessment.</param>
        /// <param name="licenseAssessor">Assessor used to count the licenses required for each user's filtered installations.</param>
        /// <returns>The sum of required licenses across all users.</returns>
        public int CountLicensesByUser(IInstallationFilter installationFilter, ILicenseAssessor licenseAssessor)
        {
            // Walk the dictionary, considering each user individually.
            var licensesRequiredTotal = 0;

            foreach (var userInstallations in _indexMap.Values)
            {
                var considerInstallations = userInstallations.Where(installationFilter.Filter);

                licensesRequiredTotal += licenseAssessor.AssessInstallationLicenses(considerInstallations);
            }

            return licensesRequiredTotal;
        }


        private readonly IInstallationIndexMap _indexMap;
    }
}
