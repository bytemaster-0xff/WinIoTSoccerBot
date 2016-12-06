using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBotApp.Protocols
{

    public class mBlockMessage
    {
        /* According to the mbot protocol from review of the source at: https://github.com/Makeblock-official/Makeblock-Firmware
         * This is the structure of the message:
         * 
         * ff 55 len idx action device port slot data a
         * 0  1  2   3   4      5      6    7    8
         * 
         * 
         */

        public static byte MessageIndex { get; private set; }

        private mBlockMessage()
        {

        }

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

        const byte HEADER_LENGTH = 3;

        public byte[] Buffer
        {
            get
            {
                byte length = 6;

                if (Slot.HasValue && !Port.HasValue)
                    throw new Exception("If you specify a slot you must also specify a port");

                if(Data.HasValue && (!Port.HasValue || !Slot.HasValue))
                    throw new Exception("If you specify data, you must also specify port and slot");

                if (Port.HasValue) length++;
                if (Slot.HasValue) length++;
                if (Data.HasValue) length++;

                var buffer = new byte[length];
                buffer[0] = 0xFF;
                buffer[1] = 0x55;
                buffer[2] = Convert.ToByte(length - HEADER_LENGTH);
                buffer[3] = MessageIndex++;                
                buffer[4] = Convert.ToByte(CommandType);
                buffer[5] = Convert.ToByte(Device);
                if (Port.HasValue) buffer[6] = Port.Value;
                if (Slot.HasValue) buffer[7] = Slot.Value;
                if (Data.HasValue) buffer[7] = Data.Value;

                return buffer;
            }
        }

        public static mBlockMessage CreateMessage(CommandTypes command, Devices device)
        {
            return new mBlockMessage()
            {
                CommandType = command,
                Device = device
            };
        }

        public static mBlockMessage CreateMessage(CommandTypes command, Devices device, byte port)
        {
            return new mBlockMessage()
            {
                CommandType = command,
                Device = device,
                Port = port
            };
        }

        public static mBlockMessage CreateMessage(CommandTypes command, Devices device, byte port, byte slot)
        {
            return new mBlockMessage()
            {
                CommandType = command,
                Device = device,
                Port = port,
                Slot = slot
            };
        }

        public static mBlockMessage CreateMessage(CommandTypes command, Devices device, byte port, byte slot, byte data)
        {
            return new mBlockMessage()
            {
                CommandType = command,
                Device = device,
                Port = port,
                Slot = slot,
                Data = data
            };
        }
    }
}
