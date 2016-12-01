﻿using SoccerBotApp.Utilities;
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

namespace SoccerBotApp.Devices
{
    public class SoccerBotBluetoothDevice : INotifyPropertyChanged
    {
        public event EventHandler<string> Disconnected;
        public event EventHandler<byte[]> MessageReceived;
        public event PropertyChangedEventHandler PropertyChanged;

        StreamSocket _socket = null;
        DataWriter _writer = null;

        DeviceInformation _deviceInfo;

        RfcommDeviceService _deviceService = null;

        public ObservableCollection<String> IncomingMessages { get; private set; }

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
            NotifyUserMessage = "Idle";
            IncomingMessages = new ObservableCollection<string>();
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

                DataReader chatReader = new DataReader(_socket.InputStream);
                ReceiveStringLoop(chatReader);
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

        private async void ReceiveStringLoop(DataReader chatReader)
        {
            try
            {
                var size = await chatReader.LoadAsync(sizeof(uint));

                if (size < sizeof(uint))
                {
                    Disconnected?.Invoke(this, "Remote device terminated connection - make sure only one instance of server is running on remote device");                    
                    return;
                }

                uint bufferLength = chatReader.ReadUInt32();
                uint actualBufferLength = await chatReader.LoadAsync(bufferLength);
                if (actualBufferLength != bufferLength)
                {
                    Disconnected?.Invoke(this, "Remote device terminated connection - make sure only one instance of server is running on remote device");
                    
                    return;
                }

                var buffer = new byte[bufferLength];
                chatReader.ReadBytes(buffer);

                MessageReceived(this, buffer);
                ReceiveStringLoop(chatReader);

                await App.TheApp.RunOnMainThread(() =>
                {
                    IncomingMessages.Add(System.Text.Encoding.ASCII.GetString(buffer));
                });
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

        public Task SendCommandsAsync(string commands)
        {
            throw new NotImplementedException();
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

        public RelayCommand DisconnectCommand { get; private set; }


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