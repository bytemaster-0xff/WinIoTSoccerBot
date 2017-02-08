using SoccerBot.Core.Interfaces;
using SoccerBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.UWP.Utilities
{
    public class SoccerBotAppLogger : ISoccerBotLogger
    {
        public ObservableCollection<Notification> Notifications { get; private set; }

        public SoccerBotAppLogger()
        {
            Notifications = new ObservableCollection<Notification>();
        }

        public void NotifyUserInfo(Notification notification)
        {
            LagoVista.Core.PlatformSupport.Services.DispatcherServices.Invoke(() =>
            {
                Notifications.Insert(0, notification);
            });
        }

        public void NotifyUserInfo(String source, String msg)
        {
            NotifyUserInfo(Notification.CreateInfo(source, msg));
        }

        public void NotifyUserWarning(String source, String msg)
        {
            NotifyUserInfo(Notification.CreateWarning(source, msg));
        }

        public void NotifyUserError(String source, String msg)
        {
            NotifyUserInfo(Notification.CreateError(source, msg));
        }

        public void Clear()
        {
            Notifications.Clear();
        }
    }
}
