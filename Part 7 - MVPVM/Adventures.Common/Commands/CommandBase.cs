using System;
using Adventures.Common.Interfaces;

namespace Adventures.Common.Commands
{
	public class CommandBase : IMvpCommand
    {
        public event EventHandler CanExecuteChanged;

        public string MatchButtonText { get; set; }
        public string MatchDataType { get; set; }
        public string Message { get; set; }

        public CommandBase()
        {
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            
        }
    }
}

