using SoccerBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBotApp.Protocols
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

    }
}
