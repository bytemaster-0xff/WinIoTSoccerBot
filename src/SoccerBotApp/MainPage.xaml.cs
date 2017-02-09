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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SoccerBotApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
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
