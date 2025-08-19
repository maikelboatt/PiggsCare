// Ignore Spelling: Piggs

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Exceptions;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using PiggsCare.ApplicationState.Stores;
using PiggsCare.ApplicationState.Stores.Animals;
using PiggsCare.Business.Services.Animals;
using PiggsCare.Core.Control;
using PiggsCare.Core.Factory;
using PiggsCare.Core.ViewModels;
using PiggsCare.DataAccess.DatabaseAccess;
using PiggsCare.DataAccess.Repositories.Animals;
using PiggsCare.Infrastructure.Services;
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
                GetType().Assembly, // Current assembly (PiggsCare.Core)
                typeof(IAnimalService).Assembly,
                typeof(IAnimalRepository).Assembly,
                typeof(IAnimalStore).Assembly,
                typeof(IDateConverterService).Assembly
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
            Mvx.IoCProvider?.RegisterSingleton<IViewModelFactory>(new ViewModelFactory());

            // Register the viewmodel factory
            Mvx.IoCProvider?.RegisterSingleton<Func<Type, object, MvxViewModel>>(() => ( viewModelType, parameter ) =>
            {
                // Resolve the ViewModel instance from the IoC container
                MvxViewModel viewModel = (MvxViewModel)(Mvx.IoCProvider.Resolve(viewModelType)
                                                        ?? throw new MvxIoCResolveException($"Failed to resolve ViewModel of type: {viewModelType.FullName}"));

                // Invoke the "Prepare" method on the ViewModel if it exists
                viewModelType.GetMethod("Prepare", new[] { parameter.GetType() })
                             ?.Invoke(viewModel, new[] { parameter });

                // Initialize the ViewModel
                viewModel.Initialize();
                return viewModel;
            });


            RegisterAppStart<ShellViewModel>();
        }
    }
}
