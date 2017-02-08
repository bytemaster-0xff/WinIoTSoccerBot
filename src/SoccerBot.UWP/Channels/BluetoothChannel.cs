using SoccerBot.Core.Channels;
using SoccerBot.Core.Interfaces;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace SoccerBot.UWP.Channels
{
    public class BluetoothChannel : ChannelBase
    {
        StreamSocket _socket = null;
        DataWriter _writer = null;
        ISoccerBotLogger _logger;
        DeviceInformation _deviceInfo;
        RfcommDeviceService _deviceService = null;

        public BluetoothChannel(DeviceInformation deviceInfo, ISoccerBotLogger logger)
        {
            _logger = logger;
            _deviceInfo = deviceInfo;
            Id = _deviceInfo.Id;
            DeviceName = _deviceInfo.Name;

            State = States.Disconnected;
            _logger.NotifyUserInfo("BT Channel", "Created");
        }

        public override async Task<bool> ConnectAsync()
        {
            _socket = new StreamSocket();

            State = States.Connecting;

            try
            {
                await _socket.ConnectAsync(_deviceService.ConnectionHostName, _deviceService.ConnectionServiceName);
                _writer = new DataWriter(_socket.OutputStream);

                var dataReader = new DataReader(_socket.InputStream);
                dataReader.InputStreamOptions = InputStreamOptions.Partial;
                ReceiveDataAsync(dataReader);
                State = States.Connected;
                ConnectCommand.Enabled = false;
                _logger.NotifyUserInfo("BT Channel", "Connected!");
                return true;
            }
            catch (Exception ex)
            {
                _logger.NotifyUserError("BT Channel", ex.Message);
                return false;
            }
        }

        private async void ReceiveDataAsync(DataReader reader)
        {
            try
            {
                uint maxBufferSize = 255;
                var size = await reader.LoadAsync(maxBufferSize);

                var buffer = new byte[size];
                reader.ReadBytes(buffer);
                RaiseMessageReceived(buffer);
                ReceiveDataAsync(reader);
            }
            catch (Exception ex)
            {
                _logger.NotifyUserError("BT Channel", "Could Not Connect" + ex.Message);
                State = States.Disconnected;
                InvokeDisconnected();
            }
        }

        public override async void Connect()
        {
            var accessStatus = DeviceAccessInformation.CreateFromId(Id).CurrentStatus;
            if (accessStatus == DeviceAccessStatus.DeniedByUser)
            {
                _logger.NotifyUserError("BT Channel", "This app does not have access to connect to the remote device (please grant access in Settings > Privacy > Other Devices");
                return;
            }

            try
            {
                var bluetoothDevice = await BluetoothDevice.FromIdAsync(Id);
                if (bluetoothDevice == null)
                {
                    _logger.NotifyUserError("BT Channel", "Bluetooth Device returned null. Access Status = " + accessStatus.ToString());
                    return;
                }

                var services = await bluetoothDevice.GetRfcommServicesAsync();
                if (!services.Services.Any())
                {
                    _logger.NotifyUserError("BT Channel", "Could not discover any remote devices.");
                    return;
                }

                _deviceService = services.Services.FirstOrDefault();

                if (await ConnectAsync())
                {
                    InvokeConnected();
                }

            }
            catch (Exception ex)
            {
                _loggerfs.NotifyUserError("BT Channel", ex.Message);
                return;
            }
        }

        public void Update(DeviceInformationUpdate udpate)
        {

        }

        public override async Task WriteBuffer(byte[] buffer)
        {
            _writer.WriteBuffer(buffer.AsBuffer());
            await _writer.StoreAsync();
            var result = await _writer.FlushAsync();
        }

        public override void Disconnect()
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

            ConnectCommand.Enabled = true;
        }

        public override Task DisconnectAsync()
        {
            Disconnect();
            return Task.FromResult(default(object));
        }

    }
}
