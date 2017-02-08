using LagoVista.Core.Networking.Interfaces;
using LagoVista.Core.Networking.Models;
using LagoVista.Core.Networking.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.mBot.Managers
{
    public class ConnectionManager
    {
        ISSDPServer _server;
        IWebServer _webServer;

        static ConnectionManager _instance = new ConnectionManager();

        private ConnectionManager()
        {

        }

        public static ConnectionManager Instance { get { return _instance; } }

        public  void StartListening(string name, string serialNumber)
        {
            var _configuration = new UPNPConfiguration()
            {
                UdpListnerPort = 1900,
                FriendlyName = name,
                Manufacture = "Tampa IoT Dev",
                SerialNumber = serialNumber,
                DefaultPageHtml = @"<html>
<head>
<title>SoccerBo</title>
<link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"" integrity=""sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u"" crossorigin=""anonymous"">
</head>
<body>
<h1>Soccer Bot Listener</h1>
</body>
</html>"
            };


            _server = NetworkServices.GetSSDPServer();
            _server.MakeDiscoverable(9501, _configuration);

            _webServer = NetworkServices.GetWebServer();
            _webServer.StartServer(80);
        }
    }
}
