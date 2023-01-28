
## Borocito-Component
This is BORO-GET, the most powerfull and usefull component/package administrador/manager for BorocitoCLI instances.  
With this component, you can install, uninstall and manage components created by you, by the community or by Zhenboro.  

The `boro-get` will be configured with the linked `config.ini` file inside `Globals.ini` file:  
```ini
[boro-get]
Configuration=http://chemic-jug.000webhostapp.com/Borocito/Boro-Get/config.ini
```  
Now, inside the `config.ini`, the configuration file for `boro-get`. Boro-Get can be downloaded from:  
```ini
[CONFIG]
Binaries=http://chemic-jug.000webhostapp.com/Borocito/Boro-Get/boro-get.zip
```  
The, we must configure the repositories. Inside `config.ini`, we can link the `Repositories.ini` file.  
```ini
[CONFIG]
Repositories=http://chemic-jug.000webhostapp.com/Borocito/Boro-Get/Repositories.ini
```  
AND THEN, we must refer every component inside the `Repositories.ini` repository linker file. Example:  
```ini
[REPOSITORIES]
boro-hear=http://chemic-jug.000webhostapp.com/Borocito/Boro-Get/boro-hear.inf
```  
Now, that `boro-get` is already done in the **Server-Side**, we can start using it.  

Now we are going to define a concept:  
1. **Single Instance Package**: A package with this warning means that only one instance of it will be started. Just one.  
These are designed to do unique unrepeatable things. Like a keylogger. This is something global, why another instance?  
Also, they can receive parameters when they are already started, so you can control them.  
These can be: broArbitra, broCiemdi, broReedit, like that ones.  

2. **The ones that are not marked as Single Instance Package**: They can be components that doesn't run after processing a command. Like adding a rule to Firewall with broFaierwoll, or doing cleaning with broMaintainer.

## How to use it
Let's start with the basics, installing `boro-get` on the selected computer:  
- You enter `boro-get status` to see the status of `boro-get` on the selected team.  
	- This can return two responses  
		- No installed  
		- Installed! (boro-get X.X.X.X)  
- If the response is `No installed`, then you should enter the command `boro-get install` and wait.  
	- When installed, it will respond with: `boro-get has been installed!`  
- If the response is `Installed! (...)`, then you can start using the components.  

## The basic input
`boro-get` has a defined command structure.  
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
```bash
boro-get broKiloger status
```
```ini
Return:
(BORO-HEAR)
	[BORO-GET: STATUS.START]
	Author: Zhenboro
	Name: broKiloger
	Version: 0.3.2.0 (22.01.20.23)
	broKiloger: C:\...\Borocito\boro-get\broKiloger\broKiloger.exe
	Binaries: broKiloger.exe
	Web: https://github.com/Zhenboro/borocito-components/tree/main/broKiloger
	isRunning: True
	[BORO-GET: STATUS.END]
```
[boro-hear](https://github.com/Zhenboro/borocito-components/blob/dev/boro-hear/README.md) must be installed (for the return response).

## Listing the installed components
You don't know how many components are installed, and wanna know that?, NO problem.  
You can use:  
`boro-get components installed`  
And the return will be:  
```ini
(BORO-HEAR)
	[BORO-GET: INSTALLED.START]
	boro-hear True, 0.1.0.0 (23.04.20.22)
	broCiemdi False, 0.3.0.0 (22.01.20.23)
	broKiloger True, 0.3.2.0 (22.01.20.23)
	broReedit False, 1.0.0.0 (29.04.20.22)
	broRescue True, 0.1.0.0 (21.05.20.22)
	broScrincam False, 0.1.0.0 (18.06.20.22)
	[BORO-GET: INSTALLED.END]
```
Reponse syntax: `<componentName> <IsRunning>, <Version> (<Compilated>)`  

## Uninstalling a package
To uninstall, remove a package, the following points must be taken into account.
- The plugin MUST BE CLOSED. There should not be an instance of this. Read the component documentation to know how to close it. *Anyways, `boro-get` will try to stop the plugin, but this might fail.*  
- All data from this plugin, recordings, files, etc. will be deleted (the install folder of that component will be deleted, with every inside).  
- They can be reinstalled, don't worry about it.  

To uninstall, use the command:  
```bash
boro-get <package> uninstall
```
An example with `broKiloger` would be:  
```bash
boro-get broKiloger uninstall
```

## Reinstall Boro-Get
Have a new version in your Server-Side and wanna use it RIGHT FUCKING NOW?!?!?. NO F PROBLEM BRA.  
Use:  
`boro-get reinstall`  
And wait for the response:  
`boro-get has been reinstalled!`

### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**
