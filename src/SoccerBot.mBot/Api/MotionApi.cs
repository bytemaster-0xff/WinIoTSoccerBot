using LagoVista.Core.Networking.Interfaces;
using LagoVista.Core.Networking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.mBot.Api
{
    public class MotionApi : IApiHandler
    {
        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET, FullPath = "/motion/forward/{speed}")]
        public HttpResponseMessage Forward(HttpRequestMessage msg, string speed)
        {
            var response = msg.GetResponseMessage();
            response.Content = Managers.ConnectionManager.GetDefaultPageHTML("Ok - starting forward");
            return response;
        }

        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET, FullPath = "/motion/left/{speed}")]
        public HttpResponseMessage Left(HttpRequestMessage msg, string speed)
        {
            var response = msg.GetResponseMessage();
            response.Content = Managers.ConnectionManager.GetDefaultPageHTML("Ok - starting left");
            return response;
        }

        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET, FullPath = "/motion/right/{speed}")]
        public HttpResponseMessage Right(HttpRequestMessage msg, string speed)
        {
            var response = msg.GetResponseMessage();
            response.Content = Managers.ConnectionManager.GetDefaultPageHTML("Ok - starting right");
            return response;
        }

        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET, FullPath = "/motion/backwards/{speed}")]
        public HttpResponseMessage Backwards(HttpRequestMessage msg, int speed)
        {
            var response = msg.GetResponseMessage();
            response.Content = Managers.ConnectionManager.GetDefaultPageHTML("Ok - starting backawards");
            return response;
        }

        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET, FullPath = "/motion/stop")]
        public HttpResponseMessage Stop(HttpRequestMessage msg)
        {
            var response = msg.GetResponseMessage();
            response.Content = Managers.ConnectionManager.GetDefaultPageHTML("Ok - stopping");
            return response;
        }
    }
}
