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
            SETMODE = 100,
        }
        

        public enum Ports
        {
            PORT_0 = 0x00,
            PORT_1 = 0x01,
            PORT_2 = 0x02,
            PORT_3 = 0x03,
            PORT_4 = 0x04,
            PORT_5 = 0x05,
            PORT_6 = 0x06,
            PORT_7 = 0x07,
            PORT_8 = 0x08,
            M1 = 0x09,
            M2 = 0x0a,
            MBOTH = 0x80,
            MODE_A = 0x81,
            MODE_B = 0x82,
            MODE_C = 0x82,
        }

        public CommandTypes CommandType { get; set; }
        public Devices Device { get; set; }

        public int? Port { get; set; }

        public byte? Slot { get; set; }

        public byte? Data { get; set; }

        public byte MessageSerialNumber { get; set; }

    }
}
