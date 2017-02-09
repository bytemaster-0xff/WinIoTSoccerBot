using LagoVista.Core.Networking.Interfaces;
using LagoVista.Core.Networking.Models;
using LagoVista.Core.Networking.Services;
using SoccerBot.Core.Interfaces;
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

        ISoccerBotCommands _soccerBot;
        ISoccerBotLogger _logger;

        private static string _deviceName;

        static ConnectionManager _instance = new ConnectionManager();

        public void Init(ISoccerBotCommands soccerBot, ISoccerBotLogger logger)
        {
            _soccerBot = soccerBot;
            _logger = logger;
        }

        public static string GetDefaultPageHTML(string message)
        {
            var html = @"<head>
<title>SoccerBo</title>
<link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"" integrity=""sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u"" crossorigin=""anonymous"">
</head>
<body>
<h1>" + _deviceName + @" Soccer Bot Api Page</h1>
<h2>" + message + @"</h2> 
<img src='https://raw.githubusercontent.com/bytemaster-0xff/WinIoTSoccerBot/master/Documentation/BasicVersion.jpg' />
<div class='row'>
<div class='col-md-1'><a class='btn btn-success' href='/motion/forward/150' >Forward</a></div>
<div class='col-md-1'><a class='btn btn-success' href='/motion/backwards/150' >Back</a></div>
<div class='col-md-1'><a class='btn btn-success' href='/motion/left/150' >Left</a></div>
<div class='col-md-1'><a class='btn btn-success' href='/motion/right/150' >Right</a></div>
<div class='col-md-1'><a class='btn btn-success' href='/motion/stop/150' >Stop</a></div>
<div class='col-md-6'></div>
</div>
</body>
</html>";

            return html;
        }


        private ConnectionManager()
        {

        }

        public static ConnectionManager Instance { get { return _instance; } }

        public void MakeDiscoverable(string name, string serialNumber)
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
<h1>Soccer Bot SSDP (uPnP) Listener Page</h1>
</body>
</html>"
            };


            _server = NetworkServices.GetSSDPServer();
            _server.MakeDiscoverable(9500, _configuration);
        }

        public void StartWebServer(int port, string name)
        {
            _deviceName = name;

            _webServer = NetworkServices.GetWebServer();
            _webServer.RegisterAPIHandler(new Api.MotionApi(_soccerBot, _logger));
            _webServer.DefaultPageHtml = GetDefaultPageHTML("Ready");
            _webServer.StartServer(port);
        }
    }
}
