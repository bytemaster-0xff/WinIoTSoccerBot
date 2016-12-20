using SoccerBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBotApp.Protocols
{
    public class mBlockMessage : Message
    {
        public enum CommandTypes
        {
            Get = 0x01,
            Run = 0x02,
            Reset = 0x04,
            Start = 0x05,
        }

        public enum Devices
        {
            VERSION = 0x00,
            ULTRASONIC_SENSOR = 0x01,
            TEMPERATURE_SENSOR = 2,
            LIGHT_SENSOR = 3,
            POTENTIONMETER = 4,
            JOYSTICK = 5,
            GYRO = 6,
            SOUND_SENSOR = 7,
            RGBLED = 8,
            SEVSEG = 9,
            MOTOR = 10,
            SERVO = 11,
            ENCODER = 12,
            IR = 13,
            IRREMOTE = 14,
            PIRMOTION = 15,
            INFRARED = 16,
            LINEFOLLOWER = 17,
            IRREMOTECODE = 18,
            SHUTTER = 20,
            LIMITSWITCH = 21,
            BUTTON = 22,
            DIGITAL = 30,
            ANALOG = 31,
            PWM = 32,
            SERVO_PIN = 33,
            TONE = 34,
            BUTTON_INNER = 35,
            TIMER = 50,
        }


        public CommandTypes CommandType { get; set; }
        public Devices Device { get; set; }

        public byte? Port { get; set; }

        public byte? Slot { get; set; }

        public byte? Data { get; set; }

    }
}
