# Windows 10 IoT Core - Soccer Bot Repo
### Tampa IoT Society

For more details please review our documentation directory.

To sign up for the Slack Channel, please contact kevinw@software-logistics.com

Added custom firmware for mBot with the following features:
* Send both left and right motor in one command, previous version took two calls and we saw the robot twist left/right while the first motor was stopped and the other was stopping
* Added the ability to set the mode through the API

The first thing the app does is requests the version from the mBot.  Our custom firmware will start at 5.0 so if it returns 5.0 it will use our enhanced features otherwise it will use the default firmware on the mBot 


#### Getting Started
These instructions currently assume you are developing on a Windows host machine.

Download and install the following:

* [VS.Net Community Edition](https://download.microsoft.com/download/D/2/3/D23F4D0F-BA2D-4600-8725-6CCECEA05196/vs_community_ENU.exe)
* [Windows Universal App SDK v14393](https://download.microsoft.com/download/C/D/8/CD8533F8-5324-4D30-824C-B834C5AD51F9/standalonesdk/sdksetup.exe)
* [Arduino IDE](https://www.microsoft.com/en-us/store/p/arduino-ide/9nblggh4rsd8) - *optional*
* [Visual Studio Arduino plugin](https://visualstudiogallery.msdn.microsoft.com/069a905d-387d-4415-bc37-665a5ac9caba/file/208854/78/Visual.Micro.Arduino.Studio.vsix)

#### Building your MBot

1. Follow the instructions that came with your device to perform the physical build.
1. Follow instructions to install and configure the Bluetooth module and connect with your mobile phone.
1. Verify that the built-in obstacle-detection mode and track-following mode are fuctional.

#### Prepare for JavaScript control of your MBox

[More details for this section available here](https://github.com/Makeblock-official/mbot_nodebots)

1. [Install the Windows Driver for the USB Serial Adapter.](https://github.com/Makeblock-official/mbot_nodebots/blob/master/drivers/windows/CH341SER.EXE)
1. [Install NodeJS](https://nodejs.org/dist/v6.9.4/node-v6.9.4-x86.msi)

Now you will need to install the interchange tool on your Windows Machine.  Open a command prompt and run the following:

	npm install -g nodebots-interchange

##### Install firmware - USB

Now install the USB firmata firmware onto your MBot using USB. Connect the USB cable between your MBot and your PC and enter the following:

	interchange install git+https://github.com/Makeblock-official/mbot_nodebots -a uno --firmata=usb

##### Test firmware - USB

Make sure the MBot is connected to your PC with a USB cable.

You will need to install a few NodeJS libraries to allow this to succeed.

	npm install -g johnny-five node-pixel
	npm install -g socket.io@1.3.6 express@2.5.1
	git checkout https://github.com/Makeblock-official/mbot_nodebots.git
	cd mbot_nodebots
	node examples/motors.js

At this point you should be able to control your MBot with your keyboard.  The arrow keys will control the direction and the space bar will stop the robot.

##### Install firmware - Bluetooth

Optionally, install the bluetooth firmata firmware onto your MBot using USB. Connect the USB cable between your MBot and your PC and enter the following:

	interchange install git+https://github.com/Makeblock-official/mbot_nodebots -a uno --firmata=bluetooth

Now disconnect the USB cable and power cycle your MBot.  Use your OS settings to pair and connect to the MBot device.

##### Test firmware - Bluetooth

Disconnect the USB cable from your computer and your MBox.  Power cycle your MBot and make sure your Bluetooth connection is established.  You may need to launch the device manager to discover the name of the serial port assigned to your bluetooth device.

You will need to install a few NodeJS libraries to allow this to succeed.

	npm install -g johnny-five node-pixel
	npm install -g socket.io@1.3.6 express@2.5.1
	git checkout https://github.com/Makeblock-official/mbot_nodebots.git
	cd mbot_nodebots
	node examples/motors.js com4

At this point you should be able to control your MBot with your keyboard.  The arrow keys will control the direction and the space bar will stop the robot.

##### Notes on firmata firmware

I tried, unsuccessfully, to run several of the other provided examples.  These all had various issues that appeared to be NodeJS related rather than MBot.  If anyone successfully troubleshoots them please update these instructions.