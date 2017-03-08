using SoccerBot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.mBot.Channels
{
    public class TCPListener
    {
        Windows.Networking.Sockets.StreamSocketListener _listener;

        Server _server;
        ISoccerBotLogger _logger;
        public TCPListener(ISoccerBotLogger logger, Server server)
        {
            _server = server;
            _logger = logger;
            _logger.NotifyUserInfo("TCPIP Listener", $"Created Listener");
            _listener = new Windows.Networking.Sockets.StreamSocketListener();
            _listener.ConnectionReceived += _listener_ConnectionReceived;
        }

        private  void _listener_ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender, Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {
            _server.ClientConnected(args.Socket);
        }

        public async void StartListening()
        {
            await _listener.BindServiceNameAsync("9050");
        }

    }
}
