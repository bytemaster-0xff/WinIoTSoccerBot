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

            RefreshSensorsCommand = RelayCommand.CreateAsync(RefreshSensorsAsync);

            Name = "mSoccerBot";            
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

        private  void ProcessBuffer(byte[] buffer)
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
                    Logger.Instance.NotifyUserInfo("mBlock", "<<< " + _currentIncomingMessage.MessageHexString);
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

        public async void RequestSonar()
        {
            var msg = Protocols.mBlockOutgingMessage.CreateMessage(Protocols.mBlockOutgingMessage.CommandTypes.Get, Protocols.mBlockOutgingMessage.Devices.ULTRASONIC_SENSOR, 0x03);
            await SendMessage(msg);            
        }
        

        public async Task MoveFowardAsync(byte speed)
        {
            var msg = Protocols.mBlockOutgingMessage.CreateMessage(Protocols.mBlockOutgingMessage.CommandTypes.Get, Protocols.mBlockOutgingMessage.Devices.ULTRASONIC_SENSOR, 0x03);
            await SendMessage(msg);
        }

        public async Task MoveBackwardsAsync(byte speed)
        {
            var msg = Protocols.mBlockOutgingMessage.CreateMessage(Protocols.mBlockOutgingMessage.CommandTypes.Get, Protocols.mBlockOutgingMessage.Devices.ULTRASONIC_SENSOR, 0x03);
            await SendMessage(msg);
        }

        public async Task TurnLeftAsync(float seconds)
        {
            var msg = Protocols.mBlockOutgingMessage.CreateMessage(Protocols.mBlockOutgingMessage.CommandTypes.Get, Protocols.mBlockOutgingMessage.Devices.ULTRASONIC_SENSOR, 0x03);
            await SendMessage(msg);
        }

        public async Task TurnRightAsync(float seconds)
        {
            var msg = Protocols.mBlockOutgingMessage.CreateMessage(Protocols.mBlockOutgingMessage.CommandTypes.Get, Protocols.mBlockOutgingMessage.Devices.ULTRASONIC_SENSOR, 0x03);
            await SendMessage(msg);
        }

        public async Task StopAsync()
        {
            var msg = Protocols.mBlockOutgingMessage.CreateMessage(Protocols.mBlockOutgingMessage.CommandTypes.Get, Protocols.mBlockOutgingMessage.Devices.ULTRASONIC_SENSOR, 0x03);
            await SendMessage(msg);
        }

        public async Task RefreshSensorsAsync()
        {
            var msg = Protocols.mBlockOutgingMessage.CreateMessage(Protocols.mBlockOutgingMessage.CommandTypes.Get, Protocols.mBlockOutgingMessage.Devices.ULTRASONIC_SENSOR, 0x03);
            await SendMessage(msg);
        }

        public RelayCommand RefreshSensorsCommand { get; private set; }
    }
}
