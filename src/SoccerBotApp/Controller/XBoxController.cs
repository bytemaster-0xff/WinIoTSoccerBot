using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using Windows.UI.Core;

namespace SoccerBotApp.Controller
{
    public class XBoxController
    {
        private Gamepad _gamePad = null;
        private GamepadReading? _lastReading = null;

        public event EventHandler ButtonPressed;

        public event EventHandler RightTriggerPressed;
        public event EventHandler LeftTriggerPressed;



        public void Init()
        {
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;
        }

        public async void StartListening(CoreDispatcher dispatcher)
        {
            while (true)
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (_gamePad != null)
                    {

                        var reading = _gamePad.GetCurrentReading();
                        

                   
                        _lastReading = reading;

                        System.Diagnostics.Debug.WriteLine(reading.LeftTrigger.ToString());
                    }
                });

                await Task.Delay(TimeSpan.FromMilliseconds(20));
            }
        }

        private void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            _gamePad = null;
        }

        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            _gamePad = e;
        }


    }
}
