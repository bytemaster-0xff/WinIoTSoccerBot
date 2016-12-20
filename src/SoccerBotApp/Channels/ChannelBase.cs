using SoccerBotApp.Utilities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace SoccerBotApp.Channels
{
    public abstract class ChannelBase : IChannel, INotifyPropertyChanged
    {
        public event EventHandler<IChannel> Connected;
        public event EventHandler<string> Disconnected;
        public event EventHandler<byte[]> MessageReceived;
        public event PropertyChangedEventHandler PropertyChanged;


        public ChannelBase()
        {
            DisconnectCommand = RelayCommand.Create(Disconnect);
            ConnectCommand = RelayCommand.Create(Connect);
        }

        public abstract void Connect();
        public abstract void Disconnect();

        

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

        protected async void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            //TODO: Should be a design time check and not run this.
            if (App.TheApp != null && App.TheApp.Dispatcher != null)
            {
                if (App.TheApp.Dispatcher.HasThreadAccess)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
                else
                {
                    await App.TheApp.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                    });
                }
            }
        }

        protected async void InvokeConnected()
        {
            await App.TheApp.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                Connected?.Invoke(this, this);
            });

        }

        protected async void InvokeDisconnected()
        {
            await App.TheApp.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                Disconnected?.Invoke(this, "Remote device terminated connection - make sure only one instance of server is running on remote device");
            });
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
