using LagoVista.Core.Commanding;
using LagoVista.Core.PlatformSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Devices
{
    public  abstract class SoccerBotBase
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

        public String Version { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected  void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            Services.DispatcherServices.Invoke(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
        }

        public SoccerBotBase()
        {
            ForwardCommand = RelayCommand.Create(SendCommand, Commands.Forward);
            StopCommand = RelayCommand.Create(SendCommand, Commands.Stop);
            BackwardsCommand = RelayCommand.Create(SendCommand, Commands.Backwards);
            LeftCommand = RelayCommand.Create(SendCommand, Commands.Left);
            RightCommand = RelayCommand.Create(SendCommand, Commands.Right);
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

        public RelayCommand ForwardCommand { get; private set; }
        public RelayCommand StopCommand { get; private set; }
        public RelayCommand BackwardsCommand { get; private set; }

        public RelayCommand LeftCommand { get; private set; }
        public RelayCommand RightCommand { get; private set; }

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
