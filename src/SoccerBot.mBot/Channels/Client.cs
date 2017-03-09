using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using SoccerBot.Core;
using System.Threading;
using System.Diagnostics;
using LagoVista.Core.PlatformSupport;
using SoccerBot.Core.Interfaces;
using SoccerBot.Core.Models;
using SoccerBot.Core.Protocols;
using SoccerBot.Core.Messages;

namespace SoccerBot.mBot.Channels
{
    public class Client : IDisposable
    {
        public event EventHandler<NetworkMessage> MessageRecevied;

        const int MAX_BUFFER_SIZE = 1024;

        TimeSpan CONNECTION_TIMEOUT = TimeSpan.FromSeconds(5);

        StreamSocket _socket;
        StreamReader _reader;
        StreamWriter _writer;
        Stream _inputStream;
        Stream _outputStream;
        Task _listenerTask;
        CancellationTokenSource _cancelListenerSource;
        char[] _readBuffer;
        DateTime _lastMessageDateStamp;
        ISoccerBotLogger _logger;
        MessageParser _parser;

        private Client(StreamSocket socket, ISoccerBotLogger logger)
        {
            _socket = socket;
            _logger = logger;
            _parser = new MessageParser();
            _parser.MessageReady += _parser_MessageReady;

            _inputStream = socket.InputStream.AsStreamForRead();
            _reader = new StreamReader(_inputStream);

            _outputStream = socket.OutputStream.AsStreamForWrite();
            _writer = new StreamWriter(_outputStream);

            _lastMessageDateStamp = DateTime.Now;

            _readBuffer = new char[MAX_BUFFER_SIZE];
        }

        private async void _parser_MessageReady(object sender, NetworkMessage e)
        {
            if(e.MessageTypeCode == SystemMessages.Ping)
            {
                await Write(SystemMessages.CreatePong().GetBuffer());
                _logger.NotifyUserInfo("Client_MessageReceived", "Ping Received >> Pong Sent");
            }
            else
            {
                MessageRecevied?.Invoke(sender, e);
            }


            _lastMessageDateStamp = DateTime.Now;
            
        }

        public static Client Create(StreamSocket socket, ISoccerBotLogger logger)
        {
            return new Client(socket, logger);
        }

        public void StartListening()
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
                    catch(Exception ex)
                    {
                        running = false;
                        _logger.NotifyUserError("Client_Listening", ex.Message);
                    }
                }

            });

            _listenerTask.Start();
        }

        public bool IsConnected
        {
            get { return (DateTime.Now - _lastMessageDateStamp) < CONNECTION_TIMEOUT; }
        }

        public void Disconnect()
        {
            _cancelListenerSource.Cancel();
        }

        public async Task Write(byte[] buffer)
        {
            if (_writer != null)
            {
                try
                {
                    await _writer.WriteAsync(buffer.ToCharArray());
                    await _writer.FlushAsync();
                }
                catch(Exception )
                {
                    Disconnect();
                }
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
