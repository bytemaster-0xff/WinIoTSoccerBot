using LagoVista.Core.UWP.Services;
using SoccerBot.Core.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using Windows.Networking.Sockets;
using System.Diagnostics;
using LagoVista.Core.Models.Drawing;
using SoccerBot.Core.Models;
using SoccerBot.Core.Messages;

namespace SoccerBot.mBot.Channels
{
    public class Server
    {
        ISoccerBotLogger _logger;
        ISoccerBot _soccerBot;
        List<Client> _clients;
        TCPListener _listener;

        System.Threading.Timer _watchDog;
        System.Threading.Timer _sensorUpdateTimer;
        int _port;

        Object _clientAccessLocker = new object();


        public Server(ISoccerBotLogger logger, int port, ISoccerBot soccerBot)
        {
            _port = port;
            _logger = logger;
            _soccerBot = soccerBot;

            _listener = new TCPListener(_logger, this, _port);
            _clients = new List<Client>();

            _watchDog = new System.Threading.Timer(_watchDog_Tick, null, 0, 2500);
            _sensorUpdateTimer = new System.Threading.Timer(_sensorUpdateTimer_Tick, null, 0, 1000);
        }

        public void Start()
        {
            _listener.StartListening();
        }

        private async void _sensorUpdateTimer_Tick(object sender)
        {
            var sensorMessage = new Core.Messages.SensorData();

            sensorMessage.Version = _soccerBot.FirmwareVersion;
            sensorMessage.DeviceName = _soccerBot.DeviceName;
            sensorMessage.FrontSonar = _soccerBot.FrontSonar;
            sensorMessage.Compass = _soccerBot.Compass;

            var msg = NetworkMessage.CreateJSONMessage(sensorMessage, Core.Messages.SensorData.MessageTypeId);

            var connectedClients = _clients.Where(clnt => clnt.IsConnected == true).ToList();

            foreach (var client in connectedClients)
            {
                await client.Write(msg.GetBuffer());
            }

        }

        private void _watchDog_Tick(object state)
        {
            var clientsToRemove = new List<Client>();
            foreach (var client in _clients)
            {
                if (client.IsConnected == false)
                    clientsToRemove.Add(client);
            }

            Debug.WriteLine($"CLient Count {_clients.Count} Remove Count {clientsToRemove.Count}");

            if (clientsToRemove.Count > 0 && _clients.Count > 0)
            {
                if (_clients.Count == 1)
                {
                    if (_soccerBot.LastBotContact.HasValue && ((DateTime.Now - _soccerBot.LastBotContact) < TimeSpan.FromSeconds(10)))
                    {
                        _soccerBot.SetLED(0, NamedColors.Yellow);
                    }
                    else
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
        }

        public void ClientConnected(StreamSocket socket)
        {
            _soccerBot.PlayTone(400);
            _soccerBot.SetLED(0, NamedColors.Green);

            lock (_clients)
            {
                var client = Client.Create(socket, _logger);
                client.MessageRecevied += Client_MessageRecevied;
                _clients.Add(client);
                client.StartListening();
            }
        }

        private void Client_MessageRecevied(object sender, NetworkMessage e)
        {
            Debug.WriteLine("XXXX___MESSAGE RECEV===> " + e.MessageTypeCode);

            switch (e.MessageTypeCode)
            {
                case Move.MessageTypeId:
                    {
                        var movePayload = e.DeserializePayload<Move>();
                        _soccerBot.Move(movePayload.Speed, movePayload.RelativeHeading, movePayload.AbsoluteHeading, movePayload.Duration);
                    }

                    break;
                case Stop.MessageTypeId:
                    _soccerBot.Stop();
                    break;
            }
        }
    }
}
