using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PiggsCare.Core.Stores;
using System.Windows.Input;

namespace PiggsCare.Core.ViewModels
{
    public class TestViewModel( ModalNavigationStore modalNavigationStore ):MvxViewModel<int>, ITestViewModel
    {
        private int _parameter;

        public ICommand CloseCommand => new MvxCommand(CloseExecute);


        public override void Prepare( int parameter )
        {
            _parameter = parameter;
            Console.WriteLine(_parameter);
        }

        private void CloseExecute()
        {
            modalNavigationStore.Close();
        }
    }
}
