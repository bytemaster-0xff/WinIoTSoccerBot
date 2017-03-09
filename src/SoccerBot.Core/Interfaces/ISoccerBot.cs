using LagoVista.Core.Models.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Interfaces
{
    public interface ISoccerBot : ISoccerBotCommands, ISoccerBotSensors
    {
        String Id { get; }
        String Name { get; }
        String DeviceName { get; }
        String FirmwareVersion { get; }
        String DeviceTypeId { get; }
        String APIMode { get; }

        void PlayTone(short frequence);

        void SetLED(byte index, Color color);

        void Move(short heading, short speed, int? durationMS = null);

        void Stop();

        ISensor FrontSonar { get; set; }
        ISensor Compass { get; set; }
    }
}
