using LagoVista.Core.UWP.Services;
using SoccerBot.Core.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using Windows.Networking.Sockets;
using System.Diagnostics;
using LagoVista.Core.Models.Drawing;

namespace SoccerBot.mBot.Channels
{
    public class Server
    {
        ISoccerBotLogger _logger;
        ISoccerBot _soccerBot;
        List<Client> _clients;
        TCPListener _listener;

        Timer _watchDog;
        int _port;

        Object _clientAccessLocker = new object();

        public Server(ISoccerBotLogger logger, int port, ISoccerBot soccerBot)
        {
            _port = port;
            _logger = logger;
            _soccerBot = soccerBot;

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
            _watchDog.Stop();
            var clientsToRemove = _clients.Where(clnt => clnt.IsConnected == false).ToList();

            if(clientsToRemove.Count > 0)
            {
                if(_clients.Count == 1)
                {
                    _soccerBot.SetLED(0, NamedColors.Red);
                }
                _soccerBot.PlayTone(200);
            }

            foreach (var client in clientsToRemove)
            {
                try
                {
                    _clients.Remove(client);
                    client.Disconnect();
                    client.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            _watchDog.Start();
        }

        public void ClientConnected(StreamSocket socket)
        {
            _soccerBot.PlayTone(400);
            _soccerBot.SetLED(0, NamedColors.Green);

            lock (_clients)
            {
                var client = Client.Create(socket, _logger);
                client.SendWelcome();
                _clients.Add(client);
                client.StartListening();
            }
        }
    }
}
