using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using SoccerBot.Core;
using System.Threading;

namespace SoccerBot.mBot.Channels
{
    public class Client : IDisposable
    {
        public event EventHandler<byte> MessageRecevied;

        TimeSpan CONNECTION_TIMEOUT = TimeSpan.FromSeconds(5);

        StreamSocket _socket;
        StreamReader _reader;
        StreamWriter _writer;
        Stream _inputStream;
        Stream _outputStream;
        Task _listenerTask;
        CancellationTokenSource _cancellListenerSource;
        Byte[] _readBuffer;
        DateTime _lastMessageDateStamp;

        private Client(StreamSocket socket)
        {
            _socket = socket;

            _inputStream = socket.InputStream.AsStreamForRead();
            _reader = new StreamReader(_inputStream);
            _outputStream = socket.OutputStream.AsStreamForWrite();
            _writer = new StreamWriter(_outputStream);

            _lastMessageDateStamp = DateTime.Now;

            _readBuffer = new byte[1024];
        }

        public static Client Create(StreamSocket socket)
        {
            return new Client(socket);
        }

        public void StartListening()
        {
            _cancellListenerSource = new CancellationTokenSource();
            _listenerTask = new Task(() =>
            {
                // _reader.ReadAsync();

            }, _cancellListenerSource.Token);

            _listenerTask.Start();
        }

        public async void SendWelcome()
        {
            await Write("Welcome - Tank Bot".ToByteArray());
        }

        public bool IsConnected
        {
            get { return (DateTime.Now - _lastMessageDateStamp) < CONNECTION_TIMEOUT; }
        }

        public void Disconnect()
        {
            _cancellListenerSource.Cancel();
        }

        public async Task Write(byte[] buffer)
        {
            await _writer.WriteAsync(buffer.ToCharArray());
            await _writer.FlushAsync();
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
