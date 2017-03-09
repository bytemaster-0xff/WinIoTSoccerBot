using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace SoccerBot.mBot.Sensors
{
    public class Compass5983 : IDisposable
    {
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

        public async Task Init()
        {
            var i2cDeviceSelector = I2cDevice.GetDeviceSelector();
            var devices = await DeviceInformation.FindAllAsync(i2cDeviceSelector);
            var HTU21D_settings = new I2cConnectionSettings(HMC5983_ADDRESS);
            _compassSensor = await I2cDevice.FromIdAsync(devices[0].Id, HTU21D_settings);

            _timer = new Timer(Read, null, 0, 500);
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


                var hX = Convert.ToInt16((inBuffer[0] << 8) + inBuffer[1]);
                var hZ = Convert.ToInt16((inBuffer[2] << 8) + inBuffer[3]);
                var hY = Convert.ToInt16((inBuffer[4] << 8) + inBuffer[5]);

                // convert the numbers to fit the 
                //if (hX > 0x07FF) hX = 0xFFFF - hX;
                //if (hZ > 0x07FF) hZ = Convert.ToInt16(0xFFFF - hZ);
                //if (hY > 0x07FF) hY = 0xFFFF - hY;
                

                if (hY == 0 && hX > 0) Heading = 180;
                else if (hY == 0 && hX <= 0) Heading = 0;
                else if (hY > 0) Heading = 90 - Convert.ToInt16((Math.Atan(hX / hY) * (180 / Math.PI)));
                else if (hY < 0) Heading = 270 - Convert.ToInt16((Math.Atan(hX / hY) * (180 / Math.PI)));

                CompassOnline = true;
                LastReading = DateTime.Now;
            }
            catch (Exception)
            {
                CompassOnline = false;
            }
        }

        private DateTime? _lastReading;
        public DateTime? LastReading
        {
            get { return _lastReading; }
            set { _lastReading = value; }
        }

        private bool _compassOnline;
        public bool CompassOnline 
        {
            get { return _compassOnline; }
            set { _compassOnline = value; }
        }

        private int _heading;
        public int Heading
        {
            get { return _heading; }
            set { _heading = value; }
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
    }
}
