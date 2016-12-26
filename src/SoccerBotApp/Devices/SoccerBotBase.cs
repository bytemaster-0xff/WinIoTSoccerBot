using SoccerBotApp.Utilities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace SoccerBotApp.Devices
{
    public abstract class SoccerBotBase
    {     
        public enum Commands
        {
            Forward,
            Backwards,
            Left,
            Right,
            Stop
        }

        DispatcherTimer _sensorRefreshTimer = new DispatcherTimer();

        public String Id { get; set; }
        public String Name { get; set; }
        public String DeviceName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected async void RaisePropertyChanged([CallerMemberName] string propertyName = null)
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

        public SoccerBotBase()
        {
            ForwardCommand = RelayCommand.Create(SendCommand,Commands.Forward);
            StopCommand = RelayCommand.Create(SendCommand, Commands.Stop);
            BackwardsCommand = RelayCommand.Create(SendCommand, Commands.Backwards);
            LeftCommand = RelayCommand.Create(SendCommand, Commands.Left);
            RightCommand = RelayCommand.Create(SendCommand, Commands.Right);
        }
        
        protected abstract void SendCommand(Commands cmd);

        public RelayCommand RefreshSensorsCommand { get; private set; }

        public RelayCommand ForwardCommand { get; private set; }
        public RelayCommand StopCommand { get; private set; }
        public RelayCommand BackwardsCommand { get; private set; }

        public RelayCommand LeftCommand { get; private set; }
        public RelayCommand RightCommand { get; private set; }

        public void StartSensorRefreshTimer()
        {
            _sensorRefreshTimer.Interval = TimeSpan.FromMilliseconds(250);
            _sensorRefreshTimer.Tick += _sensorRefreshTimer_Tick;
            StartRefreshTimer();
        }

        protected abstract void RefreshSensors();

        protected abstract void SpeedUpdated(short speed);


        private void _sensorRefreshTimer_Tick(object sender, object e)
        {
            RefreshSensors();
        }

        private short _speed = 300;
        public short Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                RaisePropertyChanged();
            }
        }

        private String _frontIRSensor = "?";
        public String FrontIRSensor
        {
            get { return _frontIRSensor; }
            set
            {
                _frontIRSensor = value;
                RaisePropertyChanged();
            }
        }

        public void PauseRefreshTimer()
        {
            _sensorRefreshTimer.Stop();
        }

        public void StartRefreshTimer()
        {
            _sensorRefreshTimer.Start();
        }
        
    }
}
