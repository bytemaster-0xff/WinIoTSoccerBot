# Remote Client App with Raspberry Pi

To get started with the desktop app, please check out [Windows 10 Desktop Control App](DesktopApp.md).

Our Desktop UWP App can Connect to the mBot either by BlueTooth or WiFi, this section will talk about connecting via WiFi.

To connect via WiFi, you will need to:
1. Connect your Raspberry Pi to the network
2. Install the application on to the Raspberry Pi and make connections between the Raspberry Pi and the Bot as show [in this document](ConnectWinIoT.md).

It will be very help to use the [Remote Desktop App]((https://www.microsoft.com/en-us/store/p/windows-iot-remote-client/9nblggh5mnxz)) to ensure your application is running.

Our Raspberry Pi application uses the [uPnP (Universal Plug and Play) Discovery](https://en.wikipedia.org/wiki/Universal_Plug_and_Play#Discovery) protocol to search for mBots on the current network.  The app will display all the devices it finds in the left hand column.

![Alt](Documentation/WiFiConnections.png)

Note ByteMaster002 - it has two strings underneath the name, the first is GUID or UUID for the device (randomally generated) and the second is it's IP address.

When you Click on "Connect" you will see the following Screen:

![Alt](Documentation/WiFiControls.png)

## Some notes about connectivity:

### Tones from the mBot:

There are three tones you should listen for (after hearing all three you wil be able to recognize them):

1. Medium Tone - Your Raspberry Pi successfully connected to your mBot
2. High Tone - The remote application has successfully connected to the Raspberry Pi Win 10 IoT Core App.
3. Low Tone - The remote applciation has disconnected from the Raspberry Pi.

### Lights RGB Lights on mBot:

1. No Light - mBot Started and No Connectivity to Raspberry Pi App
2. Yellow Light - Raspberry Pi Connected to mBot
3. Green Light - Remote App Connected to Raspberry Pi
4. Red Light - mBot Not Connected to Raspberry Pi.


Note: In the current software the lights are driven by the Raspberry Pi.  If the Raspberry Pi is not connected to the mBot, it can not control the lights.