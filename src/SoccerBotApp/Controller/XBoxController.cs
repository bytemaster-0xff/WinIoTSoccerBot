using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Gaming.Input;
using Windows.UI.Core;

namespace SoccerBotApp.Controller
{
    public class XBoxController
    {
        private Gamepad _gamePad = null;
        private GamepadReading? _lastReading = null;

        public event EventHandler<Point> JoyStickUpdated;

        Point? _lastJoyStick;


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

                        var thisJoyStick = new Point(reading.LeftThumbstickX, reading.LeftThumbstickY);

                        if (_lastJoyStick.HasValue && (_lastJoyStick.Value.X != thisJoyStick.X || _lastJoyStick.Value.Y != thisJoyStick.Y))
                        {
                            JoyStickUpdated?.Invoke(_gamePad, thisJoyStick);
                        }

                        _lastJoyStick = thisJoyStick;
                        
                        _lastReading = reading;
                    }
                });

                await Task.Delay(TimeSpan.FromMilliseconds(250));
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
