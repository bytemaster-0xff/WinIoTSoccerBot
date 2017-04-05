using SoccerBot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Commanding;
using LagoVista.Core.Models.Drawing;
using SoccerBot.Core.Messages;

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
        public SensorData SensorData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void PlayTone(short frequency)
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

        public void Reset()
        {

        }

        public  void SetLED(byte index, Color color)
        {

        }

        public void Move(short speed = 0, short? relativeHeading = 0, short? absoluteHeading = 0, short? duration = 0)
        {

        }

        public void Stop()
        {

        }
    }
}
