using LagoVista.Core.Networking.Interfaces;
using LagoVista.Core.Networking.Services;
using SoccerBot.Core.Channels;
using SoccerBot.Core.Interfaces;
using SoccerBot.UWP.Channels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.UWP.Watchers
{
    public class UPNPChannelWatcher : ChannelWatcherBase, IChannelWatcher
    {
        ISSDPClient _ssdpClient;
        ISoccerBotLogger _logger;
       
        public UPNPChannelWatcher(ISoccerBotLogger logger) : base(logger)
        {
            _logger = logger;
        }

        protected override void StartWatcher()
        {
            _logger.NotifyUserInfo("TCPIP Mgr", $"Started Watcher");

            _ssdpClient = NetworkServices.GetSSDPClient();
            _ssdpClient.ShowDiagnostics = true;
            _ssdpClient.NewDeviceFound += _ssdpClient_NewDeviceFound;
            _ssdpClient.SsdpQueryAsync();
        }

        private void _ssdpClient_NewDeviceFound(object sender, LagoVista.Core.Networking.Models.uPnPDevice device)
        {
             if(device.FriendlyName.StartsWith("ByteMa"))
            {
                _logger.NotifyUserInfo("TCPIP Mgr", "Found Channel =>: " + device.FriendlyName);
                RaiseDeviceFoundEvent(new TCPIPChannel(device, _logger));
            }
        }

        protected override void StopWatcher()
        {
            _logger.NotifyUserInfo("TCPIP Mgr", $"Stopped Watcher");
            _ssdpClient.Cancel();
        }
    }
}
