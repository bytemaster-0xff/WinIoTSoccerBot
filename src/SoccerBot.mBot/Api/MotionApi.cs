using LagoVista.Core.Networking.Interfaces;
using LagoVista.Core.Networking.Models;
using SoccerBot.Core.Interfaces;

namespace SoccerBot.mBot.Api
{
    public class MotionApi : IApiHandler
    {
        ISoccerBotCommands _soccerBot;
        ISoccerBotLogger _logger;

        public MotionApi(ISoccerBotCommands soccerBot, ISoccerBotLogger logger)
        {
            _soccerBot = soccerBot;
            _logger = logger;
        }

        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET, FullPath = "/reset")]
        public HttpResponseMessage Reset(HttpRequestMessage msg)
        {
            _soccerBot.ResetCommand.Execute(null);

            var response = msg.GetResponseMessage();
            response.ContentType = "text/html";
            response.Content = Managers.ConnectionManager.Instance.GetDefaultPageHTML("Ok - Resetting");
            return response;
        }

        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET, FullPath = "/motion/forward/{speed}")]
        public HttpResponseMessage Forward(HttpRequestMessage msg, int speed)
        {
            _soccerBot.Speed = (short)speed;
            _soccerBot.ForwardCommand.Execute(null);

            var response = msg.GetResponseMessage();
            response.ContentType = "text/html";
            response.Content = Managers.ConnectionManager.Instance.GetDefaultPageHTML("Ok - starting forward");
            return response;
        }

        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET, FullPath = "/motion/left/{speed}")]
        public HttpResponseMessage Left(HttpRequestMessage msg, int speed)
        {
            _soccerBot.Speed = (short)speed;
            _soccerBot.LeftCommand.Execute(null);

            var response = msg.GetResponseMessage();
            response.ContentType = "text/html";
            response.Content = Managers.ConnectionManager.Instance.GetDefaultPageHTML("Ok - starting left");
            return response;
        }

        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET, FullPath = "/motion/right/{speed}")]
        public HttpResponseMessage Right(HttpRequestMessage msg, int speed)
        {
            _soccerBot.Speed = (short)speed;
            _soccerBot.RightCommand.Execute(null);

            var response = msg.GetResponseMessage();
            response.ContentType = "text/html";
            response.Content = Managers.ConnectionManager.Instance.GetDefaultPageHTML("Ok - starting right");
            return response;
        }

        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET, FullPath = "/motion/backwards/{speed}")]
        public HttpResponseMessage Backwards(HttpRequestMessage msg, int speed)
        {
            _soccerBot.Speed = (short)speed;
            _soccerBot.BackwardsCommand.Execute(null);

            var response = msg.GetResponseMessage();
            response.ContentType = "text/html";
            response.Content = Managers.ConnectionManager.Instance.GetDefaultPageHTML("Ok - starting backwards");
            return response;
        }

        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET, FullPath = "/motion/stop")]
        public HttpResponseMessage Stop(HttpRequestMessage msg)
        {
            var response = msg.GetResponseMessage();
            _soccerBot.StopCommand.Execute(null);
            response.ContentType = "text/html";
            response.Content = Managers.ConnectionManager.Instance.GetDefaultPageHTML("Ok - stopping");
            return response;
        }
    }
}
