using SoccerBot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Commanding;

namespace SoccerBot.Core.Devices
{
    public class SimulatedSoccerBot : SoccerBotBase, ISoccerBot
    {
        protected override void RefreshSensors()
        {
            
        }

        protected override void SendCommand(object cmd)
        {
            
        }

        protected override void SpeedUpdated(short speed)
        {
            
        }
    }
}
