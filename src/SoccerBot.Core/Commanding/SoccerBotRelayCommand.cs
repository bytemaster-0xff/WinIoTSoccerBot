using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Commanding
{
    public class SoccerBotRelayCommand<T>
    {

        public static SoccerBotRelayCommand<T> Create(Action action, T parameter)
        {
            return new SoccerBotRelayCommand<T>();
        }
    }
}
