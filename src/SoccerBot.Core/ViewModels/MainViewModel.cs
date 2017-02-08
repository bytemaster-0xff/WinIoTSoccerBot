using LagoVista.Core.PlatformSupport;
using SoccerBot.Core.Channels;
using SoccerBot.Core.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SoccerBot.Core.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        ISoccerBotLogger _logger;

        private Managers.BluetoothChannelWatcher _blueToothConnectionManager = new Managers.BluetoothChannelWatcher();
        private ObservableCollection<ISoccerBotCommands> _connectedDevices = new ObservableCollection<ISoccerBotCommands>();
        private ObservableCollection<IChannel> _availableChannels = new ObservableCollection<IChannel>();
        public ObservableCollection<Models.Notification> Notifications { get { return _logger.Notifications; } }
        private ObservableCollection<ChannelWatcherBase> _channelWatchers = new ObservableCollection<ChannelWatcherBase>();

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            //TODO: Should be a design time check and not run this.
            Services.DispatcherServices.Invoke(() =>
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName))
            );
        }

        public MainViewModel(ISoccerBotLogger logger)
        {
            _logger = logger;
            _blueToothConnectionManager = new BluetoothChannelWatcher();
            RegisterChannelWatcher(_blueToothConnectionManager);
        }

        private void RegisterChannelWatcher(ChannelWatcherBase channelWatcher)
        {
            channelWatcher.DeviceFoundEvent += ChannelWatcher_DeviceFoundEvent;
            channelWatcher.DeviceRemovedEvent += ChannelWatcher_DeviceRemovedEvent;
            channelWatcher.ClearDevices += ChannelWatcher_ClearDevices;
            _channelWatchers.Add(channelWatcher);
        }

        private void ChannelWatcher_ClearDevices(object sender, System.EventArgs e)
        {
            AvailableChannels.Clear();
            ConnectedDevices.Clear();
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
            var soccerBot = new Devices.mBlockSoccerBot(device,_logger);
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
