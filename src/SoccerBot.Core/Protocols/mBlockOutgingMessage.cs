using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Protocols
{

    public class mBlockOutgingMessage : mBlockMessage
    {
        /* According to the mbot protocol from review of the source at: https://github.com/Makeblock-official/Makeblock-Firmware
         * This is the structure of the message:
         * 
         * ff 55 len idx action device port slot data a
         * 0  1  2   3   4      5      6    7    8
         * 
         * 
         */

        public static byte MessageIndexCounter { get; protected set; }


        byte[] _payload = null;

        private mBlockOutgingMessage()
        {
            MessageSerialNumber = MessageIndexCounter++;
            DateStamp = DateTime.Now;
        }

        const byte HEADER_LENGTH = 3;

        public override byte[] Buffer
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

                if(_payload != null && _payload.Length > 0)
                {
                    length += (byte)_payload.Length;
                }

                var position = 0;
                var buffer = new byte[length];
                buffer[position++] = 0xFF;
                buffer[position++] = 0x55;
                buffer[position++] = Convert.ToByte(length - HEADER_LENGTH);
                buffer[position++] = (byte)(MessageSerialNumber & 0xFF);                
                buffer[position++] = Convert.ToByte(CommandType);
                buffer[position++] = Convert.ToByte(Device);
                if (Port.HasValue) buffer[position++] = Convert.ToByte(Port.Value);
                if (Slot.HasValue) buffer[position++] = Slot.Value;
                if (Data.HasValue) buffer[position++] = Data.Value;

                if (_payload != null)
                {
                    for (var idx = 0; idx < _payload.Length; ++idx)
                    {
                        buffer[position++] = _payload[idx];
                    }
                }

                return buffer;
            }
        }

        public static mBlockOutgingMessage CreateMessage(CommandTypes command, Devices device)
        {           
            return new mBlockOutgingMessage()
            {
                CommandType = command,
                Device = device
            };
        }

        public static mBlockOutgingMessage CreateMessage(CommandTypes command, Devices device, Ports port)
        {            
            return new mBlockOutgingMessage()
            {
                CommandType = command,
                Device = device,
                Port = (int)port
            };
        }

        public static mBlockOutgingMessage CreateMessage(CommandTypes command, Devices device, Ports port, byte[] payload)
        {
            return new mBlockOutgingMessage()
            {
                CommandType = command,
                Device = device,
                Port = (int)port,
                _payload = payload
            };
        }

        public static mBlockOutgingMessage CreateMessage(CommandTypes command, Devices device, int port, byte[] payload)
        {
            return new mBlockOutgingMessage()
            {
                CommandType = command,
                Device = device,
                Port = port,
                _payload = payload
            };
        }

        public static mBlockOutgingMessage CreateMessage(CommandTypes command, Devices device, Ports port, byte slot)
        {
            return new mBlockOutgingMessage()
            {
                CommandType = command,
                Device = device,
                Port = (int)port,
                Slot = slot
            };
        }

        public static mBlockOutgingMessage CreateMessage(CommandTypes command, Devices device, Ports port, byte slot, byte data)
        {
            return new mBlockOutgingMessage()
            {
                CommandType = command,
                Device = device,
                Port = (int)port,
                Slot = slot,
                Data = data
            };
        }

        public Action<mBlockIncomingMessage> Handler { get; set; }
    }
}
