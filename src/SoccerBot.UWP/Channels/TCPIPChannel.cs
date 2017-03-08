using LagoVista.Core.Networking.Models;
using SoccerBot.Core.Channels;
using SoccerBot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.UWP.Channels
{
    public class TCPIPChannel : ChannelBase
    {
        Windows.Networking.Sockets.StreamSocketListener _listener;

        ISoccerBotLogger _logger;
        public TCPIPChannel(uPnPDevice device, ISoccerBotLogger logger)
        {
            DeviceName = device.FriendlyName;
            Id = device.UDN + " " + device.IPAddress;
            _logger = logger;
            _logger.NotifyUserInfo("TCPIP Channel", $"Created Device for: {DeviceName}");
        }

        public async Task StartListening()
        {
            _listener = new Windows.Networking.Sockets.StreamSocketListener();

            //Hook up an event handler to call when connections are received.
            _listener.ConnectionReceived += _listener_ConnectionReceived;

            //Start listening for incoming TCP connections on the specified port. You can specify any port that' s not currently in use.
            await _listener.BindServiceNameAsync("9050");
        }

        private async void _listener_ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender, Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Stream inStream = args.Socket.InputStream.AsStreamForRead();
            StreamReader reader = new StreamReader(inStream);
            string request = await reader.ReadLineAsync();

            //Send the line back to the remote client.
            Stream outStream = args.Socket.OutputStream.AsStreamForWrite();
            StreamWriter writer = new StreamWriter(outStream);
            await writer.WriteLineAsync(request);
            await writer.FlushAsync();


            throw new NotImplementedException();
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
