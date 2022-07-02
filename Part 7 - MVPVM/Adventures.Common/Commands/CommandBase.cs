#pragma warning disable CA1416

namespace Adventures.Common.Commands
{
    public partial class CommandBase : IMvpCommand
    {
        private string _matchButtonText;

        public event EventHandler CanExecuteChanged;

        public bool IsNotBusy
        {
            get
            {
                var buttonArgs = EventArgs as ButtonEventArgs;
                if (buttonArgs == null
                        || buttonArgs.ViewModel == null
                        || buttonArgs.ViewModel is not IListViewModel)
                    return false;

                return ((IListViewModel)buttonArgs.ViewModel).IsBusy;
            }
        }

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

