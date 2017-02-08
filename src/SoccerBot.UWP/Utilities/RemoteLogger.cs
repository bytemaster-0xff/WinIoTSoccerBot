using SoccerBot.Core.Interfaces;
using SoccerBot.Core.Models;
using System;
using System.Collections.ObjectModel;

namespace SoccerBot.UWP.Utilities
{
    public class RemoteLogger : ISoccerBotLogger
    {
        public ObservableCollection<Notification> Notifications { get; private set; }

        public RemoteLogger()
        {
            Notifications = new ObservableCollection<Notification>();
        }

        public void NotifyUserError(string source, string msg)
        {
            throw new NotImplementedException();
        }

        public void NotifyUserInfo(Notification notification)
        {
            throw new NotImplementedException();
        }

        public void NotifyUserInfo(string source, string msg)
        {
            throw new NotImplementedException();
        }

        public void NotifyUserWarning(string source, string msg)
        {
            throw new NotImplementedException();
        }
        public void Clear()
        {
            Notifications.Clear();
        }
    }
}
