using LagoVista.Core.Networking.Models;
using SoccerBot.Core.Channels;
using SoccerBot.Core.Interfaces;
using System;
using System.IO;
using SoccerBot.Core;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using SoccerBot.Core.Models;
using SoccerBot.Core.Protocols;
using SoccerBot.Core.Messages;

namespace SoccerBot.UWP.Channels
{
    public class TCPIPChannel : ChannelBase, IDisposable
    {
        uPnPDevice _remoteDevice;

        const int MAX_BUFFER_SIZE = 1024;

        StreamSocket _socket;
        StreamReader _reader;
        StreamWriter _writer;
        Stream _inputStream;
        Stream _outputStream;

        char[] _readBuffer;

        Timer _pingTimer;

        CancellationTokenSource _cancelListenerSource;
        Task _listenerTask;

        MessageParser _parser;

        ISoccerBotLogger _logger;
        public TCPIPChannel(uPnPDevice device, ISoccerBotLogger logger)
        {
            _remoteDevice = device;
            DeviceName = device.FriendlyName;
            Id = device.UDN + " " + device.IPAddress;
            _logger = logger;
            _logger.NotifyUserInfo("TCPIP Channel", $"Created Device for: {DeviceName}");

            _parser = new MessageParser();
            _parser.MessageReady += _parser_MessageReady;
        }

        private void _parser_MessageReady(object sender, NetworkMessage e)
        {
            _logger.NotifyUserInfo("Client_MessageReceived", "Message Received");

            RaiseNetworkMessageReceived(e);
        }

        public void ReceiveData()
        {
            _cancelListenerSource = new CancellationTokenSource();
            _listenerTask = new Task(async () =>
            {

                var running = true;
                while (running)
                {
                    try
                    {
                        var readTask = _reader.ReadAsync(_readBuffer, 0, MAX_BUFFER_SIZE);
                        readTask.Wait(_cancelListenerSource.Token);
                        var bytesRead = await readTask;

                        var byteBuffer = _readBuffer.ToByteArray(0, bytesRead);
                        _parser.Parse(byteBuffer);
                    }
                    catch (OperationCanceledException)
                    {
                        running = false;
                        /* Task Cancellation */
                    }
                    catch (Exception ex)
                    {
                        running = false;
                        _logger.NotifyUserError("Client_Listening", ex.Message);
                    }
                }
            });

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

                _pingTimer = new Timer(Ping, null, 0, 2500);

                InvokeConnected();

                return true;
            }
            catch (Exception ex)
            {
                _logger.NotifyUserError("TCPIPChannel_Listen", ex.Message);
                return false;
            }
        }

        private async void Ping(Object state)
        {
            await WriteBuffer(SystemMessages.CreatePing().GetBuffer());

            _logger.NotifyUserError("TCPIPChannel_Ping", "Ping Sent");
        }

        public override void Disconnect()
        {
            if (_pingTimer != null)
            {
                _pingTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _pingTimer.Dispose();
                _pingTimer = null;
            }

            _cancelListenerSource.Cancel();
        }

        public override Task DisconnectAsync()
        {
            return Task.FromResult(default(object));
        }

        public async override Task WriteBuffer(byte[] buffer)
        {
            try
            {
                if (_writer != null)
                {
                    await _writer.WriteAsync(buffer.ToCharArray());
                    await _writer.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.NotifyUserError("TCPIPChannel_WriteBuffer", ex.Message);
                Disconnect();
            }
        }

        public void Dispose()
        {
            lock (this)
            {
                if (_reader != null)
                {
                    _reader.Dispose();
                    _reader = null;
                }

                if (_writer != null)
                {
                    _writer.Dispose();
                    _writer = null;
                }

                if (_inputStream != null)
                {
                    _inputStream.Dispose();
                    _inputStream = null;
                }

                if (_outputStream != null)
                {
                    _outputStream.Dispose();
                    _outputStream = null;
                }

                if (_socket != null)
                {
                    _socket.Dispose();
                    _socket = null;
                }
            }
        }
    }
}
