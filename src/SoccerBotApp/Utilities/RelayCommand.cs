using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SoccerBotApp.Utilities
{
    public class RelayCommand : ICommand
    {
        Action<object> _action;
        Action _voidReturnAction;
        Func<Task> _asyncAction;

        public event EventHandler CanExecuteChanged;

        private RelayCommand()
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
            if (_voidReturnAction != null)
            {
                if (parameter != null)
                    throw new Exception("Wrong action type mapping, parameter passed to null command handler.");

                _voidReturnAction();
            }
            else if (_action != null)
            {
                _action(parameter);
            }
            else if (_asyncAction != null)
            {
                _asyncAction.Invoke();
            }
            else
            {
                throw new Exception("Missing Action.");
            }
        }

        public static RelayCommand Create(Action<object> action)
        {
            return new RelayCommand() { _action = action };
        }

        public static RelayCommand Create(Action action)
        {
            return new RelayCommand() { _voidReturnAction = action };
        }

        public static RelayCommand CreateAsync(Func<Task> asyncAction)
        {
            return new RelayCommand() { _asyncAction = asyncAction };
        }

    }
}
