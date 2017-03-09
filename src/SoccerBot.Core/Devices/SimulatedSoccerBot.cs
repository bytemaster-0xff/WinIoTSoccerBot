using SoccerBot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Commanding;
using LagoVista.Core.Models.Drawing;

namespace SoccerBot.Core.Devices
{
    public class SimulatedSoccerBot : SoccerBotBase, ISoccerBot
    {
        public ISensor FrontSonar
        {
            get; set;
        }

        public ISensor Compass
        {
            get; set;
        }

        public override void PlayTone(short frequency)
        {
            
        }

        protected override void RefreshSensors()
        {
            
        }

        protected override void SendCommand(object cmd)
        {
            
        }

        protected override void SpeedUpdated(short speed)
        {
            
        }

        public override void SetLED(byte index, Color color)
        {

        }

        public void Move(short heading, short speed, int? durationMS = default(int?))
        {

        }

        public void Stop()
        {

        }
    }
}
