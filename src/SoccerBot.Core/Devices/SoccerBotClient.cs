using LagoVista.Core.Models.Drawing;
using SoccerBot.Core.Interfaces;
using SoccerBot.Core.Messages;
using System.Diagnostics;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LagoVista.Core.PlatformSupport;

namespace SoccerBot.Core.Devices
{
    public class SoccerBotClient : SoccerBotBase, ISoccerBot
    {
        IChannel _channel;
        ISoccerBotLogger _logger;

        Commands _currentCommand = Commands.Stop;


        public SoccerBotClient(IChannel channel, ISoccerBotLogger logger, string pin = "9999")
        {
            _channel = channel;
            _channel.NetworkMessageReceived += _channel_NetworkMessageReceived;
            _logger = logger;
        }

        private void _channel_NetworkMessageReceived(object sender, Models.NetworkMessage e)
        {
            if (e.MessageTypeCode == Core.Messages.SensorData.MessageTypeId)
            {
                SensorData = e.DeserializePayload<SensorData>();
            }
        }

        ISensor _compass;
        public ISensor Compass
        {
            get { return _compass; }
            set
            {
                _compass = value;
                RaisePropertyChanged();
            }
        }

        ISensor _frontSensor;
        public ISensor FrontSonar
        {
            get { return _frontSensor; }
            set
            {
                _frontSensor = value;
                RaisePropertyChanged();
            }
        }

        SensorData _sensorData;
        public SensorData SensorData
        {
            get { return _sensorData; }
            set
            {
                _sensorData = value;
                RaisePropertyChanged();
            }
        }

        public void Move(short speed = 0, short? relativeHeading = 0, short? absoluteHeading = 0, short? duration = 0)
        {
            var moveMessage = Messages.Move.Create(speed, relativeHeading, absoluteHeading, duration);
            _channel.WriteBuffer(moveMessage.GetBuffer());
        }

        public void PlayTone(short frequency)
        {

        }

        public void Reset()
        {

        }

        public void SetLED(byte index, Color color)
        {

        }

        public void Stop()
        {
            var moveMessage = Messages.Stop.Create();
            _channel.WriteBuffer(moveMessage.GetBuffer());
        }

        protected override void RefreshSensors()
        {

        }

        protected override void SendCommand(object inputCmd)
        {
            var cmd = (Commands)inputCmd;
            _currentCommand = cmd;
            switch (cmd)
            {
                case Commands.Forward: Move(Speed, 0); break;
                case Commands.Stop: Speed = 0; Move(0, 0); break;
                case Commands.Left: Move(Speed, 270); break;
                case Commands.Right: Move(Speed, 90); break;
                case Commands.Backwards: Move(Speed, 180); break;
                case Commands.Reset: Reset(); break;
            }
        }

        protected override void SpeedUpdated(short speed)
        {
            Speed = speed;
            SendCommand(_currentCommand);
        }
    }
}
