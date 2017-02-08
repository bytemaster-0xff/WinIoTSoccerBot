using SoccerBot.Core.Models;
using System;
using System.Collections.ObjectModel;

namespace SoccerBot.Core.Interfaces
{
    public interface ISoccerBotLogger
    {
        ObservableCollection<Notification> Notifications { get;  }
        void NotifyUserInfo(Notification notification);
        void NotifyUserInfo(String source, String msg);
        void NotifyUserWarning(String source, String msg);
        void NotifyUserError(String source, String msg);
        void Clear();
    }
}
