# Windows 10 IoT Core - Soccer Bot Repo
## Getting Started


### Software Requirements

[Visual Studio 2015](https://www.microsoftstore.com/store/msusa/en_US/cat/Visual-Studio-2015/categoryID.69407500?s_kwcid=AL!4249!10!13675262506!84040865656&ef_id=WA5uGgAABWJOSWR6:20170130221540:s)
Make sure when you install Visual Studio, you install support for building UWP/Windows Store apps.  The minimum [Windows SDK](https://developer.microsoft.com/en-US/windows/downloads/windows-10-sdk) we support is 14393


[Arduino](https://www.arduino.com) - Necessary to install [custom Arduino firmware](https://github.com/bytemaster-0xff/WinIoTSoccerBot/tree/master/src/mBotFirmware).  At this time installing custom firmware isn't 100% required bit it will provide better performance and advanved features.

[Visual Micro](https://www.VisualMicro.com) - Not 100% necessary but provides a much better IDE for working with Arduino.


### Software Organization
There are three main programs that make up the client portion of the Soccer Bot Platform

Windows Store App Running on a PC - Used to manually control your SoccerBot

Windows UWP app (Windows 10 IoT Core) running on Raspberry Pi - A small backpack for the robot to provide additional computing power and cloud connectivity

Custom Firmware - Additional features and tuned software to work with the Windows 10 IoT Core app.



### Install Windows 10 IoT Core

Follow the instructions [here](https://developer.microsoft.com/en-us/windows/iot/GetStarted) to get Windows 10 IoT core up and running your Rasperry Pi