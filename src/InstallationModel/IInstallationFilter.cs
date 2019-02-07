namespace InstallationModel
{
    /// <summary>
    /// Interface for Installation filters that identify Installation objects of interest according to criteria specific to the Filter implementation.
    /// For example, a filter might restrict (return true) only for Installation objects with a particular applicationId, or for a particular user.
    /// </summary>
    public interface IInstallationFilter
    {
        /// <summary>
        /// Return true if the installation meets the filter criteria.
        /// </summary>
        /// <param name="installation">Installation to filter.</param>
        /// <returns>True if the Installation should be retained; false if the Installation should be excluded.</returns>
        bool Filter(Installation installation);
    }
}