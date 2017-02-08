using System;
using System.Linq;
using System.Collections.ObjectModel;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.UI.Core;
using SoccerBot.Core.Channels;
using SoccerBot.Core.Interfaces;
using LagoVista.Core.PlatformSupport;

namespace SoccerBot.UWP.Channels
{
    public class BluetoothChannelWatcher : ChannelWatcherBase
    {
        ISoccerBotLogger _logger;
        DeviceWatcher _deviceWatcher = null;

        private ObservableCollection<IChannel> _channels = new ObservableCollection<IChannel>();

        public BluetoothChannelWatcher(ISoccerBotLogger logger)
        {
            _logger = logger;
        }

        protected override void StopWatcher()
        {
            if (null != _deviceWatcher && (DeviceWatcherStatus.Started == _deviceWatcher.Status ||
                    DeviceWatcherStatus.EnumerationCompleted == _deviceWatcher.Status))
            {
                _deviceWatcher.Stop();

                StartWatcherCommand.Enabled = true;
                StopWatcherCommand.Enabled = false;
                RaiseClearDevicesEvent();
            }
        }

        protected override void StartWatcher()
        {
            // Request additional properties
            string[] requestedProperties = new string[] { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

            _deviceWatcher = DeviceInformation.CreateWatcher("(System.Devices.Aep.ProtocolId:=\"{e0cbf06c-cd8b-4647-bb8a-263b43f0f974}\")",
                                                            requestedProperties,
                                                            DeviceInformationKind.AssociationEndpoint);

            _logger.NotifyUserInfo("BT Mgr", $"Started BT Watcher");

            // Hook up handlers for the watcher events before starting the watcher
            _deviceWatcher.Added += new TypedEventHandler<DeviceWatcher, DeviceInformation>( (watcher, deviceInfo) =>
            {
                // Since we have the collection databound to a UI element, we need to update the collection on the UI thread.
                Services.DispatcherServices.Invoke(() => {
                    // Make sure device name isn't blank
                    if (deviceInfo.Name != "")
                    {
                        RaiseDeviceFoundEvent(new BluetoothChannel(deviceInfo, _logger));
                        _logger.NotifyUserInfo("BT Mgr", $"Found Device => " + deviceInfo.Name);
                    }

                });
            });

            _deviceWatcher.Updated += new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>( (watcher, deviceInfoUpdate) =>
            {
                Services.DispatcherServices.Invoke(() => {
                    _logger.NotifyUserInfo("BT Mgr", $"Device Updated => {deviceInfoUpdate.Id}.");

                    var updatedChannel = _channels.Where(itm => itm.Id == deviceInfoUpdate.Id).FirstOrDefault();
                    if(updatedChannel != null)
                    {
                        updatedChannel.Update(deviceInfoUpdate);
                    }
                });
            });

            _deviceWatcher.EnumerationCompleted += new TypedEventHandler<DeviceWatcher, Object>((watcher, obj) =>
            {
                _logger.NotifyUserInfo("BT Mgr", $"{_channels.Count} devices found. Enumeration completed. Watching for updates...");
            });

            _deviceWatcher.Removed += new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>( (watcher, deviceInfoUpdate) =>
            {
                // Since we have the collection databound to a UI element, we need to update the collection on the UI thread.
                Services.DispatcherServices.Invoke(() => {
                    _logger.NotifyUserInfo("BT Mgr", $"Device Removed => {deviceInfoUpdate.Id}.");
                    var removedDevice = _channels.Where(itm => itm.Id == deviceInfoUpdate.Id).FirstOrDefault();
                    if(removedDevice != null)
                    {
                        _channels.Remove(removedDevice);
                    }                    
                });
            });

            _deviceWatcher.Stopped += new TypedEventHandler<DeviceWatcher, Object>( (watcher, obj) =>
            {
                Services.DispatcherServices.Invoke(() => {
                    var status = (watcher.Status == DeviceWatcherStatus.Aborted ? "aborted" : "stopped");
                    _logger.NotifyUserInfo("BT Mgr", $"BT Manager State Change: {status}.");
                    _channels.Clear();
                });
            });

            _deviceWatcher.Start();

            StartWatcherCommand.Enabled = false;
            StopWatcherCommand.Enabled = true;
        }        
    }
}