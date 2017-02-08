using System;
using System.Collections.Generic;
using System.Text;

namespace SoccerBot.Core.Models
{
    public class Message
    {
        private List<byte> _buffer;

        public Message()
        {
            _buffer = new List<byte>();
        }
        

        public DateTime DateStamp { get; set; }
        public virtual Byte[] Buffer { get {  return _buffer.ToArray() ;  } }

        public int BufferSize { get { return Buffer == null ? 0 : Buffer.Length; } }
        public String MessageHexString
        {
            get
            {
                if (Buffer != null && BufferSize > 0)
                {
                    var byteMessageBuilder = new StringBuilder();
                    foreach (var value in Buffer)
                    {
                        byteMessageBuilder.AppendFormat(" 0x{0:x2}", value);
                    }
                    return byteMessageBuilder.ToString();
                }

                return "[empty]";
            }
        }

        public void AddByte(byte value)
        {
            _buffer.Add(value);
        }

        public bool EndsWithCRLF()
        {
            if(Buffer.Length < 2)
            {
                return false;
            }

            if(Buffer[Buffer.Length - 2] == 0x0D && Buffer[Buffer.Length - 1] == 0x0A)
            {
                DateStamp = DateTime.Now;
                return true;
            }

            return false;
        }
    }
}
