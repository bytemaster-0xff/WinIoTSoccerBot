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
using SoccerBot.UWP.Channels;
using SoccerBot.Core.Devices;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SoccerBotApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        SoccerBotAppLogger _logger;

        Controller.XBoxController _controller;

        public MainPage()
        {
            _logger = new SoccerBotAppLogger();
            _controller = new Controller.XBoxController();

            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        public MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel(_logger);
            ViewModel.RegisterChannelWatcher(new SoccerBot.UWP.Watchers.BluetoothChannelWatcher(ViewModel.Logger));
            ViewModel.RegisterChannelWatcher(new SoccerBot.UWP.Watchers.UPNPChannelWatcher(ViewModel.Logger));
            ViewModel.ChannelConnected += ViewModel_ChannelConnected;

            _controller.Init();
            _controller.StartListening(Dispatcher);
            _controller.JoyStickUpdated += _controller_JoyStickUpdated;
        }

        private void _controller_JoyStickUpdated(object sender, Point e)
        {
            var angle = Math.Abs(Convert.ToInt16((Math.Atan2(e.Y, e.X) * 180 / Math.PI) - 90)); /* -90 to get heading of zero being north */
            var speed = Convert.ToInt16(Math.Sqrt(e.Y * e.Y + e.X * e.X) * 100);
            Debug.WriteLine("ANGLE => " + angle);

            if (ViewModel.ActiveRemoteDevice != null)
            {                
                ViewModel.ActiveRemoteDevice.Move(speed,angle);
            }
        }

        private void ViewModel_ChannelConnected(object sender, SoccerBot.Core.Interfaces.IChannel e)
        {
            if (e is BluetoothChannel)
            {
                ViewModel.AvailableChannels.Add(e);
            }
            else if(e is TCPIPChannel)
            {
                ViewModel.ActiveRemoteDevice = new SoccerBotClient(e, _logger, "9999");
            }
        }

        

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DeviceView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Debug.WriteLine(args.NewValue);
        }
    }
}
