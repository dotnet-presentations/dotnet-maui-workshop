using System;
using Adventures.Common.Interfaces;

namespace Adventures.Common.Commands
{
	public class CommandBase : IMvpCommand
    {
        public event EventHandler CanExecuteChanged;

        public EventArgs EventArgs { get; set; }
        public string MatchButtonText { get; set; }
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

        public virtual void OnExecute()
        {
        }
    }
}

