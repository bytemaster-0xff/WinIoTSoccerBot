using SoccerBotApp.Devices;
using SoccerBotApp.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace SoccerBotApp.Managers
{
    public class BluetoothConnectionManager : INotifyPropertyChanged
    {
        DeviceWatcher _deviceWatcher = null;

        private ObservableCollection<SoccerBotBluetoothDevice> _resultCollection = new ObservableCollection<SoccerBotBluetoothDevice>();
        private ObservableCollection<SoccerBotBluetoothDevice> _connectedDevices = new ObservableCollection<SoccerBotBluetoothDevice>();

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

        public BluetoothConnectionManager()
        {
            StartWatcherCommand = RelayCommand.Create(StartWatcher);
            StopWatcherCommand = RelayCommand.Create(StopWatcher);
            StopWatcherCommand.Enabled = false;
        }

        private void StopWatcher()
        {
            if (null != _deviceWatcher && (DeviceWatcherStatus.Started == _deviceWatcher.Status ||
                    DeviceWatcherStatus.EnumerationCompleted == _deviceWatcher.Status))
            {
                _deviceWatcher.Stop();

                StartWatcherCommand.Enabled = true;
                StopWatcherCommand.Enabled = false;
                ConnectedDevices.Clear();
                ResultCollection.Clear();

                SelectedDevice = null;
                ActiveDevice = null;
            }
        }

        private void StartWatcher()
        {
            // Request additional properties
            string[] requestedProperties = new string[] { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

            _deviceWatcher = DeviceInformation.CreateWatcher("(System.Devices.Aep.ProtocolId:=\"{e0cbf06c-cd8b-4647-bb8a-263b43f0f974}\")",
                                                            requestedProperties,
                                                            DeviceInformationKind.AssociationEndpoint);

            // Hook up handlers for the watcher events before starting the watcher
            _deviceWatcher.Added += new TypedEventHandler<DeviceWatcher, DeviceInformation>(async (watcher, deviceInfo) =>
            {
                // Since we have the collection databound to a UI element, we need to update the collection on the UI thread.
                await App.TheApp.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    // Make sure device name isn't blank
                    if (deviceInfo.Name != "")
                    {
                        ResultCollection.Add(new SoccerBotBluetoothDevice(deviceInfo));
                        NotifyUserMessage = $"{ResultCollection.Count} devices found.";
                    }

                });
            });

            _deviceWatcher.Updated += new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(async (watcher, deviceInfoUpdate) =>
            {
                await App.TheApp.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    foreach (var rfcommInfoDisp in ResultCollection)
                    {
                        if (rfcommInfoDisp.Id == deviceInfoUpdate.Id)
                        {
                            rfcommInfoDisp.Update(deviceInfoUpdate);
                            break;
                        }
                    }
                });
            });

            _deviceWatcher.EnumerationCompleted += new TypedEventHandler<DeviceWatcher, Object>((watcher, obj) =>
            {
                NotifyUserMessage = $"{ResultCollection.Count} devices found. Enumeration completed. Watching for updates...";
            });

            _deviceWatcher.Removed += new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(async (watcher, deviceInfoUpdate) =>
            {
                // Since we have the collection databound to a UI element, we need to update the collection on the UI thread.
                await App.TheApp.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    // Find the corresponding DeviceInformation in the collection and remove it
                    foreach (var rfcommInfoDisp in ResultCollection)
                    {
                        if (rfcommInfoDisp.Id == deviceInfoUpdate.Id)
                        {
                            ResultCollection.Remove(rfcommInfoDisp);
                            break;
                        }
                    }

                    NotifyUserMessage = $"{ResultCollection.Count} devices found.";
                });
            });

            _deviceWatcher.Stopped += new TypedEventHandler<DeviceWatcher, Object>(async (watcher, obj) =>
            {
                await App.TheApp.RunOnMainThread(() =>
                {
                    var status = (watcher.Status == DeviceWatcherStatus.Aborted ? "aborted" : "stopped");
                    NotifyUserMessage = $"{ResultCollection.Count} devices found, Watcher {status}.";
                    ResultCollection.Clear();
                });
            });

            _deviceWatcher.Start();

            StartWatcherCommand.Enabled = false;
            StopWatcherCommand.Enabled = true;
        }



        Devices.SoccerBotBluetoothDevice _selectedDevice;
        public Devices.SoccerBotBluetoothDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {

                
                if(_selectedDevice != null)
                {
                    _selectedDevice.DeviceConnected -= _selectedDevice_DeviceConnected;
                }               

                _selectedDevice = value;
                if (_selectedDevice != null)
                {
                    _selectedDevice.DeviceConnected += _selectedDevice_DeviceConnected;
                }

                RaisePropertyChanged();
            }
        }

        private void _selectedDevice_DeviceConnected(object sender, SoccerBotBluetoothDevice device)
        {
            ConnectedDevices.Add(device);
            SelectedDevice = null;
        }

        Devices.SoccerBotBluetoothDevice _activeDevice;
        public Devices.SoccerBotBluetoothDevice ActiveDevice
        {
            get { return _activeDevice; }
            set
            {
                _activeDevice = value;
                RaisePropertyChanged();
            }
        }


        private String _notifyUserMessage;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public ObservableCollection<Devices.SoccerBotBluetoothDevice> ResultCollection { get { return _resultCollection; } }

        public ObservableCollection<Devices.SoccerBotBluetoothDevice> ConnectedDevices { get { return _connectedDevices; } }

        public RelayCommand StartWatcherCommand { get; private set; }
        public RelayCommand StopWatcherCommand { get; private set; }
    }
}