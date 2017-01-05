using SoccerBotApp.Channels;
using SoccerBotApp.Utilities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SoccerBotApp.Managers
{
    public abstract class ChannelWatcherBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<IChannel> DeviceFoundEvent;
        public event EventHandler<IChannel> DeviceRemovedEvent;
        public event EventHandler<IChannel> DeviceConnectedEvent;

        public event EventHandler ClearDevices;

        public ChannelWatcherBase()
        {
            StartWatcherCommand = RelayCommand.Create(StartWatcher);
            StopWatcherCommand = RelayCommand.Create(StopWatcher);
            StopWatcherCommand.Enabled = false;
        }

        public async void RaiseDeviceFoundEvent(IChannel channel)
        {
            if (App.TheApp != null && App.TheApp.Dispatcher != null)
            {
                await App.TheApp.RunOnMainThread(() =>
                {
                    DeviceFoundEvent?.Invoke(this, channel);
                });
            }
        }

        public async void RaiseDeviceRemovedEvent(IChannel channel)
        {
            if (App.TheApp != null && App.TheApp.Dispatcher != null)
            {
                await App.TheApp.RunOnMainThread(() =>
                {
                    DeviceRemovedEvent?.Invoke(this, channel);
                });
            }
        }

        public async void RaiseDeviceConnectedEvent(IChannel channel)
        {
            if (App.TheApp != null && App.TheApp.Dispatcher != null)
            {
                await App.TheApp.RunOnMainThread(() =>
                {
                    DeviceConnectedEvent?.Invoke(this, channel);
                });
            }
        }

        public async void RaiseClearDevicesEvent()
        {
            if (App.TheApp != null && App.TheApp.Dispatcher != null)
            {
                await App.TheApp.RunOnMainThread(() =>
                {
                    ClearDevices?.Invoke(this, null);
                });
            }
        }

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

        protected abstract void StartWatcher();
        protected abstract void StopWatcher();

        public RelayCommand StartWatcherCommand { get; private set; }
        public RelayCommand StopWatcherCommand { get; private set; }
    }
}
