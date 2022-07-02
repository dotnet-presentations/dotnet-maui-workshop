#pragma warning disable CA1416

namespace Adventures.Common.Commands
{
    public partial class CommandBase : IMvpCommand
    {
        public string Name { get; set; }

        public const string GLOBAL = "GLOBAL";

        private string _matchButtonText;
        private string _message;

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

        public string MatchDataType { get; set; }

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

        public string Message {
            get
            {
                if (_message == null)
                    _message = MatchButtonText;
                return _message;
            }
            set { _message = value; }
        }

        public CommandBase() {
            Name = this.GetType().Name;
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            EventArgs = parameter as EventArgs;
            OnExecute();
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnExecute()
        {
        }
    }
}

