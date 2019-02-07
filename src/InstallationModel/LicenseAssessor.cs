using System.Collections.Generic;

namespace InstallationModel
{
    /// <summary>
    /// Counts the number of licenses required for an enumeration of Installation objects.
    /// It uses a rule where licenses are per-user-per-machine, but the user is allowed an extra install onto a laptop if they have a matching desktop install.
    /// </summary>
    public class LicenseAssessor : ILicenseAssessor
    {
        // License rules as gleaned from the requirements...
        //
        // The text says: "In our scenario, each copy of the application (ID 374) allows the user to install the application on to two computers if at least one of them is a laptop."
        //
        // Thereafter, examples are given where a desktop and laptop install are allowed on a single license.
        // Another example shows that two desktop installs requires two licenses.
        // The final example shows two desktops and a laptop, which requires two licenses.
        //
        // There is no example of two laptops.
        //
        // HOWEVER, the the words of the requirements state "AT LEAST ONE OF THEM IS A LAPTOP".
        // From this wording, it MUST also be valid to install TWO LAPTOPS on a single license, because at least one of them is a laptop.
        //
        // This feels slightly counter-intuitive.
        // Intuitively, it seems more probable that the intent is for a license to allow a desktop plus a bonus laptop...
        // ...not for a license to allow two laptops.
        //
        // In a real situation, I would seek clarification on this, as it seems suspicious.
        //
        // However, in this case, I cannot seek clarification, and must simply assume this is a deliberate trap for the unwary, and that the two laptops 
        // example was intentionally omitted to catch out people who didn't read the requirements carefully enough.
        //
        // It also makes calculation of the number of licenses required a little more complex than simply MAX(desktopCount, laptopCount)
        //
        // Examples:
        //     D           => 1 license
        //     L           => 1 license
        //     D D         => 2 licenses
        //     D L         => 1 license
        //     L L         => 1 license
        //     D D D       => 3 licenses
        //     D D L       => 2 licenses
        //     D L L       => 2 licenses
        //     L L L       => 2 licenses
        //     D D D D     => 4 licenses
        //     D D D L     => 3 licenses
        //     D D L L     => 2 licenses
        //     D L L L     => 2 licenses
        //     L L L L     => 2 licenses
        //     D D D D L   => 4 licenses
        //     D D D L L   => 3 licenses
        //     D D L L L   => 3 licenses
        //     D L L L L   => 3 licenses
        //     D L L L L L => 3 licenses


        /// <summary>
        /// Count the licenses required for this set of per-user Installation objects.
        /// </summary>
        /// <param name="installations">An enumeration of Installation, which must all belong to the same user, and belong to a single application that uses this licensing rule.</param>
        /// <returns>The number of licenses required for the user whose installations are being assessed.</returns>
        public int AssessInstallationLicenses(IEnumerable<Installation> installations)
        {
            var desktopComputers = 0;
            var laptopComputers = 0;

            // Unlike using LINQ count with two sets of conditions, this requires only a single traversal of the enumeration.
            // e.g. could have written installations.Count(ii => ComputerType.Desktop == ii.ComputerType.Desktop) - slightly more compact, but no clearer.
            foreach (var installation in installations)
            {
                switch (installation.ComputerType)
                {
                    case ComputerType.Desktop:
                        desktopComputers += 1;
                        break;
                    case ComputerType.Laptop:
                        laptopComputers += 1;
                        break;
                }
            }

            // If laptops <= desktops, all laptops come for "free"
            var licensesRequiredForThisUser = desktopComputers;

            // otherwise, each two excess laptops or part thereof costs an additional license.
            if (laptopComputers > desktopComputers)
            {
                var excessLaptops = laptopComputers - desktopComputers;
                licensesRequiredForThisUser += (excessLaptops + 1) / 2;
            }

            return licensesRequiredForThisUser;

        }
    }
}
