## Borocito-Component
This is a component. And it can be installed with '`boro-get`'.  
Check [this readme](https://github.com/Zhenboro/borocito-components/blob/dev/boro-get/README.md) to know how to implement `boro-get`, with your BorocitoCLI instances.  

## About
This plugin was created because it turns out that sometimes an instance of BorocitoCLI can be closed by some external or internal factor. Perhaps the user noticed the presence and closed it, or, a command left a mess that BorocitoCLI couldn't handle.  
For this reason, broRescue is a good recommendation that it be installed.  

> Single Instance Package  
## Usage
If you notice that BorocitoCLI has closed. You can make broRescue restart the BorocitoCLI instance by using the command:  
```sh
broRescue Borocito
```  
and not only that, you can also start the extractor, or the updater.  
To do this, the commands will be:  
```sh
broRescue Extractor
```  
```sh
broRescue Updater
```  
Or even restart the computer with the command:  
```sh
broRescue Restart
```  
And, you can also try to prevent the computer from being turned off. with the command:  
```sh
broRescue NoShutdown
```  
to avoid or, `YesShutdown` to not try to avoid.  

This packet reads directly from the server. Every 5 minutes. So yes, you will notice the effect in 5 minutes. And that is done so as not to load the server, and also so that it is not noticed from the task manager.  

---
**You can also get the number of seconds since the user's last activity with the command:**  
```sh
/GetAFK
```  
Example:  
```sh
boro-get broRescue True /GetAFK
```  
Data returned *([boro-hear](https://github.com/Zhenboro/borocito-components/blob/dev/boro-hear/README.md) must be installed)* 
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

### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**