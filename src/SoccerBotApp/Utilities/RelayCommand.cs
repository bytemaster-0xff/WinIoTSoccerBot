using System;
using System.Windows.Input;

namespace SoccerBotApp.Utilities
{
    public class RelayCommand : ICommand
    {
        Action<object> _action;
        Action _nullAction;

        public event EventHandler CanExecuteChanged;

        public RelayCommand()
        {
            _enabled = true;
        }

        private async void RaiseCanExecuteChanged()
        {
            if (App.TheApp != null)
            {
                await App.TheApp.RunOnMainThread(() => CanExecuteChanged(this, null));
            }
        }

        private bool _enabled;
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    RaiseCanExecuteChanged();
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return Enabled;
        }

        public void Execute(object parameter)
        {
            if (_nullAction != null)
            {
                if (parameter != null)
                    throw new Exception("Wrong action type mapping, parameter passed to null command handler.");

                _nullAction();
            }

            if (_action != null)
            {
                _action(parameter);
            }
        }

        public static RelayCommand Create(Action<object> action)
        {
            return new RelayCommand() { _action = action };
        }

        public static RelayCommand Create(Action action)
        {
            return new RelayCommand() { _nullAction = action };
        }
    }
}
