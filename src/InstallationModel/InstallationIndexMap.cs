using System.Collections.Generic;

namespace InstallationModel
{
    /// <summary>
    /// An implementation of IInstallationIndexMap using a Dictionary of int mapping to sets of Installation.
    /// Agnostic about the meaning of the index value, though it is constrained to be an int.
    /// </summary>
    public class InstallationIndexMap : IInstallationIndexMap
    {
        // Could make the index type a generic type parameter, but no need for that at this point.
        // Abstracting Dictionary handling in this way keeps functionality blocks simple and comprehensible and eases testing and injection configuration.
        // Could implement this sort of Dictionary behaviour with an extension method, but it would make it harder to write
        // tests for classes that use the dictionary.

        /// <summary>
        /// Construct an empty InstallationIndex.
        /// </summary>
        public InstallationIndexMap()
        {
            _installationSets = new Dictionary<int, ISet<Installation>>();
        }

        /// <summary>
        /// Return an enumeration of the values sets (grouped by index value), but in no specific indexMap value order.
        /// </summary>
        public IEnumerable<ISet<Installation>> Values => _installationSets.Values;

        /// <summary>
        /// Add an Installation to a set identified by the index value.
        /// If no set exists yet, a new one will be created.
        /// All Installation with the same index value are placed in the same set.
        /// </summary>
        /// <param name="indexValue">The index value to select the target set for the Installation.</param>
        /// <param name="installation">The Installation object to add to the target set.</param>
        public void AddInstallation(int indexValue, Installation installation)
        {
            // TryGetValue more efficient than double queries on the dictionary.
            if (_installationSets.TryGetValue(indexValue, out var installationSet))
            {
                installationSet.Add(installation);
            }
            else
            {
                _installationSets.Add(indexValue, GetInstallationSet(installation));
            }
        }

        /// <summary>
        /// Get a new ISet of Installation. Allows changing the set implementation being used in a single place.
        /// </summary>
        /// <param name="initialMember">An installation to add to the new set.</param>
        /// <returns>A new set of Installation, with initialMember as its sole occupant.</returns>
        private ISet<Installation> GetInstallationSet(Installation initialMember)
        {
            return new HashSet<Installation> { initialMember };
        }

        private Dictionary<int, ISet<Installation>> _installationSets;
    }
}
