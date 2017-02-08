using LagoVista.Core.Commanding;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SoccerBot.Core.Interfaces
{
    public interface IChannelWatcher : INotifyPropertyChanged
    {
        event EventHandler<IChannel> DeviceFoundEvent;
        event EventHandler<IChannel> DeviceRemovedEvent;
        event EventHandler<IChannel> DeviceConnectedEvent;

        event EventHandler ClearDevices;

        ObservableCollection<IChannel> Channels { get; }

        RelayCommand StartWatcherCommand { get; }
        RelayCommand StopWatcherCommand { get; }
    }
}
