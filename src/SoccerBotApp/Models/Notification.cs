using System;

namespace SoccerBotApp.Models
{
    public class Notification
    {
        public Notification(Levels level, String source, String message)
        {
            DateStamp = DateTime.Now;
            Message = message;
            Source = source;
            Level = level;
        }

        public enum Levels
        {
            Info,
            Warning,
            Error
        }

        public Levels Level { get; private set; }
        public DateTime DateStamp { get; private set; }
        public String Source { get; private set; }
        public String Message { get; private set; }

        public static Notification CreateInfo(String source, String message)
        {
            return new Notification(Levels.Info, source, message);
        }

        public static Notification CreateWarning(String source, String message)
        {
            return new Notification(Levels.Info, source, message);
        }

        public static Notification CreateError(String source, String message)
        {
            return new Notification(Levels.Info, source, message);
        }
    }
}
