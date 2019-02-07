using System.Collections.Generic;

namespace InstallationModel
{
    /// <summary>
    /// An interface for an abstract indexer.
    /// The implementation should allow Installation objects to be indexed via an integer key.
    /// Allows the items to be returned as an enumeration of sets, in no particular order.
    /// The use of sets implies that duplicate items should not be be possible for a given index.
    /// </summary>
    public interface IInstallationIndexMap
    {
        /// <summary>
        /// Retrieve an enumeration of the indexed sets. All members in any given set will have the same index.
        /// Sets are not returned in any particular order.
        /// </summary>
        IEnumerable<ISet<Installation>> Values { get; }

        /// <summary>
        /// Adds an Installation to the indexed collection, placing it in set based on the passed index value.
        /// </summary>
        /// <param name="indexValue">The value to index the Installation by.</param>
        /// <param name="installation">The installation to store in a set, grouped by index.</param>
        void AddInstallation(int indexValue, Installation installation);
    }
}