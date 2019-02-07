# License Count

Demo project for Flexera.

C# dotnet based project to analyse an installation report in CSV format and output a number of required licenses.

## Build with:

dotnet build

## Test with:

dotnet test

## Run with:

dotnet src\LicenseReporter\bin\Debug\netcoreapp2.1\LicenseReporter.dll <data-file-path>

If you do not specify a data file, a default will be used.
Likely, it will not be found, and you'll see an error message.

## Technology

Main worker assemblies use dotnet standard.
Dotnet core is used for the tests and main program.
Could trivially be switched to build in dotnet Framework.

## Requirements

The license counting rule used is:


"Some applications from vendors are allowed to be installed on multiple computers per user with specific 
restrictions. In our scenario, each copy of the application (ID 374) allows the user to install the 
application on to two computers if at least one of them is a laptop."


This requirement is a little "interesting" because examples are given for laptop+desktop and laptop + multiple desktops, but not for laptop + laptop.

The rule states "on to two computers if at least one of them is a laptop", so we must conclude that laptop + laptop can share a license.

However, given the delicate nature of the wording, this is the sort of requirement that I'd seek clarification on if I came across it during normal work.

## Additional Notes

Some tests have been ignored, as they depended on the Flexera supplied data files, which are not in the git repository. If you drop these into the tests directory, you can unignore and run those tests.
