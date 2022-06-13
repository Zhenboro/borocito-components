## Borocito-Component
This is a component. And it can be installed with '`boro-get`'.  
Check [this readme](https://github.com/Zhenboro/borocito-components/blob/dev/boro-get/README.md) to know how to implement `boro-get`, with your BorocitoCLI instances.  

## About
boro-hear is a module that allows plugins to communicate (one-way C->S) with the Control Panel.  

If you make a plugin, you can make it send messages to the control panel. In your code you should put something like [this code](https://github.com/Zhenboro/borocito-components/blob/33632ac2104ceabbc001c9de55fb82cf519842f8/boro-get/Utility.vb#L40-L57).  
That will start an instance of boro-hear which will send your message to the server. boro-hear is single-instance, but messages can be sent by passing parameters to it. These are picked up by boro-hear via the StartUpNextInstance event.  

**I recommend you install it.** Note that **plugins can work without boro-hear**, but **some plugins are more useful and easier to use with boro-hear** installed, as some of them return information that can be very useful to you.
*An example of this is broEstoraje. If you compress a folder, broEstoraje will automatically notify you that the compression has finished. Without boro-hear installed, this notification will not occur.*  

## Usage
The components have access to this component.  
Whatever, you can test the connection by sending boro-get `boro-hear True echo`. And wait ~10sec to receive the 'echo' response.

### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**