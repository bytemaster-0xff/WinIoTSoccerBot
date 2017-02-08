using LagoVista.Core.Networking.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.mBot.Api
{
    public class MotionApi : IApiHandler
    {
        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET)]
        public void Forward(string speed)
        {

        }

        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET)]
        public void Left(string speed)
        {

        }

        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET)]
        public void Right(string speed)
        {

        }

        [MethodHandler(MethodHandlerAttribute.MethodTypes.GET)]
        public void Backwards(string speed)
        {

        }
    }
}
