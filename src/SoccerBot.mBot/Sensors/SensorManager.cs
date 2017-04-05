using SoccerBot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using System.ComponentModel;

namespace SoccerBot.mBot.Sensors
{
    public class SensorManager 
    {
        Compass5983 _compass;
        IRSensorArray _irSensorArray;

        public event PropertyChangedEventHandler PropertyChanged;

        public SensorManager()
        {

            _compass = new Compass5983();
        }

        public async Task InitAsync()
        {
            
            var gpio = GpioController.GetDefault();

            _irSensorArray = new IRSensorArray(gpio);
            await _compass.InitAsync();            
        }

        public void Start()
        {
            _irSensorArray.Start();
            _compass.Start();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Compass5983 Compass { get { return _compass; } }
        public IRSensorArray SensorArray { get { return _irSensorArray; } }
    }
}
