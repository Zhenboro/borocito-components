## Borocito-Component
This is a component. And it can be installed with '`boro-get`'.  
Check [this readme](https://github.com/Zhenboro/borocito-components/blob/dev/boro-get/README.md) to know how to implement `boro-get`, with your BorocitoCLI instances.  

## About
This component was created to create exceptions in the windows firewall. This way you can avoid the Firewall message where an app is trying to make a connection.  
I honestly don't know if it works, but from what I saw, it does. I don't know.  

## Usage
Create a basic exception:  
```sh
/basic <ruleName> <processPath> <ActiveOrNot>
```
Example  
```sh
/basic rmtdsk_tcp C:\Users\...\RMTDSK_Client.exe True
```

Create a simple exception:  
```sh
/simple <ruleName> <portPort> <ActiveOrNot>
```
Example  
```sh
/simple rmtdsk_tcp 15243 True
```

### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**