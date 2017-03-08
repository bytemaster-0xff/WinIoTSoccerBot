using LagoVista.Core.Networking.Models;
using SoccerBot.Core.Channels;
using SoccerBot.Core.Interfaces;
using System;
using System.IO;
using SoccerBot.Core;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace SoccerBot.UWP.Channels
{
    public class TCPIPChannel : ChannelBase
    {
        uPnPDevice _remoteDevice;

        const int MAX_BUFFER_SIZE = 1024;

        StreamSocket _socket;
        StreamReader _reader;
        StreamWriter _writer;
        Stream _inputStream;
        Stream _outputStream;

        char[] _readBuffer;


        CancellationTokenSource _cancellListenerSource;
        Task _listenerTask;

        ISoccerBotLogger _logger;
        public TCPIPChannel(uPnPDevice device, ISoccerBotLogger logger)
        {
            _remoteDevice = device;
            DeviceName = device.FriendlyName;
            Id = device.UDN + " " + device.IPAddress;
            _logger = logger;
            _logger.NotifyUserInfo("TCPIP Channel", $"Created Device for: {DeviceName}");
        }

        public void ReceiveData()
        {
            _cancellListenerSource = new CancellationTokenSource();
            _listenerTask = new Task(async () =>
            {
                var bytesRead = await _reader.ReadAsync(_readBuffer, 0, MAX_BUFFER_SIZE);

                RaiseMessageReceived(_readBuffer.ToByteArray(0, bytesRead));

            }, _cancellListenerSource.Token);

            _listenerTask.Start();
        }

        public async override void Connect()
        {
            await ConnectAsync();
        }

        public async override Task<bool> ConnectAsync()
        {
            try
            {
                _readBuffer = new char[MAX_BUFFER_SIZE];

                _socket = new Windows.Networking.Sockets.StreamSocket();
                var host = new Windows.Networking.HostName(_remoteDevice.IPAddress);
                var port = 9000;
                await _socket.ConnectAsync(host, port.ToString());

                _inputStream = _socket.InputStream.AsStreamForRead();
                _reader = new StreamReader(_inputStream);

                _outputStream = _socket.OutputStream.AsStreamForWrite();
                _writer = new StreamWriter(_outputStream);

                ReceiveData();

                return true;
            }
            catch(Exception ex)
            {
                _logger.NotifyUserError("TCPIPChannel_Listen", ex.Message);
                return false;
            }
        }

        public override void Disconnect()
        {

        }

        public override Task DisconnectAsync()
        {
            return Task.FromResult(default(object));
        }

        public async override Task WriteBuffer(byte[] buffer)
        {
            await _writer.WriteAsync(buffer.ToCharArray());
            await _writer.FlushAsync();
        }
    }
}
