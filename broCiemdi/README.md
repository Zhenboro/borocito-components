
## Borocito-Component
This is a component. And it can be installed with '`boro-get`'.  
Check [this readme](https://github.com/Zhenboro/borocito-components/blob/dev/boro-get/README.md) to know how to implement `boro-get`, with your BorocitoCLI instances.  

## About
This plugin will allow you to execute code from the native Windows console and allows the display of the output regarding the entered command.  
Can be used as a SSH.  
To use this component it is necessary to have [boro-hear](https://github.com/Zhenboro/borocito-components/blob/dev/boro-hear/README.md) installed.  

> Single Instance Package

## Usage
**Starting the instance:**  
```bash
boro-get broCiemdi True /start
```
**Stoping the instance:**  
```bash
boro-get broCiemdi True /stop
```
**Send a command:**  
```bash
boro-get broCiemdi True <commands...>
```
Example:  
```bash
boro-get broCiemdi True ipconfig
```
```bash
RETURN
	Configuración IP de Windows
	

	Adaptador de Ethernet Ethernet0:

	   Sufijo DNS específico para la conexión. . :
	   Vínculo: dirección IPv6 local. . . : fe80::9908:1943:e520:7f17%4
	   Dirección IPv4. . . . . . . . . . . . . . : 192.168.0.69
	   Máscara de subred . . . . . . . . . . . . : 255.255.255.0
	   Puerta de enlace predeterminada . . . . . : 192.168.0.1

```
just like that.  
U can use concat to run more than one command. (&&, &) [i guess you can]  

## Use case
If you want to create a folder inside Desktop. You can:  
```bash
boro-get broCiemdi cd Desktop
*return*
boro-get broCiemdi mkdir holaFolder
boro-get broCiemdi cd holaFolder
```

### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**
