# March 2017

We are working with very early software, the primary goal of this meeting is to establish base line connectivity between all the moving parts.  Once we get the software installed and configured, we will improve its stablility and robustness.  The basic connectivty mechanism should remain the same.


### Current Known Issues:
1. Early and Unstable - Connectivity Works, but is limited on error checking, retries and recovery
1. Open Security - Security will be very important, but it's early.  Our first cut (not implemented) will be to require a pin similar to BlueTooth to establish connections, for now you can connect to anyone elses robot.  Please be respectful. 
1. Motor Speed - Issue with Motor Speed Alogithm when coming from Joy Stick, the problem is in mBlockSoccerBot.cs on or around line 385 if someone can investigate

### Agenda

1. Distribute Mounting Panels
1. Discuss Status of Sensor Board
1. Ensure we have Good Network Connectivity With our Rasberry PIs
4. [Install and Use Raspberry Pi Remote Desktop Client](https://www.microsoft.com/en-us/store/p/windows-iot-remote-client/9nblggh5mnxz)
5. [Make sure Raspberry Pi Connects to mBot](ConnectWinIoT.md)
6. [Make sure we can use the Desktop App to Communicate with the Rasberry Pi](RemoteClient.md)
7. Connect up XBox Controller