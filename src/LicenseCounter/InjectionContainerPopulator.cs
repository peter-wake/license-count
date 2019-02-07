using InstallationModel;
using ReportLoader;
using SimpleInjector;

namespace LicenseCounter
{
    public class InjectionContainerPopulator
    {
        public void Populate(Container container)
        {
            // If more than one of each object is being made, some attention needs to be given to object lifetime management (as SimpleInjector calls it).
            // For current use cases, simplistic transient lifecycle is sufficient.
            container.Register<IInstallationIndexMap, InstallationIndexMap>();
            container.Register<ILicenseAssessor, LicenseAssessor>();
            container.Register<IInstallationCatalog, InstallationCatalog>();

            container.Register<IReportAnalyser, ReportAnalyser>();
            container.Register<IReportLoader, ReportLoader.ReportLoader>();
            container.Register<IReportToModelConverter, ReportToModelConverter>();

            container.Verify();
        }
    }
}
