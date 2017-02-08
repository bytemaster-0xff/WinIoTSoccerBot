# Windows 10 IoT Core - Soccer Bot Repo
### Shopping List


## Supported Platforms
The primary platform for our Soccer Bot will be the mBot platform.  There are three varients that are supported, we encourage you to build your own robot
but selecting one of these platforms will make things easier and the configuration will be documented here.

What's nice about these platforms is that they are arduino based which means we can provide custom firmware which is included in this repository. 


### [Basic Version](https://www.amazon.com/Makeblock-DIY-mBot-Kit-Bluetooth/dp/B01FS7BGJI/ref=sr_1_3?ie=UTF8&qid=1485812877&sr=8-3&keywords=mBot)
![Alt](Documentation/BasicVersion.jpg)

### [Rover Version](https://www.amazon.com/dp/B00W6Y194Y?psc=1)
![Alt](Documentation/RoverVersion.jpg)

### [Ranger Version](https://www.amazon.com/Makeblock-Ranger-Transformable-Educational-Robot/dp/B01DY3OTHO/ref=sr_1_6?ie=UTF8&qid=1485812877&sr=8-6&keywords=mBot)
![Alt](Documentation/RangerBot.jpg)


## The Brains

The built in board on the mBot (or your platform) will be responsible for controlling the motors and some sensors to move the robot.  Our intention is to build an smart robot that can navigate on it's on (mostly).  

We will be using a Raspberry Pi running Windows 10 IoT core to create the intelligence.  This will not only provide a more friendly environemnt to build our logic - C# but also provides considerably more storage as well as network capability.

For this you will need a Rasperry Pi capable running Windows 10 IoT core.  We recommend Raspberry Pi 3 since it has built in WiFi.

### [Rasberry Pi 3](https://www.amazon.com/Raspberry-Computer-Performance-Anodized-Heatsink/dp/B01KGMMI1A/ref=sr_1_1?s=electronics&ie=UTF8&qid=1485813892&sr=1-1-spons&keywords=Raspberry+Pi+3&psc=1)
![Alt](Documentation/RaspberyPi.jpg)

You will also need an Micro SD card to install the firmware.  Below find an example.

### [Micro SD Card (32GB Minimum)](https://www.amazon.com/SanDisk-microSDXC-Standard-Packaging-SDSQUNC-064G-GN6MA/dp/B010Q588D4/ref=sr_1_1?s=pc&ie=UTF8&qid=1485814127&sr=1-1&keywords=Micro+SD+card)
![Alt](Documentation/MicroSDCard.jpg)


### [M4x30 Standoffs](https://www.amazon.com/gp/product/B0177VGC92/ref=oh_aui_search_detailpage?ie=UTF8&psc=1)
We will be using these to attach the Raspbery Pi to the mBot
![Alt](Documentation/Standoffs.jpg)



