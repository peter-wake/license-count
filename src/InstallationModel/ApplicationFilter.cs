namespace InstallationModel
{
    /// <summary>
    /// Filters applications have that have the required ID.
    /// </summary>
    public class ApplicationFilter : IInstallationFilter
    {
        /// <summary>
        /// Create a new filter for a specific application ID.
        /// </summary>
        /// <param name="applicationId">The application ID that we want to isolate.</param>
        public ApplicationFilter(int applicationId)
        {
            _applicationId = applicationId;
        }

        /// <summary>
        /// Returns true for installations that have an application ID matching the one set on filter creation.
        /// </summary>
        /// <param name="installation">An Installation to test.</param>
        /// <returns>True is the Installation is for the specified application ID.</returns>
        public bool Filter(Installation installation) => _applicationId == installation.ApplicationId;


        private readonly int _applicationId;
    }
}
