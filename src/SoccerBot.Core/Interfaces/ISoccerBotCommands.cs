using LagoVista.Core.Commanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SoccerBot.Core.Devices.SoccerBotBase;

namespace SoccerBot.Core.Interfaces
{
    public interface ISoccerBotCommands
    {
        short Speed { get; set; }

        RelayCommand RefreshSensorsCommand { get; }
        RelayCommand<Commands> ForwardCommand { get; }
        RelayCommand<Commands> LeftCommand { get; }
        RelayCommand<Commands> RightCommand { get; }
        RelayCommand<Commands> BackwardsCommand { get; }
        RelayCommand<Commands> StopCommand { get; }
    }
}