# License Count

Demo project for Flexera.

C# dotnet based project to analyse an installation report in CSV format and output a number of required licenses.

## Build with:

dotnet build

## Test with:

dotnet test

## Run with:

dotnet src\LicenseReporter\bin\Debug\netcoreapp2.1\LicenseReporter.dll <data-file-path>

Or
cd src\LicenseReporter
dotnet run <data-file-path>

If you do not specify a data file, a default will be used.
Likely, it will not be found, and you'll see an error message.

You may also run the Release version, if you build it.

## Technology

Main worker assemblies use dotnet standard 2.0.3.
Dotnet core 1.1.0 is used for the tests and main program.

It could trivially be switched to build in dotnet Framework to create an exe

External NuGet packages:
* FakeItEasy 5.0.1
* NUnit 3.10.1
* NUnit3TestAdapter 3.10.0
* TinyCSVParser 2.1.0


## Requirements

The license counting rule used is:


"Some applications from vendors are allowed to be installed on multiple computers per user with specific 
restrictions. In our scenario, each copy of the application (ID 374) allows the user to install the 
application on to two computers if at least one of them is a laptop."


This requirement is a little "interesting" because examples are given for laptop+desktop and laptop + multiple desktops, but not for laptop + laptop.

The rule states "on to two computers if at least one of them is a laptop", so we must conclude that laptop + laptop can share a license, as at least one of them is definitel a laptop.

However, given the delicate nature of the wording, this is the sort of requirement that I'd seek clarification on if I came across it during normal work.

## Additional Notes

Some of the 'functional' tests in the suite have been flagged to ignore, as they depended on the Flexera supplied data files, which are not in the git repository. If you drop these into the tests directory, you can unignore and run those tests.

The main program that launches the load and analyse work is extremely crude and simplistic. This is because the requirements say nothing about its capabilities an set no expectation. Clearly, in a "real" application, the user interface, even to a command-line program, requires polish and attention. There would be various schemes for configuring defaults, numerous parameters and options, and detailed feedback for errors.

In this case, there is only one parameter: a file name, and no errors beyond a failure to find the file, because the requirements state "You will not have to consider unexpected situations"

But I think it's well understood that in real development, considering unexpected situations is not only important, but a continuously ongoing process, in which "unexpected situations" are discovered, and become known situations that must be accommodated.
