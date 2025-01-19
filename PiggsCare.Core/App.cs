using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using PiggsCare.Core.ViewModels;
using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.Repositories;
using PiggsCare.Domain.Repositories;
using PiggsCare.Domain.Services;
using System.Reflection;

namespace PiggsCare.Core
{
    public class App:MvxApplication
    {
        public override void Initialize()
        {



            RegisterAppStart<ShellViewModel>();
        }
    }
}
