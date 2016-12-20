using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.HumanInterfaceDevice;

namespace SoccerBotApp.Managers
{
    public class HID_UARTChannelWatcher : ChannelWatcherBase
    {
        public const string USAGE_PAGE = "0416";
        public const string UART_VID = "0416";
        public const string UART_PID = "FFFF";

        protected override void StartWatcher()
        {

        }

        protected override void StopWatcher()
        {
            throw new NotImplementedException();
        }
    }
}
