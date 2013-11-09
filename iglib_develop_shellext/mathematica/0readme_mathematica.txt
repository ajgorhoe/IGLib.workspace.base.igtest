
This directory contains interface between IGLib and Mathematica.


The notebooks/ directory contains some helping & auxiliary notebooks for the 
interface with Mathematica.
Some notebooks have initialization cells and should be converted to Mathematica 
packages (.m) so they can be loaded directly.
The notebook iglib.nb contains stuff that is needed for the two way interface.

With Mathematica versions lower than 9, it must be ensured that the .NET/Link uses
the correct (high enough, currently 4 - check at project settings in IGLib) .NET 
interface. Some instructions are in the iglib.nb:

====  iglib.nb file, cell Initializaton of IGLib interface/Instructions:

The following things must be done before using the IGLib code:
Loading of .NET environment:

  Needs["NETLink`"]; (*
 Imports the .NET/Link module *) 
  ReInstallNET[]; (* Installs the .NET runtime environment *)

Important!
It must be ensured that the right .NET framework wersion is loaded. The framework is loaded by the .NET program InstallableNET.exe that is found in the "SystemFiles/Links/NETLink/" subdirectory of the Mathematica installation directory. The exact location of this program is obtainef by
  StringJoin[$InstallationDirectory, "/", "SystemFiles/", "Links/", "NETLink/", "InstallableNET.exe"]
Which framework is preferably loaded can be specified in the program's config file, that is the InstallableNET.exe.config located in teh same directory. You just edit this file and add the possible .NET runtime versions from the most preferable to the least preferable in the <supportedRuntime/> XML elements. For exemple, teh file can lool like:
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<startup>
		<supportedRuntime version="v4.0" />
	</startup>
</configuration>
This means that the InstallableNET.exe will try to load the runtime environment version 4. If the earlier runtime is loaded that that in which the assemblies (dll-s and exe-s) to be used were created, then MathLink will not be able to load these assemblies.
Warning: after editing the configuration file, you may need to restart the machine in order to prevent possible cashed vertions to make effect!

Loading assemblies and using their types:
After the runtime has been loaded, assemblies (dll-s and exe-s) san be loaded and their types can be instantiated and their methods, properties and fields can be called directly from Mathematica. Within Initialization block, there are some definitions to make the access of IGLib code easier. You can check that everything works correctly in the "Test IGLib Interface" block.

====