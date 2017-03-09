using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Messages
{
    public class Move
    {
        public const int MessageTypeId = 50;
      
        public short? RelativeHeading { get; set; }

        public short? AbsoluteHeading { get; set; }

        public short Speed { get; set; }

        public short? Duration { get; set; }
    }

    public class Moves
    {
        public const int MessageTypeId = 51;

        public List<Move> MoveList { get; set; }
    }
}
