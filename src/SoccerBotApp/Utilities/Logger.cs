using SoccerBotApp.Models;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using System;

namespace SoccerBotApp.Utilities
{
    public class Logger
    {
        private static Logger _instance = new Logger();

        private Logger()
        {
            Notifications = new ObservableCollection<Notification>();
        }

        public static Logger Instance { get { return _instance; } }

        public ObservableCollection<Notification> Notifications { get; private set; }

        public async void NotifyUserInfo(Notification notification)
        {

            if (App.TheApp != null && App.TheApp.Dispatcher != null)
            {
                if (App.TheApp.Dispatcher.HasThreadAccess)
                {
                    Notifications.Insert(0, notification);
                }
                else
                {
                    await App.TheApp.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                    {
                        Notifications.Insert(0, notification);
                    });
                }
            }
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
    }
}
