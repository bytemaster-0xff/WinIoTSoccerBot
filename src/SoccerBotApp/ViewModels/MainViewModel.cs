using SoccerBotApp.Channels;
using SoccerBotApp.Devices;
using SoccerBotApp.Managers;
using SoccerBotApp.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SoccerBotApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Managers.BluetoothChannelWatcher _blueToothConnectionManager = new Managers.BluetoothChannelWatcher();
        private ObservableCollection<ISoccerBotCommands> _connectedDevices = new ObservableCollection<ISoccerBotCommands>();
        private ObservableCollection<IChannel> _availableChannels = new ObservableCollection<IChannel>();
        public ObservableCollection<Models.Notification> Notifications { get { return Logger.Instance.Notifications; } }
        private ObservableCollection<IChannelWatcher> _channelWatchers = new ObservableCollection<IChannelWatcher>();

        public event PropertyChangedEventHandler PropertyChanged;
        private async void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            //TODO: Should be a design time check and not run this.
            if (App.TheApp != null && App.TheApp.Dispatcher != null)
            {
                await App.TheApp.RunOnMainThread(() =>
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                });
            }
        }

        public MainViewModel()
        {
            _blueToothConnectionManager = new BluetoothChannelWatcher();
            RegisterChannelWatcher(_blueToothConnectionManager);
        }

        private void RegisterChannelWatcher(IChannelWatcher channelWatcher)
        {
            channelWatcher.DeviceFoundEvent += ChannelWatcher_DeviceFoundEvent;
            channelWatcher.DeviceRemovedEvent += ChannelWatcher_DeviceRemovedEvent;
            _channelWatchers.Add(channelWatcher);
        }

        private void ChannelWatcher_DeviceRemovedEvent(object sender, IChannel e)
        {
            lock (this)
            {
                if (AvailableChannels.Contains(e))
                {
                    AvailableChannels.Remove(e);
                }
            }
        }

        private void ChannelWatcher_DeviceFoundEvent(object sender, IChannel e)
        {
            lock(this)
            {
                if (!AvailableChannels.Contains(e))
                {
                    e.Connected += channel_Connected;
                    AvailableChannels.Add(e);
                }
            }            
        }
              

        private void channel_Connected(object sender, IChannel device)
        {
            var soccerBot = new Devices.mBlockSoccerBot(device);
            _connectedDevices.Add(soccerBot);
        }


        ISoccerBotCommands _activeDevice;
        public ISoccerBotCommands ActiveDevice
        {
            get { return _activeDevice; }
            set
            {
                _activeDevice = value;
                RaisePropertyChanged();
            }
        }

        public Managers.BluetoothChannelWatcher BlueTooth { get { return _blueToothConnectionManager; } }

        public ObservableCollection<ISoccerBotCommands> ConnectedDevices { get { return _connectedDevices; } }

        public ObservableCollection<IChannel> AvailableChannels { get { return _availableChannels; } }

    }
}
