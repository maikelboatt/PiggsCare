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

            Assembly[] assembliesToScan =
            [
                GetType().Assembly,              // Current assembly (your main application)
                typeof(IAnimalService).Assembly, // Assembly containing the interfaces
                typeof(AnimalService).Assembly,  // Assembly containing the implementations
                typeof(IAnimalRepository).Assembly,
                typeof(AnimalRepository).Assembly
            ];

            foreach (Assembly assembly in assembliesToScan)
            {
                // Register Services
                CreatableTypes(assembly)
                    .EndingWith("Service")
                    .AsInterfaces()
                    .RegisterAsLazySingleton();

                // Register Repositories
                CreatableTypes(assembly)
                    .EndingWith("Repository")
                    .AsInterfaces()
                    .RegisterAsLazySingleton();

                // Register Stores
                CreatableTypes(assembly)
                    .EndingWith("Stores")
                    .AsInterfaces()
                    .RegisterAsLazySingleton();
            }

            // Register ISqlDataAccess
            Mvx.IoCProvider?.RegisterSingleton<ISqlDataAccess>(new TestDataAccess());


            RegisterAppStart<ShellViewModel>();
        }
    }
}
