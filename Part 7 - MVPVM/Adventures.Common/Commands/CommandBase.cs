using System;
using System.ComponentModel;
using Adventures.Common.Interfaces;

namespace Adventures.Common.Commands
{
	public class CommandBase : IMvpCommand
    {
        private string _matchButtonText;

        public event EventHandler CanExecuteChanged;

        public EventArgs EventArgs { get; set; }

        public string MatchButtonText
        {
            get {
                if (_matchButtonText == null)
                    _matchButtonText = MatchDataType;
                return _matchButtonText;
            }
            set
            {
                _matchButtonText = value;
            }
        }
        public string MatchDataType { get; set; }

        public string Message { get; set; }
        
        public CommandBase() { }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            EventArgs = parameter as EventArgs;
            OnExecute();
        }

        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }


        public virtual void OnExecute()
        {
        }
    }
}

