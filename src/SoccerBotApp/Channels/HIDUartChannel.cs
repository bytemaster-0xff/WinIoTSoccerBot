using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBotApp.Channels
{
    public class HIDUartChannel : ChannelBase
    {
        public const string VID = "0416";
        public const string PID = "FFFF";

        public override void Connect()
        {
            throw new NotImplementedException();
        }

        public override void Disconnect()
        {
            throw new NotImplementedException();
        }

        public override Task WriteBuffer(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public override Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }
    }
}
