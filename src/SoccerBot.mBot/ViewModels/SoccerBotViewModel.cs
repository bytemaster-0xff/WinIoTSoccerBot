using SoccerBot.Core.Interfaces;
using SoccerBot.mBot.Channels;
using SoccerBot.mBot.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace SoccerBot.mBot.ViewModels
{
    public class SoccerBotViewModel
    {

        ISoccerBotLogger _logger;
        ISoccerBot _soccerBot;
        Server _server;


        public SoccerBotViewModel(ISoccerBot soccerBot, ISoccerBotLogger logger, Server server)
        {
            _server = server;

            _logger = logger;

            _soccerBot = soccerBot;
        }

        public ISoccerBot SoccerBot { get { return _soccerBot; } }
        
        public DateTime LastRemotePing { get; set; }
        public DateTime LastBotPing { get; set; }
        public String IPAddress { get; set; }

    }
}
