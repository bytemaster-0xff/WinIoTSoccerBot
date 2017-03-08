using LagoVista.Core.UWP.Services;
using SoccerBot.Core.Channels;
using SoccerBot.Core.Devices;
using SoccerBot.Core.Interfaces;
using SoccerBot.UWP.Channels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SoccerBot.mBot
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        ISoccerBotLogger _logger;
        ISoccerBot _soccerBot;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                UWPDeviceServices.Init(rootFrame.Dispatcher);

                InitSoccerBot();


                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        private async void InitSoccerBot()
        {
            var hostNames = NetworkInformation.GetHostNames();
            var computerName = hostNames.FirstOrDefault(name => name.Type == HostNameType.DomainName)?.DisplayName ?? "???";

            _logger = new Loggers.DebugLogger();

            switch (AnalyticsInfo.VersionInfo.DeviceFamily)
            {
                case "Windows.IoT":
                    var ports = (await LagoVista.Core.PlatformSupport.Services.DeviceManager.GetSerialPortsAsync());
                    if (ports.Count == 0)
                    {
                        throw new Exception("Could not find any serial ports, a serial port is required.");
                    }
                    else if (ports.Count > 1)
                    {
                        throw new Exception("Found more than one serial port, please add additional logic to select the serial port the mBot is connected to.");
                    }

                    var serialPortChannel = new SerialChannel(ports.First().Id, _logger);
                    await serialPortChannel.ConnectAsync();
                    _soccerBot = new mBlockSoccerBot(serialPortChannel, _logger);                    

              //      Managers.ConnectionManager.Instance.MakeDiscoverable(computerName);
                    break;
                case "Windows.Desktop":
                    _soccerBot = new SimulatedSoccerBot();

                    break;
            }

            Managers.ConnectionManager.Instance.Init(_soccerBot, _logger);
            Managers.ConnectionManager.Instance.StartWebServer(80, computerName);
            Managers.ConnectionManager.Instance.StartTCPServer(9000);
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
