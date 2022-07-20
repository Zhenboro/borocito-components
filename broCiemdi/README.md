## Borocito-Component

This is a component. And it can be installed with '`boro-get`'.

Check [this readme](https://github.com/Zhenboro/borocito-components/blob/dev/boro-get/README.md) to know how to implement `boro-get`, with your BorocitoCLI instances.

  

## About

This plugin will allow you to execute code from the native Windows console and allows the display of the output regarding the entered command.

  

To use this component it is necessary to have [boro-hear](https://github.com/Zhenboro/borocito-components/blob/dev/boro-hear/README.md) installed.

> Single Instance Package

  

## Usage

**Send a command:**  

```sh
boro-get broCiemdi True <commands...>
```
Example:  
```sh
ipconfig
```
```sh
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

U can use concat to run more than one command. (&&, &)

### WARNING

**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**