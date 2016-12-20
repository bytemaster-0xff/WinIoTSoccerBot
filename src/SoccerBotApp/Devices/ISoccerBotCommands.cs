using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBotApp.Devices
{
    public interface ISoccerBotCommands
    {
        Task MoveFowardAsync(byte speed);
        Task MoveBackwardsAsync(byte speed);

        Task TurnLeftAsync(float seconds);
        Task TurnRightAsync(float seconds);

        Task StopAsync();

        Task RefreshSensorsAsync();
        
    }
}
