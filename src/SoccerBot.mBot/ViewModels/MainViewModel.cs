using SoccerBot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.mBot.ViewModels
{
    public class MainViewModel
    {
        ISoccerBot _soccerBot;

        public ISoccerBot SoccerBot { get { return _soccerBot; } }
        
        public DateTime LastRemotePing { get; set; }
        public DateTime LastBotPing { get; set; }
        public String IPAddress { get; set; }

    }
}
