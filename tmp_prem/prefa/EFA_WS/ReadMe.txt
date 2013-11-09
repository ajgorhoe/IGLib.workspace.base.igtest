NAPAKE in RE�ITVE:

1.Retrieving the COM class factory for component with CLSID {8270CB2F-B0E6-4C37-8A40-D70778F47894} failed due to the following error: 80040154.
	V primeru te�av z instanciiranjem BulkLoad COM objekta, je potrebno:
	- �e v C:\Program Files ni mape SQLXML 4.0 potem je treba instalirati paket sqlxml 4.0 (verzijo 4.0 se ne dobi za download, je pa na voljo na SQL Server DVD-ju)
	- v VS v projekt kjer uporablja� SQLXMLBULKLOADLib doda� referenco, ampak pozor: doda� COM referenco Microsoft SQLXML Bulkload 4.0 Type Library
	- po dodajanju reference se bo zgradil Interop.SQLXMLBULKLOADLib.dll, ki je ovojnica za COM komponento xblkld4.dll
	- �lanek na to temo je na voljo tukaj: http://msdn.microsoft.com/en-us/library/aa257393(SQL.80).aspx

2.Cannot bulk load because the file "C:\DOCUME~1\blakuc\LOCALS~1\Temp\BL_2008617113637195_8.txt" could not be opened. Operating system error code 3(error not found).
	Oddaljen SQL stre�nik ne vidi temp datoteke za BULK INSERT. 
	Nastaviti pravilne poti v app.config, klju�a SQLServerTempFilePath in remoteSQLServer.
	Pravilna pot pomeni, da share-an direktorij vidita tako account pod katerim te�e Web Service in account pod katerim te�e SQL Server.


3.One of the temporary files could not be created. Check that the TempFilePath property is valid or that the TEMP environment variable is set. 
	Error message returned: 'Access is denied.'


4.Cannot bulk load. The file "c:\temp\BL_2008617127974_8.txt" does not exist.
	Oddaljen SQL stre�nik ne vidi temp datoteke za BULK INSERT. 
	Nastaviti pravilne poti v web.config, klju�a SQLServerTempFilePath in remoteSQLServer.
	Pravilna pot pomeni, da share-an direktorij vidita tako account pod katerim te�e Web Service in account pod katerim te�e SQL Server.

5.Cannot bulk load because the file "c:\temp\BL_2008617127974_8.txt" could not be opened. Operating system error code 5(error not found).
	Oddaljen SQL stre�nik ne vidi temp datoteke za BULK INSERT. 
	Nastaviti pravilne poti v app.config, klju�a SQLServerTempFilePath in remoteSQLServer.
	Pravilna pot pomeni, da share-an direktorij vidita tako account pod katerim te�e Web Service in account pod katerim te�e SQL Server.

6. V EfaPaketPovratnica pi�e v polju NapakaSporocilo: You do not have permission to use the bulk load statement.
	Uporabnik, ki je naveden v cnnStringBulk v web.config od EFA_WS mora biti v bulkadmin server vlogi (�e je v sysadmin vlogi potem je implicitno tudi v bulkadmin vlogi)

7. V ERR_Paket ... datoteki (nahaja se tam, kamor ka�e PathName v web.config) �isto na koncu pi�e: You do not have permission to use the bulk load statement.
	Uporabnik, ki je naveden v cnnStringBulk v web.config od EFA_WS mora biti v bulkadmin server vlogi (�e je v sysadmin vlogi potem je implicitno tudi v bulkadmin vlogi)
	
8. MSBuild javi napako: warning MSB3287: Cannot load type library for reference "SQLXMLBULKLOADLib". Library not registered. (Exception from HRESULT: 0x8002801D (TYPE_E_LIBNOTREGISTERED))
	- Problem je v tem, da xblkld4.dll ni .NET komponenta ampak je to COM komponenta, torej je treba naredit ovojnico okrog nje. 
	- �e v C:\Program Files ni mape SQLXML 4.0 potem je treba instalirati paket sqlxml4.msi (verzijo 4.0 se ne dobi za download, je pa na voljo na SQL Server DVD-ju)
	- POZOR: Datoteko xblkld4.dll dobi� po instalaciji sqlxml4.msi v mapi C:\Program Files\Common Files\System\Ole DB
	- s tlbimp.exe (pride z .NET Frameworkom, dobi� ga v C:\Program Files\Microsoft SDKs\Windows\v6.1\Bin na W2k3) naredi� .NET ovojnico, primer: C:\tlbimp xblkld4.dll. To naredi nov dll (SQLXMLBULKLOADLib.dll) in tega lahko zdaj uporabi� v projektu kot referenco.
	- Na BuildServer-ju ta nov dll skopira� tja, koder projekti pri�akujejo reference.
	- �lanek na to temo je na voljo tukaj: http://msdn.microsoft.com/en-us/library/aa257393(SQL.80).aspx

