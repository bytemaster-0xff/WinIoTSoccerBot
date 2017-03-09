using LagoVista.Core.Commanding;
using LagoVista.Core.PlatformSupport;
using SoccerBot.Core.Interfaces;
using SoccerBot.Core.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SoccerBot.Core.Channels
{
    public abstract class ChannelBase : IChannel, INotifyPropertyChanged
    {
        public event EventHandler<IChannel> Connected;
        public event EventHandler<string> Disconnected;
        public event EventHandler<byte[]> MessageReceived;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<NetworkMessage> NetworkMessageReceived;


        public ChannelBase()
        {
            DisconnectCommand = new RelayCommand(Disconnect);
            ConnectCommand = new RelayCommand(Connect);
        }

        public abstract void Connect();
        public abstract void Disconnect();


        private DateTime? _lastMessageReceived;
        public DateTime? LastMessageReceived
        {
            get { return _lastMessageReceived; }
            set
            {
                _lastMessageReceived = value;
                RaisePropertyChanged();
            }
        }

        public void RaiseNetworkMessageReceived(NetworkMessage message)
        {
            NetworkMessageReceived?.Invoke(this, message);
        }

        private States _state;
        public States State
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged();
            }
        }

        protected void RaiseMessageReceived(byte[] buffer)
        {
            MessageReceived?.Invoke(this, buffer);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            Services.DispatcherServices.Invoke(() =>
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName))
                );
        }

        protected void InvokeConnected()
        {
            Services.DispatcherServices.Invoke(() =>
                Connected?.Invoke(this, this)
            );
        }

        protected void InvokeDisconnected()
        {
            Services.DispatcherServices.Invoke(() =>
                Disconnected?.Invoke(this, "Remote device terminated connection - make sure only one instance of server is running on remote device")
                );
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

        public RelayCommand ConnectCommand { get; private set; }
        public RelayCommand DisconnectCommand { get; private set; }
        public RelayCommand SendCommand { get; private set; }

        public abstract Task WriteBuffer(byte[] buffer);

        public abstract Task<bool> ConnectAsync();
        public abstract Task DisconnectAsync();

    }
}
