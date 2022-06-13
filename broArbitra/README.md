## Borocito-Component
This is a component. And it can be installed with '`boro-get`'.  
Check [this readme](https://github.com/Zhenboro/borocito-components/blob/dev/boro-get/README.md) to know how to implement `boro-get`, with your BorocitoCLI instances.  

## About
broArbitra was created to execute arbitrary code completely independent of the BorocitoCLI instance.  
You can program your class with your own behavior.  

What is mandatory are only three things:  
- The name of the class must be `broCodeProvider`  
- The function that will be called when starting the instance will be called `Initiali` [optional initParam as String] as String  
- The name of the function that will be called every time you want will be called `Arbitra` [optional Param as String] as String  

An example can be found [here](https://github.com/Zhenboro/borocito-components/blob/dev/broArbitra/ArbitraCode.vb)

## Usage
It may first be necessary to add references for your Arbitra instance.  
For this you can add references to assemblies from Arbitra, and these will be implemented in the instance, so that your class can use them.  
To add instances  
```sh
/addref <refPath>
```
Example  
```sh
/addref C:\Users\Zhenboro\Downloads\json_manager.dll
```
Were you wrong? Well, aweonao.  
You can remove a reference with  
```sh
/delref <refPath>
```
Example  
```sh
/delref C:\Users\Zhenboro\Downloads\json_manager.dll
```

Now that the references are ready. We can start using our class.  
For this we use:  
```sh
-init <textCodePath> <[opt]initParam (as string)>
```
Example  
```sh
-init C:\Users\Zhenboro\Downloads\ArbitraCode.vb
```
We wait for the answer (if `boro-hear` is installed) or we wait a reasonable time.  
Since we know that the instance is created, we can use the `Arbitra` function and optionally pass a parameter that we want. This depends on how you have programmed it. Remember that you make your code.  
Call `Arbitra` function:  
```sh
/call <[opt]Param (as string)>
```
Example  
```sh
/call -firstStart
```
Have you done what you should? You can kill the instance with:  
```sh
-kill
```
broArbitra will close. The instance too.

### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**