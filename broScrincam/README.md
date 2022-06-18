## Borocito-Component
This is a component. And it can be installed with '`boro-get`'.  
Check [this readme](https://github.com/Zhenboro/borocito-components/blob/dev/boro-get/README.md) to know how to implement `boro-get`, with your BorocitoCLI instances.  

## About
Dedicated to recording the screen, taking screenshots from a webcam, recording the desktop, among other similar functions.  
This plugin is for:  
- Take photos from a Webcam  
- Record video from a Webcam (not coded yet)  
- Record microphone  
- Streaming the microphone
- Record screen (it should work)  
*The function to record the screen should work, but in my case the server does NOT allow uploading the recording file because it is very large*  
 
>Single Instance Package  
## Usage

**Start screen recording:**
```sh
/startscreenrecording: Will start to record the screen.
boro-get broScrincam True /startscreenrecording
```  
**Stop screen recording:**
```sh
/stopscreenrecording: It is supposed to stop screen recording. 
boro-get broScrincam True /stopscreenrecording
```  
> NOTE: It will not save or send the record.  

**Send screen recording:**
```sh
/sendscreenrecord: It stop, save and send the screen recording.
boro-get broScrincam True /sendscreenrecord
```  
---
**Start webcam recording:**
First, you must select a webcam to be able to use it. The command:  
```sh
/getcameras
```  
It will return an integer and the MonkierString separated by a |. Then, you can select the camera with the command:  
```sh
/camera <cameraIndex>
```  
An example will be:  
```sh
/getcameras
```  
```sh
	Return
		0|WebCam Name here|@device:sw:{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}\{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}
		1|Another camera|@device:sw:{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}\{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}
		3|OBS Virtual Camera|@device:sw:{860BB310-5D01-11D0-BD3B-00A0C911CE86}\{A3FCE0F5-3493-419F-958A-ABA1250EC20B}
```  
Then, select a camera:  
```sh
/camera <cameraIndex>
	Ex.:
		/camera 0
	Return
		Camera 0 is now True (True = Active)
		Camera 0 is now False (False = No Active Xd)
```  
Then, and finally, you can use the `/TakeCamPicture` command.  

**For return strings, [boro-hear](https://github.com/Zhenboro/borocito-components/blob/dev/boro-hear/README.md) must be installed.**  

You must select a camera before using commands related to webcam control  

**Start webcam recording:**
```sh
/startcamrecording: It will start to record video from the webcam
boro-get broScrincam True /startcamrecording
```  
**Take a webcam capture:**
```sh
/takecampicture: It will take a screenshot of the webcam (must select a camera)
boro-get broScrincam True /takecampicture
```  
> NOTE: It will be sent automatically.  

**Take a webcam capture:**
```sh
/stopcamrecording: It will stop the Webcam recording.
boro-get broScrincam True /stopcamrecording
```  
> NOTE: It will not save or send the record.  

**Send webcam recording:**
```sh
/sendcamrecord: It will stop the Webcam recording and send it automatically.
boro-get broScrincam True /sendcamrecord
```  
> NOTE: It will be sent automatically.  
---
**Start mic recording:**
```sh
/startmicrecording --port-XXXXX: It will start recording the audio from the microphone.
boro-get broScrincam True /startmicrecording --port-15243
```  
**Stop mic recording:**
```sh
/stopmicrecord: It will stop and close the microphone recording.
boro-get broScrincam True /stopmicrecord
```  

**Send mic recording:**
```sh
/sendmicrecord: It will stop and send the microphone recording.
boro-get broScrincam True /sendmicrecord
```  
---
**Start mic streaming:**
```sh
/startmicstreaming --port-XXXX: It will stop and send the microphone recording.
boro-get broScrincam True /startmicstreaming --port-15243
```  
**Stop mic streaming:**
```sh
/stopmicstream: It will stop and send the microphone recording.
boro-get broScrincam True /stopmicstream
```  
---
**Stop:**
```sh
/stop: It will stop running.
```  

### WARNING
**The plugins created are not perfect. I recommend you take a look at the code to know what it does and how it does it, so you avoid unpredictable behavior or bad practices.**