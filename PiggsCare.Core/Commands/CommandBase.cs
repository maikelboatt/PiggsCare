using System.Windows.Input;

namespace PiggsCare.Core.Commands
{
    public abstract class CommandBase:ICommand
    {
        public virtual bool CanExecute( object? parameter )
        {
            return true;
        }

        public abstract void Execute( object? parameter );

        public event EventHandler? CanExecuteChanged;

        protected void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
