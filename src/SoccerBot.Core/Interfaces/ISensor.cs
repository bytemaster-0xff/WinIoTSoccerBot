using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Interfaces
{
    public interface ISensor : INotifyPropertyChanged, IDisposable
    {
        string Value { get; set; }
        DateTime? LastUpdated { get; }
        bool IsOnline { get; }
    }
}
