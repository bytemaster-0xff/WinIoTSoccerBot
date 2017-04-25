using LagoVista.Core.Models.Drawing;
using LagoVista.Core.PlatformSupport;
using SoccerBot.Core.Interfaces;
using SoccerBot.Core.Models;
using SoccerBot.mBot.Filters;
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

        MedianFilter _medianFilter;

        public async Task InitAsync()
        {
            var i2cDeviceSelector = I2cDevice.GetDeviceSelector();
            var devices = await DeviceInformation.FindAllAsync(i2cDeviceSelector);
            var hmc5983ConnectionSettings = new I2cConnectionSettings(HMC5983_ADDRESS);
            _compassSensor = await I2cDevice.FromIdAsync(devices[0].Id, hmc5983ConnectionSettings);

            //Config Register A
            // 7 - 7 = 0 Temperature Off
            // 6 - 5 = 11 => 8 Samples per reading
            // 4 - 2 = 100 => Data Rate 15 Hz
            // 1 - 0 = 00 Measurement Config Bits (default)

            //Config Register B
            //Gain - Use Default

            /*
            var setupBuffer = new byte[2];
            setupBuffer[0] = HMC5983_CONF_A;
            setupBuffer[1] = 0b01110000;
            _compassSensor.Write(setupBuffer);*/

            _medianFilter = new MedianFilter();

            RawX = new Sensor() { IsOnline = false };
            RawY = new Sensor() { IsOnline = false };
            IsOnline = false;
        }

        public void Start()
        {
            if (_timer == null)
            {
                _timer = new Timer(Read, null, 0, 250);
            }
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
                /* Ask the Compass to Perform a Reading */
                var outBuffer = new byte[2];
                outBuffer[0] = HMC5983_MODE;
                outBuffer[1] = 0x01;
                _compassSensor.Write(outBuffer);

                /* Wait for Conversion complete, 10ms should be plenty */
                await Task.Delay(20);

                /* Move the read buffer to register #3 which is where the compass data starts */
                /* Read Compass Values */

                var inBuffer = new byte[6];
                _compassSensor.WriteRead(new byte[1] { HMC5983_OUT_X_MSB }, inBuffer);

    //            var hX = BitConverter.ToInt16(inBuffer, 0);
  //              var hZ = BitConverter.ToInt16(inBuffer, 2);
//                var hY = BitConverter.ToInt16(inBuffer, 4);


                var hX = (Int16)(inBuffer[0] << 8 | inBuffer[1]);
                var hZ = (Int16)(inBuffer[2] << 8 | inBuffer[3]);
                var hY = (Int16)(inBuffer[4] << 8 | inBuffer[5]);

                _medianFilter.Add(new Point2D<int>(hX, hY));

                
                var radians = Math.Atan2(_medianFilter.Filtered.X, _medianFilter.Filtered.Y);
                var angle = radians * (180 / Math.PI);

                Value = angle.ToString();


                if (hY > 0) Value = (90 - angle).ToString();
                else if (hY < 0) Value = (270 - angle).ToString(); 

                IsOnline = true;

                RawX.Value = hX.ToString();
                RawX.IsOnline = true;
                RawY.Value = hY.ToString();
                RawY.IsOnline = true;

                Debug.WriteLine($"Angle={Value}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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
