using System;
using System.ComponentModel;
using SoccerBot.Core.Interfaces;
using Windows.Devices.Gpio;

namespace SoccerBot.mBot.Sensors
{
    public class IRSensor : ISensor
    {
        GpioPin _input;

        public IRSensor(GpioController gpio, int pin)
        {
            _input = gpio.OpenPin(pin);
            _input.SetDriveMode(GpioPinDriveMode.Input);
            Value = "?";
        }

        public string Value { get; set; }

        public DateTime? LastUpdated { get; private set; }

        public bool IsOnline { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Read()
        {
            if (_input != null)
            {

                if (_input.Read() == GpioPinValue.High)
                {
                    Value = "Off";
                }
                else
                {
                    Value = "On";
                }

                LastUpdated = DateTime.Now;
                IsOnline = true;
            }
            else
            {
                Value = "Offline";
                IsOnline = false;
            }
        }

        public void Dispose()
        {
            _input.Dispose();
            _input = null;
            IsOnline = false;
            
        }

    }
}
