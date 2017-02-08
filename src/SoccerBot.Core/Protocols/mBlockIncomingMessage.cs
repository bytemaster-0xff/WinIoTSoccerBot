using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Protocols
{
    public class mBlockIncomingMessage : mBlockMessage
    {
        public enum PayloadType
        {
            Byte = 1,
            Float = 2,
            Short = 3,
            String = 4,
            Double = 5
        }

        public Single FloatPayload
        {
            get { return BitConverter.ToSingle(Buffer, 4); }
        }

        public String StringPayload
        {
            get
            {
                return System.Text.UTF8Encoding.UTF8.GetString(Buffer, 5, Buffer[4]);
            }
        }        
    }
}
