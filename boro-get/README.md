## Borocito-Component
This is BORO-GET, the most powerfull and usefull component/package administrador/manager for BorocitoCLI instances.  
With this component, you can install, uninstall and manage components created by you, by the community or by Zhenboro.  

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
