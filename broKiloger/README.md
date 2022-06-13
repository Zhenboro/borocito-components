## Borocito-Component
This is a component. And it can be installed with '`boro-get`'.  
Check [this readme](https://github.com/Zhenboro/borocito-components/blob/dev/boro-get/README.md) to know how to implement `boro-get`, with your BorocitoCLI instances.  

## About
Dedicated to registering keys from the keyboard.  

> Single Instance Package  
## Usage

 **Start recording:**
 ```sh
/startrecording
```
**Stop recording:**
 ```sh
/stoprecording
```
It will not save or send the record.  
**Send record:**
 ```sh
/sendrecord
```
**Reset record:**
 ```sh
/resetrecord
```
Is like a restart. Clean the record and then keep logging keys.
**Send and close:**
 ```sh
/sendandexit
```
The logs contain the keys pressed. {KEY_CODE} or normal uppercase letters for alphabet keys.  
Can't understand a shit? [Key mapping for broKiloger keylog](https://chemic-jug.000webhostapp.com/Borocito/Mapeo_Teclas_Kiloger.txt)  

You can also send keys with:  
 ```sh
/process <key_code>
```
Example:  
 ```sh
/process LWIN e x p l o r e r DECIMAL e x e RETURN
```
That will open the start menu and then type explorer.exe, then start that process with the enter key.  
This function is executed asynchronously. So the keylog will still work.  

### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**