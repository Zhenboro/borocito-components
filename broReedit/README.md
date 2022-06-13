## Borocito-Component
This is a component. And it can be installed with '`boro-get`'.  
Check [this readme](https://github.com/Zhenboro/borocito-components/blob/dev/boro-get/README.md) to know how to implement `boro-get`, with your BorocitoCLI instances.  

## About
This complement allows you to modify the Windows Registry.  

To use this component (and not navigate blindly) it is necessary to have [boro-hear](https://github.com/Zhenboro/borocito-components/blob/dev/boro-hear/README.md) installed.  
> Single Instance Package

## Usage
**To select a "Hive":**  
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
**To select a SubKey:**  
```sh
/selectkey <subkey>
```
Example:  
```sh
/selecthk SOFTWARE\Borocito
```
---
**To get a value:**  
```sh
/getvalue <valueName>
```
Example:  
```sh
/getvalue OwnerServer
```
---
**To set a value:**  
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
**To delete a value:**  
```sh
/deletevalue <valueName>
```
Example:  
```sh
/deletevalue OwnerServer
```
---
**To get a value names:**  
```sh
/getvaluenames()
```
```sh
Return:
	OwnerServer
	UID
	(etc...)
```
---
**To get a value kind:**  
```sh
/getvaluekind <valueName>
```
Example:  
```sh
/getvaluekind OwnerServer
```
```sh
Return:
	1
```
See types in [RegistryValueKind Enum (Microsoft.Win32) | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registryvaluekind?msclkid=cb066977c71011ecb24bd53c4a938a9b&view=net-6.0)

---
**To create a SubKey:**  
```sh
/createsubkey <subKeyName>
```
Example:  
```sh
/createsubkey Backup
```
---
**To delete a SubKey:**  
```sh
/deletesubkey <subkeyName>
```
Example:  
```sh
/deletesubkey boro-get
```
---
**To delete a SubKey Tree:**  
```sh
/deletesubkeytree <subkeyName>
```
Example:  
```sh
/deletesubkeytree boro-get
```
(Will remove all subkeys hanging from the "boro-get" key)  

### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**