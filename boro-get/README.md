## Borocito-Component
This is BORO-GET, the most powerfull and usefull component/package administrador/manager for BorocitoCLI instances.  
With this component, you can install, uninstall and manage components created by you, by the community or by Zhenboro.  

The `boro-get` binaries are downloaded from the link provided in `GlobalSettings.ini` file:  
```sh
[Components]
boro-get=http://chemic-jug.000webhostapp.com/Borocito/Boro-Get/boro-get.zip
```  
Now that boro-get is installed. We can start installing and using the packages.  

Now we are going to define a concept.  
**Single Instance Package**: A package with this warning means that only one instance of it will be started. Just one.  
These are designed to do unique unrepeatable things. Like a keylogger. This is something global, why another instance?  
Also, they can receive parameters when they are already started, so you can control them.  

## How to use it
Let's start with the basics, installing boro-get on the selected computer:  
- You enter `boro-get status` to see the status of `boro-get` on the selected team.  
	- This can return two responses  
		- No installed  
		- Installed! (boro-get X.X.X.X)  
- If the response is `No installed`, then you should enter the command `boro-get install` and wait.  
	- When installed, it will respond with: `boro-get has been installed!`  
- If the response is `Installed! (...)`, then you can start using the components.  

## The basic input
boro-get has a defined command structure.  
This is:  
```sh
	boro-get <packetName> <runOrNot(boolean)> <parameters...>
```
An example will be:
```sh
	boro-get broArbitra True -init C:\...\Code.vb
```
|boro-get|packetName|runOrNot|parameters|
|--|--|--|--|
|boro-get|broArbitra|True|-init C:\\...\Code.vb
**Its that simple.**
Obviusly, with already-installed components, u can use `<set cmd boro-get` to manage the component without prefix.
An example will be:
```sh
broArbitra -init /firstStart
```

## Knowing the state
Don't know if a component is running? Don't know where a component was downloaded from? What version does the component run? Who created the component? All these questions are answerable with the command:
```sh
boro-get <packetName> status
```
An example will be:  
```sh
boro-get broKiloger status
```
```sh
Return:
	[BORO-GET: STATUS.START]
	Author: Zhenboro
	Name: broKiloger
	Version: 0.1.1.0
	broKioger: C:\...\boro-get\broKiloger\broKiloger.exe
	Binaries: http://chemic-jug.000webhostapp.com/Borocito/Boro-Get/REPO/broKiloger.zip
	isRunning: True
	[BORO-GET: STATUS.END]
```
[boro-hear](https://github.com/Zhenboro/borocito-components/blob/dev/boro-hear/README.md) must be installed.

## Uninstalling a package
To uninstall, remove a package, the following points must be taken into account.
- The plugin MUST BE CLOSED. There should not be an instance of this. Read the component documentation to know how to close it. *Anyways, boro-get will try to stop the plugin, but this might fail.*  
- All data from this plugin, recordings, files, etc. will be deleted.  
- They can be reinstalled, don't worry about it.  

To uninstall, use the command:  
```sh
boro-get <package> uninstall
```
An example with broKiloger would be:  
```sh
boro-get broKiloger uninstall
```

### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**