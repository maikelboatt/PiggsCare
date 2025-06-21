using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Exceptions;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using PiggsCare.Core.Control;
using PiggsCare.Core.Factory;
using PiggsCare.Core.Stores;
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
            IConfigurationRoot configuration = new ConfigurationBuilder()
                                               .SetBasePath(AppContext.BaseDirectory)
                                               .AddJsonFile("appsettings.json", false, true)
                                               .Build();

            // Register the configuration file
            Mvx.IoCProvider?.RegisterSingleton(configuration);

            // Register ISqlDataAccess with the connection string
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ISqlDataAccess>(() =>
            {
                ILogger<SqlDataAccess> logger = Mvx.IoCProvider.Resolve<ILogger<SqlDataAccess>>();
                return new SqlDataAccess(configuration.GetConnectionString("DefaultConnection"), logger);
            });

            Assembly[] assembliesToScan =
            [
                GetType().Assembly,              // Current assembly (PiggsCare.Core)
                typeof(IAnimalService).Assembly, // Assembly containing the interfaces
                // typeof(AnimalService).Assembly,  // Assembly containing the implementations
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

                // Register Stores
                CreatableTypes(assembly)
                    .EndingWith("Store")
                    .AsInterfaces()
                    .RegisterAsLazySingleton();

                // Register Validation Classes
                CreatableTypes(assembly)
                    .EndingWith("Validation")
                    .AsInterfaces()
                    .RegisterAsLazySingleton();

                // Register Repositories
                CreatableTypes(assembly)
                    .EndingWith("Repository")
                    .AsInterfaces()
                    .RegisterAsLazySingleton();

                // Register Viewmodels
                CreatableTypes(assembly)
                    .EndingWith("ViewModel")
                    .AsTypes()
                    .RegisterAsDynamic();
            }


            // Register ModalNavigationStore
            Mvx.IoCProvider?.RegisterSingleton(new ModalNavigationStore());

            // Register ModalNavigationControl
            Mvx.IoCProvider?.RegisterType<IModalNavigationControl, ModalNavigationControl>();


            // Register ViewModelFactory
            // Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IViewModelFactory, ViewModelFactory>();
            Mvx.IoCProvider?.RegisterSingleton<IViewModelFactory>(new ViewModelFactory());

            // Register the viewmodel factory
            Mvx.IoCProvider?.RegisterSingleton<Func<Type, object, MvxViewModel>>(() => ( viewModelType, parameter ) =>
            {
                object? resolvedViewModel = Mvx.IoCProvider.Resolve(viewModelType);

                if (resolvedViewModel == null)
                {
                    // Handle the case where the ViewModel is not registered.
                    // This is crucial for preventing runtime exceptions.
                    string errorMessage = $"Failed to resolve ViewModel of type: {viewModelType.FullName}";
                    //Options for handling the error
                    //1. Throw an exception:
                    throw new MvxIoCResolveException(errorMessage);
                    //2. Log the error and return null (less recommended, as it might lead to other null reference exceptions later):
                    //Mvx.IoCProvider.Log.Error(errorMessage);
                    //return null;
                }

                MvxViewModel viewModel = (MvxViewModel)resolvedViewModel; // Safe cast now

                //Call the correct prepare statement based on the parameter type
                {
                    MethodInfo? prepareMethod = viewModelType.GetMethod("Prepare", new[] { parameter.GetType() });
                    prepareMethod?.Invoke(viewModel, new[] { parameter });
                }

                viewModel.Initialize();
                return viewModel;
            });


            RegisterAppStart<ShellViewModel>();
        }
    }
}
