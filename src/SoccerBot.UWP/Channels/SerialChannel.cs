using SoccerBot.Core.Channels;
using SoccerBot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace SoccerBot.UWP.Channels
{
    public class SerialChannel : ChannelBase
    {

        ISoccerBotLogger _logger;
        string _serialPortId;
        SerialDevice _serialPort = null;
        DataWriter _dataWriteObject = null;
        DataReader _dataReaderObject = null;
        const int MAX_BUFFER_SIZE = 255;

        CancellationTokenSource _readCancellationTokenSource;

        public SerialChannel(string serialPortId, ISoccerBotLogger logger)
        {
            _serialPortId = serialPortId;
            _logger = logger;
        }

        public override void Connect()
        {



        }

        public override void Disconnect()
        {
            
        }

        public async override Task WriteBuffer(byte[] buffer)
        {
            _dataWriteObject.WriteBytes(buffer);
            await _dataWriteObject.StoreAsync();
        }

        public async override Task<bool> ConnectAsync()
        {
            _serialPort = await SerialDevice.FromIdAsync(_serialPortId);
            _serialPort.WriteTimeout = TimeSpan.FromMilliseconds(100);
            _serialPort.ReadTimeout = TimeSpan.FromMilliseconds(100);
            _serialPort.BaudRate = 115200;
            _readCancellationTokenSource = new System.Threading.CancellationTokenSource();

            _dataReaderObject = new DataReader(_serialPort.InputStream);
            _dataWriteObject = new DataWriter(_serialPort.OutputStream);

            Listen();

            return true;
        }

        public async void Listen()
        {
            try
            {
                // keep reading the serial input
                while (true)
                {
                    await ReadAsync(_readCancellationTokenSource.Token);
                }

            }
            catch (TaskCanceledException)
            {
                _logger.NotifyUserInfo("SerialChannel_Listen", "Task Cancelled - Closing");
            }
            catch (Exception ex)
            {
                _logger.NotifyUserError("SerialChannel_Listen", ex.Message);
            }
            finally
            {
                // Cleanup once complete
                if (_dataReaderObject != null)
                {
                    _dataReaderObject.DetachStream();
                    _dataReaderObject = null;
                }
            }
        }

        private async Task ReadAsync(CancellationToken cancellationToken)
        {
            Task<UInt32> loadAsyncTask;

            uint ReadBufferLength = 1024;

            // If task cancellation was requested, comply
            cancellationToken.ThrowIfCancellationRequested();

            // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
            _dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;

            // Create a task object to wait for data on the serialPort.InputStream
            loadAsyncTask = _dataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);

            // Launch the task and wait
            UInt32 bytesRead = await loadAsyncTask;
            if (bytesRead > 0)
            {
                var buffer = new byte[bytesRead];
                _dataReaderObject.ReadBytes(buffer);
                RaiseMessageReceived(buffer);
            }
        }

        public override Task DisconnectAsync()
        {
            if(_dataReaderObject != null)
            {
                _dataReaderObject.DetachStream();
                _dataReaderObject = null;
            }

            if (_dataWriteObject != null)
            {
                _dataWriteObject.DetachStream();
                _dataWriteObject = null;
            }

            return Task.FromResult(default(object));
        }
    }
}
