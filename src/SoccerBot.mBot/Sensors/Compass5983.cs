using LagoVista.Core.PlatformSupport;
using SoccerBot.Core.Interfaces;
using SoccerBot.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace SoccerBot.mBot.Sensors
{
    public class Compass5983 : ISensor
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            //TODO: Should be a design time check and not run this.
            Services.DispatcherServices.Invoke(() =>
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName))
            );
        }


        private I2cDevice _compassSensor;
        Timer _timer;

        // I2C ADDRESS
        const byte HMC5983_ADDRESS = 0x1E;

        // I2C COMMANDS
        const byte HMC5983_WRITE = 0x3C;
        const byte HMC5983_READ = 0x3D;

        const byte HMC5983_CONF_A = 0x00;
        const byte HMC5983_CONF_B = 0x01;
        const byte HMC5983_MODE = 0x02;
        const byte HMC5983_OUT_X_MSB = 0x03;
        const byte HMC5983_OUT_X_LSB = 0x04;
        const byte HMC5983_OUT_Z_MSB = 0x05;
        const byte HMC5983_OUT_Z_LSB = 0x06;
        const byte HMC5983_OUT_Y_MSB = 0x07;
        const byte HMC5983_OUT_Y_LSB = 0x08;
        const byte HMC5983_STATUS = 0x09;
        const byte HMC5983_ID_A = 0x0A;
        const byte HMC5983_ID_B = 0x0B;
        const byte HMC5983_ID_C = 0x0C;
        const byte HMC5983_TEMP_OUT_MSB = 0x31;
        const byte HMC5983_TEMP_OUT_LSB = 0x32;

        public async Task InitAsync()
        {
            var i2cDeviceSelector = I2cDevice.GetDeviceSelector();
            var devices = await DeviceInformation.FindAllAsync(i2cDeviceSelector);
            var hmc5983ConnectionSettings = new I2cConnectionSettings(HMC5983_ADDRESS);
            _compassSensor = await I2cDevice.FromIdAsync(devices[0].Id, hmc5983ConnectionSettings);

            RawX = new Sensor() { IsOnline = false };
            RawY = new Sensor() { IsOnline = false };
            IsOnline = false;
        }

        public void Start()
        {
            _timer = new Timer(Read, null, 0, 500);
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _timer.Dispose();
                _timer = null;
            }
        }

        public async void Read(Object state)
        {
            try
            {
                var outBuffer = new byte[2];
                outBuffer[0] = HMC5983_MODE;
                outBuffer[1] = 0x01;
                _compassSensor.Write(outBuffer);

                await Task.Delay(10);

                var inBuffer = new byte[6];
                _compassSensor.Read(inBuffer);

                var hX = BitConverter.ToUInt16(inBuffer, 0);
                var hY = BitConverter.ToUInt16(inBuffer, 2);
                var hZ = BitConverter.ToUInt16(inBuffer, 4);

                if (hY == 0 && hX > 0) Value = 180.ToString();
                else if (hY == 0 && hX <= 0) Value = 0.ToString();
                else if (hY > 0) Value = (90 - Convert.ToUInt32((Math.Atan(hX / hY) * (180 / Math.PI)))).ToString();
                else if (hY < 0) Value = (270 - Convert.ToUInt32((Math.Atan(hX / hY) * (180 / Math.PI)))).ToString();

                IsOnline = true;

                RawX.Value = hX.ToString();
                RawX.IsOnline = true;
                RawY.Value = hX.ToString();
                RawY.IsOnline = true;

                Debug.WriteLine("Compass: " + Value.ToString());
            }
            catch (Exception)
            {
                RawX.IsOnline = false;
                RawY.IsOnline = false;
                Debug.WriteLine("Compass Offline: " + Value.ToString());
                IsOnline = false;
            }
        }

        private bool _isOnline = false;
        public bool IsOnline
        {
            get { return _isOnline; }
            set
            {
                _isOnline = value;
                RaisePropertyChanged();
            }
        }


        public DateTime? LastUpdated
        {
            get; private set;
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                LastUpdated = DateTime.Now;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(LastUpdated));
            }
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _timer.Dispose();
            }

            if (_compassSensor != null)
            {
                _compassSensor.Dispose();
                _compassSensor = null;
            }
        }

        public Sensor RawX { get; set; }
        public Sensor RawY { get; set; }
    }
}
