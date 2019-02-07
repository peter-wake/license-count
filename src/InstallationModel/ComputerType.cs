namespace InstallationModel
{
    /// <summary>
    /// Enumeration of computer types.
    /// These are the types that exist for license assignment.
    /// New types should not be added here unless they are selectable for an application installation target type.
    /// </summary>
    public enum ComputerType
    {
        // Adding values here may impact LicenseAssessor.
        Desktop,
        Laptop
    }
}
