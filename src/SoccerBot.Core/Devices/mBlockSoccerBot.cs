using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using SoccerBot.Core.Interfaces;
using SoccerBot.Core.Protocols;
using LagoVista.Core.Commanding;

namespace SoccerBot.Core.Devices
{
    public class mBlockSoccerBot : SoccerBotBase, INotifyPropertyChanged, ISoccerBotCommands
    {
        IChannel _channel;
        ISoccerBotLogger _logger;
        mBlockIncomingMessage _currentIncomingMessage;

        public ObservableCollection<mBlockIncomingMessage> IncomingMessages { get; private set; }
        public ObservableCollection<mBlockOutgingMessage> OutgoingMessages { get; private set; }

        DateTime _start;

        string _pin;

        Timer _timer;
        bool _connectedToBot;
        DateTime? _lastVersionCheck;

        public mBlockSoccerBot(IChannel channel, ISoccerBotLogger logger, string pin = "9999") : this()
        {
            _pin = pin;
            _logger = logger;
            _channel = channel;
            _channel.MessageReceived += _channel_MessageReceived;
            Name = "mSoccerBot";

            _timer = new Timer((state) => { RequestVersion(); }, null, 0, 5000);

            ModeACommand = RelayCommand.Create(SendModeA);
            ModeBCommand = RelayCommand.Create(SendModeB);
            ModeCCommand = RelayCommand.Create(SendModeC);
            SendLEDMessageCommand = RelayCommand.Create(SendLEDMessage);

            _start = DateTime.Now;
        }

        public override void PlayTone(short frequency)
        {
            var msg = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.TONE, frequency);
            SendMessage(msg);

        }


        protected override void RefreshSensors()
        {
            RequestSonar();
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

        public List<mBlockOutgingMessage> _messageHandlers = new List<mBlockOutgingMessage>();

        private void ProcessMessage(mBlockIncomingMessage message)
        {
            lock (_messageHandlers)
            {
                var associatedIncomingHandler = _messageHandlers.Where(msg => msg.MessageSerialNumber == message.MessageSerialNumber).FirstOrDefault();
                if (associatedIncomingHandler != null)
                {
                    associatedIncomingHandler.Handler(message);
                    _messageHandlers.Remove(associatedIncomingHandler);
                }
            }

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
                    Debug.WriteLine(String.Format("{0:000000} <<<", (DateTime.Now - _start).TotalMilliseconds) + _currentIncomingMessage.MessageHexString);
                    _logger.NotifyUserInfo("mBlock", "<<< " + _currentIncomingMessage.MessageHexString);

                    if (_currentIncomingMessage.BufferSize > 4)
                    {
                        _currentIncomingMessage.MessageSerialNumber = _currentIncomingMessage.Buffer[2];
                        ProcessMessage(_currentIncomingMessage);
                    }

                    _currentIncomingMessage = new mBlockIncomingMessage();
                }
            }
        }

        protected override void SpeedUpdated(short speed)
        {
            if (CurrentState != Commands.Stop)
            {
                SendCommand(CurrentState);
            }
        }      


        private async void SendMessage(mBlockOutgingMessage msg)
        {
            lock (_messageHandlers)
            {
                if (msg.Handler != null)
                {
                    _messageHandlers.Add(msg);
                }
            }

            try
            {
                OutgoingMessages.Add(msg);
                Debug.WriteLine(String.Format("{0:000000} >>>", (DateTime.Now - _start).TotalMilliseconds) + msg.MessageHexString);
                _logger.NotifyUserInfo("mBlock", ">>> " + msg.MessageHexString);
                await _channel.WriteBuffer(msg.Buffer);
            }
            catch(Exception ex)
            {
                _logger.NotifyUserError("mBlock_SendMessage", ex.Message);
                _logger.NotifyUserInfo("mBlock", "DISCONNECTED!");
                _channel.State = States.Disconnected;
            }
        }

        private async void SendMotorPower(int leftMotor, int rightMotor)
        {
            leftMotor = Convert.ToInt32((leftMotor * 255.0) / 100.0f);
            rightMotor = Convert.ToInt32((rightMotor * 255.0) / 100.0f);

            if (DeviceMode == DeviceModes.CustomFirmware)
            {
                var buffer = new byte[4];
                buffer[0] = BitConverter.GetBytes(leftMotor)[0]; 
                buffer[1] = BitConverter.GetBytes(leftMotor)[1];
                buffer[2] = BitConverter.GetBytes(rightMotor)[0];
                buffer[3] = BitConverter.GetBytes(rightMotor)[1];

                var message = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.MOTOR, mBlockIncomingMessage.Ports.MBOTH, buffer);
                SendMessage(message);
            }
            else if(DeviceMode == DeviceModes.Normal)
            {
                /* Need to give it about 50ms for the message to come through */
                var payload = BitConverter.GetBytes((short)leftMotor);
                var leftMotorMessage = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.MOTOR, mBlockIncomingMessage.Ports.M1, payload);
                SendMessage(leftMotorMessage);
                await Task.Delay(15);
                payload = BitConverter.GetBytes((short)rightMotor);
                var rightMotorMessage = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.MOTOR, mBlockIncomingMessage.Ports.M2, payload);
                SendMessage(rightMotorMessage);
                await Task.Delay(15);
            }
            else if(DeviceMode == DeviceModes.CustomFirmwareRanger)
            {
                
                var buffer = new byte[3];
                buffer[0] = (byte)mBlockOutgingMessage.Slots.SLOT1;
                buffer[1] = BitConverter.GetBytes(leftMotor)[0];
                buffer[2] = BitConverter.GetBytes(leftMotor)[1];
                var message = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.ENCODER_BOARD, 0, buffer);
                SendMessage(message);

                buffer[0] = (byte)mBlockOutgingMessage.Slots.SLOT2;
                buffer[1] = BitConverter.GetBytes(rightMotor)[0];
                buffer[2] = BitConverter.GetBytes(rightMotor)[1];
                message = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.ENCODER_BOARD, 0, buffer);
                SendMessage(message);
            }
        }

        public Commands CurrentState { get; set; }

        protected override void SendCommand(object inputCmd)
        {
            var cmd = (Commands)inputCmd;
            switch (cmd)
            {
                case Commands.Forward: SendMotorPower(-Speed, Speed); break;
                case Commands.Stop: SendMotorPower(0, 0); break;
                case Commands.Left: SendMotorPower(-Speed / 5, Speed); break;
                case Commands.Right: SendMotorPower(-Speed, Speed / 5); break;
                case Commands.Backwards: SendMotorPower(Speed, -Speed); break;
                case Commands.Reset: SendReset(); break;
            }

            CurrentState = cmd;
        }


        public void SendReset()
        {
            SendMessage(mBlockOutgingMessage.CreateMessage(mBlockIncomingMessage.CommandTypes.Reset, mBlockMessage.Devices.SYSTEM));
        }

        public void ProcessSonar(mBlockIncomingMessage message)
        {
            FrontIRSensor = Convert.ToInt32(_currentIncomingMessage.FloatPayload);
            
            var factor = Speed / 100;

            if (FrontIRSensor < (10 * factor) && CurrentState == Commands.Forward)
            {
                PauseRefreshTimer();
                SendCommand(Commands.Stop);
                StartRefreshTimer();
            }
        }

        public void ProcessVersion(mBlockIncomingMessage message)
        {
            if (!String.IsNullOrEmpty(message.StringPayload))
            {
                var parts = message.StringPayload.Split('.');
                int majorVersion;
                Int32.TryParse(parts[0], out majorVersion);

                if(majorVersion == 5)
                {
                    DeviceMode = DeviceModes.CustomFirmware;
                    APIMode = "Custom mBot";
                }
                else if(majorVersion == 9)
                {
                    DeviceMode = DeviceModes.CustomFirmwareRanger;
                    APIMode = "Ranger Firmware";
                }
                else
                {
                    APIMode = "Stock mBot";
                }

                FirmwareVersion = message.StringPayload;

                if(!_connectedToBot)
                {
                    PlayTone(294);
                }

                _connectedToBot = true;
                _lastVersionCheck = DateTime.Now;
            }
        }

        public void RequestSonar()
        {
            var msg = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Get, mBlockOutgingMessage.Devices.ULTRASONIC_SENSOR, mBlockMessage.Ports.PORT_3);
            //msg.Handler = ProcessSonar;
            //SendMessage(msg);
        }

        public void RequestVersion()
        {            
            var msg = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Get, mBlockOutgingMessage.Devices.VERSION);
            msg.Handler = ProcessVersion;
            SendMessage(msg);

            if(_lastVersionCheck == null || ((DateTime.Now - _lastVersionCheck) > TimeSpan.FromSeconds(12)))
            {
                _connectedToBot = false;
            }
        }

        public void SendModeA()
        {
            SendMessage(mBlockOutgingMessage.CreateMessage(mBlockIncomingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.SETMODE, mBlockMessage.Ports.MODE_A));
        }

        public void SendModeB()
        {
            SendMessage(mBlockOutgingMessage.CreateMessage(mBlockIncomingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.SETMODE, mBlockMessage.Ports.MODE_B));
        }

        public void SendModeC()
        {
            SendMessage(mBlockOutgingMessage.CreateMessage(mBlockIncomingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.SETMODE, mBlockMessage.Ports.MODE_C));
        }

        public void SendLEDMessage()
        {
            if (!String.IsNullOrEmpty(LEDMessage))
            {
                const int HEADER_LENGTH = 8;
                var payload = new byte[HEADER_LENGTH + LEDMessage.Length];
                payload[0] = 1; /* action 1 = DrawString, 2 = Draw Bitmap, 3 = Show Click*/  
                payload[1] = 9; /* 0-9 brightness */
                payload[2] = 1; /* 1 = On, 0 = Off color */
                payload[3] = 0; /* X Location */
                payload[4] = 0; /* X Location */
                payload[5] = 7; /* X Location */
                payload[6] = 0; /* Y Location */
                payload[7] = (byte)LEDMessage.Length;
                var bytes = System.Text.Encoding.UTF8.GetBytes(LEDMessage);

                var idx = 0;
                foreach(var ch in bytes)
                {
                    payload[idx + HEADER_LENGTH] = bytes[idx];
                    idx++;
                }
          
                var ledMessage = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.LED_MATRIX, 1, payload);
                SendMessage(ledMessage);
            }
        }

        public void SetRGBA0sync(byte r, byte g, byte b)
        {
            var payload = new byte[3] { r, g, b };
            var rgbMessage = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.RGBLED, 0, payload);
            SendMessage(rgbMessage);
        }


        public void SetRGBA1sync(byte r, byte g, byte b)
        {
            var payload = new byte[3] { r, g, b };
            var rgbMessage = mBlockOutgingMessage.CreateMessage(mBlockOutgingMessage.CommandTypes.Run, mBlockOutgingMessage.Devices.RGBLED, 1, payload);
            SendMessage(rgbMessage);
        }

        public RelayCommand ModeACommand { get; private set; }
        public RelayCommand ModeBCommand { get; private set; }
        public RelayCommand ModeCCommand { get; private set; }
        public RelayCommand PlayToneCommand { get; private set; }

        public RelayCommand SendLEDMessageCommand { get; private set; }

        public enum DeviceModes
        {
            Normal,
            CustomFirmware,
            CustomFirmwareRanger,
        }


        private DeviceModes _deviceMode;
        public DeviceModes DeviceMode
        {
            get { return _deviceMode; }
            set
            {
                _deviceMode = value;
                RaisePropertyChanged();
            }
        }

        private String _ledMessage = "HI";
        public String LEDMessage
        {
            get { return _ledMessage; }
            set
            {
                _ledMessage = value;
                RaisePropertyChanged();
            }
        }
    }
}
