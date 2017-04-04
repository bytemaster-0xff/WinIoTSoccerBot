# Sensor Board V1.0

## Known Issues and Work arounds
Just like Software, Hardware can have bugs in it as well, unfortunately sometimes these aren't discovered until the board is actually created.  Our V1.0 prototype has a couple bugs but have relatively easy work arounds

### PLEASE READ AND UNDERSTAND PRIOR TO ASSEMBLING YOUR BOARD
Bug #1 - The three IR sensors on the right hand side of the board have their pads "backwards", Work-Around - Install the IR Sensor Upside Down, you have for sensor boards with the pins coming out the bottom rather than the top, these are for the right top, right, right rear and rear.   The IR Sensors on the Right side will appear upside down but will function just fine.  The Rear sensor needs to be installed on the top of the board so it will provide clearance for the USB and Network ports.
Bug #2 - There is a small gap on the underside of the board between the center pin on the front IR sensor.  This will be fixed prior to distribution of the Board.


## Step 1 - Organize and Identify Parts
There are 4 distinct parts that make up our sensor board
1) Main Connector for Raspberry Pi
2) 8x IR Sensors
3)  HMC5983 Comppass (attached to I2C Bus)
4) Right Angle 4 Pin Connector - To Connect the mBot to our Raspberry Pi
5) 2x 2904 NPN Transistors - Used to switch power on/off of the IR Sensors
6) 2x 1K or 470K Resistor (Your kit may contain either one of these) - These are used to limit the current flow through the Transistor
7) 4x 4 Pin Connector


## Step 2 - Install the IR Sensors
In this step you will be installing 8 IR Sensors on your Sensor Board, these will be used to detect obstacles within a small distance of your bot.

1. Lay the board on a flat surface with the top side facing up.
1. Locate the 8 IR Sensors, for of them will have the pins facing up and four will have them facing down.
1. You will need to bend the pins so they are pointing straight up or down based on the sensor you have, easiest way to do this is to take a small pliers and just bend them, be careful so you don't break them. 
1. Next take the four IR Sensors with the pins facing up, insert them in the holes labeled REARLEFT, LEFT, FRONTLEFT, FRONT.
1. Take three of the IR Sensors with the pins facing down, turn them over so the components are facing down, and place them in the holes labled RIGHTREAR, RIGHT, FRONTRIGHT
1. Once you are confident that the sensor boards are laying flat, solder each of the pins.


1. Take the final IR Sensor with the pins facing down and place it on the top of the board in the REAR slot.





## Step 3 - Install the 4 Pin Right Angle Connector

## Step 4 - Install the 4 4-Pin Accessory Connector

## Step 5 - Install the Raspberry Pi to Sensor Board Connector.