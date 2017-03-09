using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Messages
{
    public class ConnectionStatus
    {
        public const byte BotConnected = 140;
        public const byte BotDisconnected = 141;
        public const byte Ping = 250;
        public const byte Pong = 251;
    }
}
