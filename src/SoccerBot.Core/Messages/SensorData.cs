using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Messages
{
    public class SensorData
    {
        public const int MessageTypeId = 130;

        public string Platform { get; set; }
        public string Version { get; set; }

        public short? Heading { get; set; }

        public short? FrontSonar { get; set; }

        public bool FrontProximity { get; set; }
        public bool FrontRightProximity { get; set; }
        public bool RightRearProximity { get; set; }
        public bool RearPoximity { get; set; }
        public bool LeftRearPoximity { get; set; }
        public bool LeftProximity { get; set; }
        public bool FrontLeftProximity { get; set; }

        public bool TurretAngle { get; set; }

    }
}
