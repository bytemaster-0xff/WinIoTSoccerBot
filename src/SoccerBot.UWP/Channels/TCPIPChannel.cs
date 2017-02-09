using LagoVista.Core.Networking.Models;
using SoccerBot.Core.Channels;
using SoccerBot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.UWP.Channels
{
    public class TCPIPChannel : ChannelBase
    {
        ISoccerBotLogger _logger;
        public TCPIPChannel(uPnPDevice device, ISoccerBotLogger logger)
        {
            DeviceName = device.FriendlyName;
            Id = device.UDN + " " + device.IPAddress;
            _logger = logger;
            _logger.NotifyUserInfo("TCPIP Channel", $"Created Device for: {DeviceName}");
        }

        public override void Connect()
        {
           
        }

        public override Task<bool> ConnectAsync()
        {
            return Task.FromResult(true);
        }

        public override void Disconnect()
        { 
            
        }

        public override Task DisconnectAsync()
        {
            return Task.FromResult(default(object));
        }

        public override Task WriteBuffer(byte[] buffer)
        {
            return Task.FromResult(default(object));
        }
    }
}
