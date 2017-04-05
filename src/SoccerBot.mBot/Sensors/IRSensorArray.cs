using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace SoccerBot.mBot.Sensors
{
    public class IRSensorArray
    {
        public const int BANK_ONE_OUTPUT = 12;
        public const int BANK_TWO_OUTPUT = 26;

        public const int FRONT_LEFT = 17;
        public const int FRONT = 18;
        public const int FRONT_RIGHT = 23;

        public const int LEFT = 27;
        public const int RIGHT = 24;

        public const int REAR_LEFT = 20;
        public const int REAR = 21;
        public const int REAR_RIGHT = 16;

        GpioPin _bank1;
        GpioPin _bank2;

        IRSensor _front;
        IRSensor _frontRight;
        IRSensor _frontLeft;

        IRSensor _leftSide;
        IRSensor _rightSide;

        IRSensor _rear;
        IRSensor _rearLeft;
        IRSensor _rearRight;

        public IRSensorArray(GpioController gpioController)
        {
            _frontLeft = new IRSensor(gpioController, FRONT_LEFT);
            _front = new IRSensor(gpioController, FRONT);
            _frontRight = new IRSensor(gpioController, FRONT_RIGHT);

            _leftSide = new IRSensor(gpioController, LEFT);
            _rightSide = new IRSensor(gpioController, RIGHT);


            _rearLeft = new IRSensor(gpioController, REAR_LEFT);
            _rear = new IRSensor(gpioController, REAR);
            _rearRight = new IRSensor(gpioController, REAR_RIGHT);

            _bank1 = gpioController.OpenPin(BANK_ONE_OUTPUT);
            _bank1.SetDriveMode(GpioPinDriveMode.Output);
            _bank2 = gpioController.OpenPin(BANK_TWO_OUTPUT);
            _bank2.SetDriveMode(GpioPinDriveMode.Output);
        }

        public void Start()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    _bank1.Write(GpioPinValue.High);
                    _bank2.Write(GpioPinValue.High);
                    await Task.Delay(50);

                    _front.Read();
                    _frontLeft.Read();
                    _frontRight.Read();

                    _leftSide.Read();
                    _rightSide.Read();

                    _rear.Read();
                    _rearLeft.Read();
                    _rearRight.Read();


                    _bank1.Write(GpioPinValue.Low);
                    _bank2.Write(GpioPinValue.Low);
                    await Task.Delay(500);
                }
            });
        }

        public IRSensor FrontLeft { get { return _frontLeft; } }
        public IRSensor Front { get { return _front; } }
        public IRSensor FrontRight { get { return _frontRight; } }

        public IRSensor Right { get { return _rightSide; } }
        public IRSensor Left { get { return _leftSide; } }

        public IRSensor RearLeft { get { return _frontLeft; } }
        public IRSensor Rear { get { return _front; } }
        public IRSensor RearRight { get { return _frontRight; } }

    }
}
