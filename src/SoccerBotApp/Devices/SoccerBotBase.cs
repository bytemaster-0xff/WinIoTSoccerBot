﻿using SoccerBotApp.Utilities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;

namespace SoccerBotApp.Devices
{
    public abstract class SoccerBotBase
    {     
        public enum Commands
        {
            Forward,
            Backwards,
            Left,
            Right,
            Stop
        }

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

        public SoccerBotBase()
        {
            ForwardCommand = RelayCommand.Create(SendCommand,Commands.Forward);
            StopCommand = RelayCommand.Create(SendCommand, Commands.Stop);
            BackwardsCommand = RelayCommand.Create(SendCommand, Commands.Backwards);
            LeftCommand = RelayCommand.Create(SendCommand, Commands.Left);
            RightCommand = RelayCommand.Create(SendCommand, Commands.Right);
        }

        protected abstract void SendCommand(Commands cmd);

        public RelayCommand RefreshSensorsCommand { get; private set; }

        public RelayCommand ForwardCommand { get; private set; }
        public RelayCommand StopCommand { get; private set; }
        public RelayCommand BackwardsCommand { get; private set; }

        public RelayCommand LeftCommand { get; private set; }
        public RelayCommand RightCommand { get; private set; }


        private short _speed = 300;
        public short Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                RaisePropertyChanged();
            }
        }
    }
}
