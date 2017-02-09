using LagoVista.Core.Commanding;
using LagoVista.Core.PlatformSupport;
using SoccerBot.Core.Interfaces;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SoccerBot.Core.Devices
{
    public abstract class SoccerBotBase : ISoccerBot
    {
        public enum Commands
        {
            Forward,
            Backwards,
            Left,
            Right,
            Stop
        }

        ITimer _sensorRefreshTimer;

        public String Id { get; set; }
        public String Name { get; set; }
        public String DeviceName { get; set; }
        public String DeviceTypeId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            Services.DispatcherServices.Invoke(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
        }

        public SoccerBotBase()
        {
            ForwardCommand = RelayCommand<Commands>.Create(SendCommand, Commands.Forward);
            StopCommand = RelayCommand<Commands>.Create(SendCommand, Commands.Stop);
            BackwardsCommand = RelayCommand<Commands>.Create(SendCommand, Commands.Backwards);
            LeftCommand = RelayCommand<Commands>.Create(SendCommand, Commands.Left);
            RightCommand = RelayCommand<Commands>.Create(SendCommand, Commands.Right);
        }

        private String _firmwareVersion;
        public String FirmwareVersion
        {
            get { return _firmwareVersion; }
            set
            {
                _firmwareVersion = value;
                RaisePropertyChanged();
                StartSensorRefreshTimer();
            }
        }

        protected abstract void SendCommand(Commands cmd);

        public RelayCommand RefreshSensorsCommand { get; private set; }

        public RelayCommand<Commands> ForwardCommand { get; private set; }
        public RelayCommand<Commands> StopCommand { get; private set; }
        public RelayCommand<Commands> BackwardsCommand { get; private set; }

        public RelayCommand<Commands> LeftCommand { get; private set; }
        public RelayCommand<Commands> RightCommand { get; private set; }

        public void StartSensorRefreshTimer()
        {
            _sensorRefreshTimer.Interval = TimeSpan.FromMilliseconds(500);
            _sensorRefreshTimer.Tick += _sensorRefreshTimer_Tick;
            StartRefreshTimer();
        }

        protected abstract void RefreshSensors();

        protected abstract void SpeedUpdated(short speed);


        private void _sensorRefreshTimer_Tick(object sender, object e)
        {
            RefreshSensors();
        }

        private short _speed = 100;
        public short Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                RaisePropertyChanged();
            }
        }

        private int _frontIRSensor = 0;
        public int FrontIRSensor
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
