using System.Collections.ObjectModel;
using System.ComponentModel;
using SoccerBotApp.Protocols;
using System.Diagnostics;
using System.Threading.Tasks;
using SoccerBotApp.Channels;
using SoccerBotApp.Utilities;
using System;

namespace SoccerBotApp.Devices
{
    public class mBlockSoccerBot : SoccerBotBase, INotifyPropertyChanged, ISoccerBotCommands
    {

        IChannel _channel;

        mBlockIncomingMessage _currentIncomingMessage;


        public ObservableCollection<mBlockIncomingMessage> IncomingMessages { get; private set; }
        public ObservableCollection<mBlockOutgingMessage> OutgoingMessages { get; private set; }


        public mBlockSoccerBot(IChannel channel) : this()
        {
            _channel = channel;
            _channel.MessageReceived += _channel_MessageReceived;
            Name = "mSoccerBot";
        }

        protected override void RefreshSensors()
        {
          //  RequestSonar();
        }

        private void _channel_MessageReceived(object sender, byte[] buffer)
        {
            ProcessBuffer(buffer);
        }

        public mBlockSoccerBot()
        {
            IncomingMessages = new ObservableCollection<mBlockIncomingMessage>();
            OutgoingMessages = new ObservableCollection<mBlockOutgingMessage>();
        }

        private void ProcessBuffer(byte[] buffer)
        {
            if (_currentIncomingMessage == null)
            {
                _currentIncomingMessage = new mBlockIncomingMessage();
            }

            foreach (var value in buffer)
            {
                /* Received message format
                 *  0xFF - Header Byte 1
                 *  0x55 - Header Byte 2
                 *  0xXX - Message index corresponding to request
                 *  0x0X - Payload Type - 1 byte 2 float 3 short 4 len+string 5 double
                 *  [0xXX....0xXX] Payload matcing size
                 *  0x0D
                 *  0x0A
                 */

                _currentIncomingMessage.AddByte(value);
                if (_currentIncomingMessage.EndsWithCRLF())
                {
                    IncomingMessages.Add(_currentIncomingMessage);
                    Debug.WriteLine(_currentIncomingMessage.FloatPayload);
                    FrontIRSensor = _currentIncomingMessage.FloatPayload;

                    if(FrontIRSensor < 10 && CurrentState != Commands.Stop)
                    {
                        SendCommand(Commands.Stop);
                    }

                    _currentIncomingMessage = new mBlockIncomingMessage();
                }
            }
        }


        private async Task SendMessage(mBlockOutgingMessage msg)
        {
            OutgoingMessages.Add(msg);
            Logger.Instance.NotifyUserInfo("mBlock", ">>> " + msg.MessageHexString);
            await _channel.WriteBuffer(msg.Buffer);
        }

        private async Task SendMotorPower(int leftMotor, int rightMotor)
        {
            var payload = BitConverter.GetBytes((short)leftMotor);
            var leftMotorMessage = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.MOTOR, mBlockIncomingMessage.Ports.M1, payload);
            await SendMessage(leftMotorMessage);
            payload = BitConverter.GetBytes((short)rightMotor);
            var rightMotorMessage = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.MOTOR, mBlockIncomingMessage.Ports.M2, payload);
            await SendMessage(rightMotorMessage);
        }

        public Commands CurrentState { get; set; }

        protected async override void SendCommand(Commands cmd)
        {
            switch (cmd)
            {
                case Commands.Forward: await SendMotorPower(-Speed, Speed); break;
                case Commands.Stop: await SendMotorPower(0, 0); break;
                case Commands.Left: await SendMotorPower(Speed, -Speed/5); break;
                case Commands.Right: await SendMotorPower(Speed/5, -Speed); break;
                case Commands.Backwards: await SendMotorPower(Speed, -Speed); break;
            }

            CurrentState = cmd;
        }

        public async void RequestSonar()
        {
            var msg = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Get, mBlockOutgingMessage.Devices.ULTRASONIC_SENSOR, mBlockMessage.Ports.PORT_3);
            await SendMessage(msg);
        }

        
        public async Task SetRGBAsync(byte r, byte g, byte b)
        {
            var payload = new byte[3] { r, g, b };
            var rgbMessage = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.MOTOR, mBlockIncomingMessage.Ports.M1, payload);
            await SendMessage(rgbMessage);
        }

        public async Task MoveBackwardsAsync(short speed)
        {
            var msg = Protocols.mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Get, mBlockOutgingMessage.Devices.ULTRASONIC_SENSOR, mBlockMessage.Ports.PORT_3);
            await SendMessage(msg);
        }
    }
}
