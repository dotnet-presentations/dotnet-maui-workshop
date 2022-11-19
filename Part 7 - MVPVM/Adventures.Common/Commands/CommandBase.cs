#pragma warning disable CA1416

namespace Adventures.Common.Commands
{
    public partial class CommandBase : IMvpCommand
    {
        public const string GLOBAL = "GLOBAL";

        private string _message;
        private string _matchButtonText;

        /// <summary>
        /// ICommand interface implementation (not currently used)
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// EventArgs for command use. In the case of button clicks
        /// will hold ButtonEventArgs which provides the following to
        /// the command: Views, Presenter, ViewModel, Sender, and Key
        /// </summary>
        public EventArgs EventArgs { get; set; }

        /// <summary>
        /// Name of the command (set in ctor)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Command handles a DataType, e.g., ListItem versus
        /// a key such as "Get Data"
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Text to use for button, defaults to DataType if null
        /// </summary>
        public string ButtonText
        {
            get {
                if (_matchButtonText == null)
                    _matchButtonText = DataType;
                return _matchButtonText;
            }
            set
            {
                _matchButtonText = value;
            }
        }

        /// <summary>
        /// Command message - defaults to ButtonText if null
        /// </summary>
        public string Message {
            get
            {
                if (_message == null)
                    _message = ButtonText;
                return _message;
            }
            set { _message = value; }
        }

        /// <summary>
        /// Constructor sets the name of the command type
        /// </summary>
        public CommandBase() {
            Name = this.GetType().Name;
        }

        /// <summary>
        /// Invoked by PresenterBase.ButtonClickHandler after it
        /// locates the command to execute.  Sets EventArgs and
        /// invokes OnExecute() for command use
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            EventArgs = parameter as EventArgs;
            OnExecute();
        }

        /// <summary>
        /// Can execute command - defaults to true if not overridden
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// OnCanExecuteChanged()
        /// </summary>
        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Intended to be overridden.  Invoked by Execute,
        /// EventArgs will be set for command use.
        /// </summary>
        public virtual void OnExecute()
        {
        }
    }
}

