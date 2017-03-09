using SoccerBot.Core.Interfaces;
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

        public string DeviceName { get; set; }
        public string Version { get; set; }

        public ISensor Compass { get; set; }

        public ISensor FrontSonar { get; set; }

    }
}
