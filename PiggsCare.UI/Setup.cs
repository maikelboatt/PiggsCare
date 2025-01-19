using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Platforms.Wpf.Core;
using Serilog;
using Serilog.Extensions.Logging;

namespace PiggsCare.UI
{
    public class Setup:MvxWpfSetup<Core.App>
    {
        protected override ILoggerProvider? CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory? CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Debug()
                         .CreateLogger();

            return new SerilogLoggerFactory();
        }

        protected override void InitializeFirstChance( IMvxIoCProvider iocProvider )
        {
            base.InitializeFirstChance(iocProvider);
        }
    }
}
