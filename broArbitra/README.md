## Borocito-Component
This is a component. And it can be installed with '`boro-get`'.  
Check [this readme](https://github.com/Zhenboro/borocito-components/tree/main/boro-get/README.md) to know how to implement `boro-get`, with your BorocitoCLI instances.  

## About
broArbitra was created to execute arbitrary code completely independent of the BorocitoCLI instance.  
You can program your class with your own behavior.  

What is mandatory are only three things:  
- The name of the class must be `broCodeProvider`  
- The function that will be called when starting the instance will be called `Initiali` [optional initParam as String] as String  
- The name of the function that will be called every time you want will be called `Arbitra` [optional Param as String] as String  

An example can be found [here](https://github.com/Zhenboro/borocito-components/blob/dev/broArbitra/ArbitraCode.vb)

## Usage
