using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Interfaces
{
    public interface ISoccerBot : ISoccerBotCommands, ISoccerBotSensors
    {
        String Id { get;  }
        String Name { get;  }
        String DeviceName { get; }
        String FirmwareVersion { get; }
        String DeviceTypeId { get; }

    }
}
