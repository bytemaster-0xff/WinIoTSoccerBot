using LagoVista.Core.Commanding;
using LagoVista.Core.PlatformSupport;
using SoccerBot.Core.Channels;
using SoccerBot.Core.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SoccerBot.Core.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        ISoccerBotLogger _logger;

        public event EventHandler<IChannel> ChannelConnected;

        private ObservableCollection<ISoccerBotCommands> _connectedDevices;
        private ObservableCollection<IChannelWatcher> _channelWatchers;
        private ObservableCollection<IChannel> _availableChannels;
        public ObservableCollection<Models.Notification> Notifications { get { return _logger.Notifications; } }

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
            _availableChannels = new ObservableCollection<IChannel>();
            _channelWatchers = new ObservableCollection<IChannelWatcher>();
            _connectedDevices = new ObservableCollection<ISoccerBotCommands>();
            StartWatchersCommand = new RelayCommand(StartWatchers);
            StopWatchersCommand = new RelayCommand(StopWatchers);
        }

        public ISoccerBotLogger Logger { get { return _logger; } set {  _logger = value; } }

        public void RegisterChannelWatcher(IChannelWatcher channelWatcher)
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
            ChannelConnected?.Invoke(this, device);
        }

        ISoccerBot _activeDevice;
        public ISoccerBot ActiveDevice
        {
            get { return _activeDevice; }
            set
            {
                _activeDevice = value;
                RaisePropertyChanged();
            }
        }

        ISoccerBot _activeRemoteDevice;
        public ISoccerBot ActiveRemoteDevice
        {
            get { return _activeRemoteDevice; }
            set
            {
                _activeRemoteDevice = value;
                RaisePropertyChanged();
            }
        }


        public void StartWatchers()
        {
            foreach(var watcher in _channelWatchers)
            {
                watcher.StartWatcherCommand.Execute(null);
            }

            StartWatchersCommand.Enabled = false;
            StopWatchersCommand.Enabled = true;
        }

        public void StopWatchers()
        {
            foreach(var watcher in _channelWatchers)
            {
                watcher.StopWatcherCommand.Execute(null);
            }

            StartWatchersCommand.Enabled = true;
            StopWatchersCommand.Enabled = false;
        }


        public RelayCommand StartWatchersCommand { get; private set; }
        public RelayCommand StopWatchersCommand { get; private set; }

        public ObservableCollection<ISoccerBotCommands> ConnectedDevices { get { return _connectedDevices; } }

        public ObservableCollection<IChannel> AvailableChannels { get { return _availableChannels; } }

    }
}
