using SoccerBotApp.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Linq;
using Windows.Devices.Bluetooth;

namespace SoccerBotApp.Devices
{
    public class SoccerBotBluetoothDevice : INotifyPropertyChanged
    {
        public event EventHandler<string> Disconnected;
        public event EventHandler<byte[]> MessageReceived;
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<SoccerBotBluetoothDevice> DeviceConnected;

        Models.Message _currentIncomingMessage;

        public RelayCommand _sendCommand;

        StreamSocket _socket = null;
        DataWriter _writer = null;

        DeviceInformation _deviceInfo;

        RfcommDeviceService _deviceService = null;

        public ObservableCollection<Models.Message> IncomingMessages { get; private set; }
        public ObservableCollection<Models.Message> OutgoingMessages { get; private set; }

        public enum States
        {
            Connecting,
            Connected,
            Disconnected,
        }

        public SoccerBotBluetoothDevice()
        {
            State = States.Disconnected;
            DisconnectCommand = RelayCommand.Create(Disconnect);
            SendCommand = RelayCommand.Create(SendCommands);
            ConnectCommand = RelayCommand.Create(Connect);
            NotifyUserMessage = "Idle";
            IncomingMessages = new ObservableCollection<Models.Message>();
            OutgoingMessages = new ObservableCollection<Models.Message>();

        }

        public SoccerBotBluetoothDevice(DeviceInformation deviceInfo) : this()
        {
            _deviceInfo = deviceInfo;
            Id = _deviceInfo.Id;
            DeviceName = _deviceInfo.Name;
        }


        private async void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            //TODO: Should be a design time check and not run this.
            if (App.TheApp != null && App.TheApp.Dispatcher != null)
            {
                await App.TheApp.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                });
            }
        }

        public async Task<bool> ConnectAsync(RfcommDeviceService deviceService)
        {
            _deviceService = deviceService;
            _socket = new StreamSocket();

            State = States.Connecting;

            try
            {
                await _socket.ConnectAsync(deviceService.ConnectionHostName, deviceService.ConnectionServiceName);
                _writer = new DataWriter(_socket.OutputStream);

                var dataReader = new DataReader(_socket.InputStream);
                dataReader.InputStreamOptions = InputStreamOptions.Partial;
                ReceiveDataAsync(dataReader);
                State = States.Connected;

                NotifyUserMessage = "Connected!";
                ErrorMessage = String.Empty;
                return true;
            }
            catch (Exception ex)
            {
                NotifyUserMessage = String.Empty;

                ErrorMessage = ex.Message;
                return false;
            }
        }
        
        private async void ReceiveDataAsync(DataReader chatReader)
        {
            try
            {
                uint maxBufferSize = 255;
                var size = await chatReader.LoadAsync(maxBufferSize);

                var buffer = new byte[size];

                chatReader.ReadBytes(buffer);

                if(_currentIncomingMessage == null)
                {
                    _currentIncomingMessage = new Models.Message();
                }

                foreach(var value in buffer)
                {
                    _currentIncomingMessage.AddByte(value);
                    if(_currentIncomingMessage.EndsWithCRLF())
                    {
                        MessageReceived?.Invoke(this, buffer);
                        IncomingMessages.Add(_currentIncomingMessage);
                        _currentIncomingMessage = new Models.Message();
                    }
                }

                ReceiveDataAsync(chatReader);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Could not connect {ex.Message}";
                State = States.Disconnected;
                lock (this)
                {
                    if (_socket == null)
                    {
                        // Do not print anything here -  the user closed the socket.

                        // HResult = 0x80072745 - catch this (remote device disconnect) ex = {"An established connection was aborted by the software in your host machine. (Exception from HRESULT: 0x80072745)"}
                    }
                    else
                    {
                        Disconnected?.Invoke(this, "Remote device terminated connection - make sure only one instance of server is running on remote device");
                    }
                }
            }
        }

        /// <summary>
        /// Invoked once the user has selected the device to connect to.
        /// Once the user has selected the device,
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Connect()
        {
            // Perform device access checks before trying to get the device.
            // First, we check if consent has been explicitly denied by the user.
            DeviceAccessStatus accessStatus = DeviceAccessInformation.CreateFromId(Id).CurrentStatus;
            if (accessStatus == DeviceAccessStatus.DeniedByUser)
            {
                ErrorMessage = "This app does not have access to connect to the remote device (please grant access in Settings > Privacy > Other Devices";
                return;
            }

            BluetoothDevice bluetoothDevice = null;

            // If not, try to get the Bluetooth device
            try
            {
                bluetoothDevice = await BluetoothDevice.FromIdAsync(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }

            // If we were unable to get a valid Bluetooth device object,
            // it's most likely because the user has specified that all unpaired devices
            // should not be interacted with.
            if (bluetoothDevice == null)
            {
                ErrorMessage = "Bluetooth Device returned null. Access Status = " + accessStatus.ToString();
                return;
            }

            var services = await bluetoothDevice.GetRfcommServicesAsync();
            if (!services.Services.Any())
            {
                ErrorMessage = "Could not discover any remote devices.";
                return;
            }

            var commService = services.Services.FirstOrDefault();

            if (await ConnectAsync(commService))
            {
                await App.TheApp.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    DeviceConnected?.Invoke(this, this);                    
                });
            }
        }

        public void Disconnect()
        {
            if (_writer != null)
            {
                _writer.DetachStream();
                _writer = null;
            }

            if (_deviceService != null)
            {
                _deviceService.Dispose();
                _deviceService = null;
            }
            lock (this)
            {
                if (_socket != null)
                {
                    _socket.Dispose();
                    _socket = null;
                }
            }
        }

        public async Task WriteBuffer(byte[] buffer)
        {
            _writer.WriteBuffer(buffer.AsBuffer());
            await _writer.StoreAsync();
            var result = await _writer.FlushAsync();
        }

        public async void SendCommands()
        {
            var msg = Protocols.mBlockMessage.CreateMessage(Protocols.mBlockMessage.CommandTypes.Get, Protocols.mBlockMessage.Devices.ULTRASONIC_SENSOR, 0x03);
            OutgoingMessages.Add(msg);
            await WriteBuffer(msg.Buffer);
        }

        public void Update(DeviceInformationUpdate update)
        {

        }

        private States _state;
        public States State
        {
            get { return _state; }
            set
            {
                _state = value;
            }
        }


        public RelayCommand ConnectCommand { get; private set; }
        public RelayCommand DisconnectCommand { get; private set; }
        public RelayCommand SendCommand { get; private set; }


        private String _notifyUserMessage;

        public String NotifyUserMessage
        {
            get { return _notifyUserMessage; }
            set
            {
                _notifyUserMessage = value;

                RaisePropertyChanged();
            }
        }

        private String _errorMessage;
        public String ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                RaisePropertyChanged();
            }
        }

        private String _name;
        public String DeviceName
        {
            set { _name = value; RaisePropertyChanged(); }
            get { return _name; }
        }

        private String _id;
        public String Id
        {
            set { _id = value; RaisePropertyChanged(); }
            get { return _id; }
        }
    }
}
