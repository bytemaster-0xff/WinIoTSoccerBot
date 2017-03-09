using SoccerBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Messages
{
    public class SystemMessages
    {
        public const byte BotConnected = 140;
        public const byte BotDisconnected = 141;
        public const byte Reset = 142;
        public const byte Ping = 250;
        public const byte Pong = 251;

        public static NetworkMessage CreatePing()
        {
            return NetworkMessage.CreateEmptyMessage(SystemMessages.Ping);
        }

        public static NetworkMessage CreatePong()
        {
            return NetworkMessage.CreateEmptyMessage(SystemMessages.Pong);
        }

        public static NetworkMessage CreateBotConnected()
        {
            return NetworkMessage.CreateEmptyMessage(SystemMessages.BotConnected);
        }

        public static NetworkMessage CreateBotDisconnected()
        {
            return NetworkMessage.CreateEmptyMessage(SystemMessages.BotDisconnected);
        }

        public static NetworkMessage CreateReset()
        {
            return NetworkMessage.CreateEmptyMessage(SystemMessages.Reset);
        }
    }
}
