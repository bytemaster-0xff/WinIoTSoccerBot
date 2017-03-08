using LagoVista.Core.UWP.Services;
using SoccerBot.Core.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using Windows.Networking.Sockets;

namespace SoccerBot.mBot.Channels
{
    public class Server
    {
        ISoccerBotLogger _logger;
        List<Client> _clients;
        TCPListener _listener;

        Timer _watchDog;
        int _port;

        Object _clientAccessLocker = new object();

        public Server(ISoccerBotLogger logger, int port)
        {
            _port = port;
            _logger = logger;
            _listener = new TCPListener(_logger, this, _port);
            _clients = new List<Client>();

            _watchDog = new Timer();
            _watchDog.Interval = TimeSpan.FromSeconds(1);
            _watchDog.Tick += _watchDog_Tick;
            _watchDog.Start();
        }

        public void Start()
        {
            _listener.StartListening();
        }

        private void _watchDog_Tick(object sender, EventArgs e)
        {

            var clientsToRemove = _clients.Where(clnt => clnt.IsConnected == false).ToList();

            foreach (var client in clientsToRemove)
            {
                _clients.Remove(client);
                client.Disconnect();
                client.Dispose();
            }

        }

        public void ClientConnected(StreamSocket socket)
        {
            lock (_clients)
            {
                var client = Client.Create(socket);
                client.SendWelcome();
                _clients.Add(client);
                client.StartListening();
            }
        }
    }
}
