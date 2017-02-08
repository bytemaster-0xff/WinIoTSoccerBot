using LagoVista.Core.Commanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Interfaces
{
    public interface ISoccerBotCommands
    {
        short Speed { get; set; }

        RelayCommand RefreshSensorsCommand { get; }
        RelayCommand ForwardCommand { get; }
        RelayCommand StopCommand { get; }
    }
}