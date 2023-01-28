## Borocito-Component
This is a component. And it can be installed with '`boro-get`'.  
Check [this readme](https://github.com/Zhenboro/borocito-components/blob/dev/boro-get/README.md) to know how to implement `boro-get`, with your BorocitoCLI instances.  

## About
This complement allows you to modify the Windows Registry.  

To use this component (and not navigate blindly) it is necessary to have [boro-hear](https://github.com/Zhenboro/borocito-components/blob/dev/boro-hear/README.md) installed.  
> Single Instance Package

## Usage
**To select a "Hive":**  
```bash
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
```bash
/selecthk CurrentUser
```
---
**To select a SubKey:**  
```bash
/selectkey <subkey>
```
Example:  
```bash
/selecthk SOFTWARE\Borocito
```
---
**To get a value:**  
```bash
/getvalue <valueName>
```
Example:  
```bash
/getvalue OwnerServer
```
---
**To set a value:**  
```bash
/setvalue <valueName> <value> <valueKind>
```
See types in [RegistryValueKind Enum (Microsoft.Win32) | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registryvaluekind?msclkid=cb066977c71011ecb24bd53c4a938a9b&view=net-6.0)
Example:  
```bash
/setvalue OwnerServer http://.../ 1
```
If you set null in valueKind then will be 1 (String)  

---
**To delete a value:**  
```bash
/deletevalue <valueName>
```
Example:  
```bash
/deletevalue OwnerServer
```
---
**To get a value names:**  
```bash
/getvaluenames()
```
```bash
Return:
	OwnerServer
	UID
	(etc...)
```
---
**To get a value kind:**  
```bash
/getvaluekind <valueName>
```
Example:  
```bash
/getvaluekind OwnerServer
```
```bash
Return:
	1
```
See types in [RegistryValueKind Enum (Microsoft.Win32) | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/api/microsoft.win32.registryvaluekind?msclkid=cb066977c71011ecb24bd53c4a938a9b&view=net-6.0)

---
**To get list of SubKeys:**  
```bash
/getsubkeynames()
```
---
**To create a SubKey:**  
```bash
/createsubkey <subKeyName>
```
Example:  
```bash
/createsubkey Backup
```
---
**To delete a SubKey:**  
```bash
/deletesubkey <subkeyName>
```
Example:  
```bash
/deletesubkey boro-get
```
---
**To delete a SubKey Tree:**  
```bash
/deletesubkeytree <subkeyName>
```
Example:  
```bash
/deletesubkeytree boro-get
```
(Will remove all subkeys hanging from the "`boro-get`" key)  

### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**
