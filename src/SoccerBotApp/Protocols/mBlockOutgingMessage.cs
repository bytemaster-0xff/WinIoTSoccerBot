using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBotApp.Protocols
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

        private int _messageIndex;

        private mBlockOutgingMessage()
        {
            _messageIndex = MessageIndexCounter++;
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

                var buffer = new byte[length];
                buffer[0] = 0xFF;
                buffer[1] = 0x55;
                buffer[2] = Convert.ToByte(length - HEADER_LENGTH);
                buffer[3] = (byte)(_messageIndex & 0xFF);                
                buffer[4] = Convert.ToByte(CommandType);
                buffer[5] = Convert.ToByte(Device);
                if (Port.HasValue) buffer[6] = Port.Value;
                if (Slot.HasValue) buffer[7] = Slot.Value;
                if (Data.HasValue) buffer[7] = Data.Value;

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

        public static mBlockOutgingMessage CreateMessage(CommandTypes command, Devices device, byte port)
        {            
            return new mBlockOutgingMessage()
            {
                CommandType = command,
                Device = device,
                Port = port
            };
        }

        public static mBlockOutgingMessage CreateMessage(CommandTypes command, Devices device, byte port, byte slot)
        {
            return new mBlockOutgingMessage()
            {
                CommandType = command,
                Device = device,
                Port = port,
                Slot = slot
            };
        }

        public static mBlockOutgingMessage CreateMessage(CommandTypes command, Devices device, byte port, byte slot, byte data)
        {
            return new mBlockOutgingMessage()
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
