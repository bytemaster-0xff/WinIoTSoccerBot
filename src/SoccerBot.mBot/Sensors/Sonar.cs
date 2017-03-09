using LagoVista.Core.PlatformSupport;
using SoccerBot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.mBot.Sensors
{
    public class Sonar : ISensor
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            //TODO: Should be a design time check and not run this.
            Services.DispatcherServices.Invoke(() =>
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName))
            );
        }


        private bool _isOnline;

        public bool IsOnline
        {
            get { return _isOnline; }
            set
            {
                _isOnline = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? LastUpdated
        {
            get; private set;
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                LastUpdated = DateTime.Now;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(LastUpdated));
            }
        }


        public void Dispose()
        {

        }

    }
}
