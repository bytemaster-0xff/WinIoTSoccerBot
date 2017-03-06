using SoccerBot.Core.ViewModels;
using SoccerBot.UWP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.Gaming.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SoccerBotApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Gamepad _gamePad = null;

        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;

            Gamepad.GamepadAdded += Gamepad_GamepadAdded;


            StartListening();
        }

        private async void StartListening()
        {

            while (true)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (_gamePad == null)
                    {
                        return;
                    }

                    var reading = _gamePad.GetCurrentReading();

                    System.Diagnostics.Debug.WriteLine(reading.LeftTrigger.ToString());

                });


                await Task.Delay(TimeSpan.FromMilliseconds(500));
            }
        }
        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            _gamePad = e;
        }

        public MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel(new SoccerBotAppLogger());
            ViewModel.RegisterChannelWatcher(new SoccerBot.UWP.Watchers.BluetoothChannelWatcher(ViewModel.Logger));
            ViewModel.RegisterChannelWatcher(new SoccerBot.UWP.Watchers.UPNPChannelWatcher(ViewModel.Logger));
        }
    }
}
