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
        RelayCommand ForwardCommand { get; }
        RelayCommand LeftCommand { get; }
        RelayCommand RightCommand { get; }
        RelayCommand BackwardsCommand { get; }
        RelayCommand StopCommand { get; }

        RelayCommand ResetCommand { get; }
    }
}