using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;

namespace SoccerBotApp.Devices
{
    public class SoccerBotBase
    {     
        public String Id { get; set; }
        public String Name { get; set; }
        public String DeviceName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected async void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            //TODO: Should be a design time check and not run this.
            if (App.TheApp != null && App.TheApp.Dispatcher != null)
            {
                await App.TheApp.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                });
            }
        }

       
    }
}
