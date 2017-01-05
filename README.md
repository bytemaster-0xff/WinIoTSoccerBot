# Win IoT Soccer Bot Repo
### Tampa IoT Society

Contact the project owner to contribute or get access to our Slack channel and help out!

For more details please review our documentation directory.

Slack Channel: https://soccerbot.slack.com/signup

Added custom firmware for mBot with the following features:
* Send both left and right motor in one command, previous version took two calls and we saw the robot twist left/right while the first motor was stopped and the other was stopping
* Added the ability to set the mode through the API

The first thing the app does is requests the version from the mBot.  Our custom firmware will start at 5.0 so if it returns 5.0 it will use our enhanced features otherwise it will use the default firmware on the mBot 


