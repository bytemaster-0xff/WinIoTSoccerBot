using Newtonsoft.Json;
using SoccerBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Messages
{
    public class Move 
    {
        public const int MessageTypeId = 51;
      
        public short? RelativeHeading { get; set; }

        public short? AbsoluteHeading { get; set; }

        public short Speed { get; set; }

        public short? Duration { get; set; }

        public static NetworkMessage Create(short speed = 0, short? relativeHeading = 0, short? absoluteHeading = 0,  short? duration = 0)
        {
            var msg = new Move();
            msg.RelativeHeading = relativeHeading;
            msg.AbsoluteHeading = relativeHeading;
            msg.Speed = speed;
            msg.Duration = duration;

            return NetworkMessage.CreateJSONMessage(msg, Move.MessageTypeId);
        }
    }

    public class Stop
    {
        public const int MessageTypeId = 50;

        public static NetworkMessage Create()
        {
            return NetworkMessage.CreateEmptyMessage(Stop.MessageTypeId);
        }
    }
}
