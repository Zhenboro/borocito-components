## Borocito-Component
This is a component. And it can be installed with '`boro-get`'.  
Check [this readme](https://github.com/Zhenboro/borocito-components/blob/dev/boro-get/README.md) to know how to implement `boro-get`, with your BorocitoCLI instances.  

## About
This plugin will be useful if you want to clean up the main Borocito folder.  
Many screenshots? Have the log files exceeded one megabyte of data? Does the hard drive sound louder than before? Do you want to prevent someone from noticing the size of Borocito in the Microsoft folder? solved with broMaintainer.  

## Usage
Forget about deleting files one by one with `/Windows.FileSystem.Delete=` or worse, using `boro-get broEstoraje True /deletefile,/deletefirectory` (since these doesn't exists, if you tried it's sad, read the documentation!).  
With broMaintainer you can use the following commands:   
- `/fullclear`: will delete every
	- screenshot
	- log file
	- boro-get component files
		- log file
		- scrincam files
			- wav
			- gif
			- png
			- avi
		- boro-get zip install files
	- Basically everything that is not necessary.  
 - `/boro-get`: To remove all residual files from installed plugins
 - `/deletefiles`: to delete files in the root folder. need wildcard. (*.jpg, *.png, etc (only one))
 - `/stop`: idk why lmao

### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**