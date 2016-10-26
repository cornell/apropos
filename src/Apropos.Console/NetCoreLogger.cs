using Microsoft.Extensions.Logging;

namespace Apropos.Console
{
    public class NetCoreLogger
    {
        static ILogger Logger { get; } = ApplicationLogging.CreateLogger<Program>();

        public static class ApplicationLogging
        {
            public static ILoggerFactory LoggerFactory { get; } = new LoggerFactory();
            public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
        }

        public class Controller
        {
            ILogger Logger { get; } =
              ApplicationLogging.CreateLogger<Controller>();
            // ...
            public void Initialize()
            {
                using (Logger.BeginScope($"=>{ nameof(Initialize) }"))
                {
                    Logger.LogInformation("Initialize the data");
                    //...
                    Logger.LogInformation("Initialize the UI");
                    //...
                }
            }
        }

        public void Trace()
        {
            ApplicationLogging.LoggerFactory.AddConsole(true);
            Logger.LogInformation("This is a test of the emergency broadcast system.");
            using (Logger.BeginScope(nameof(Trace)))
            {
                //Logger.LogInformation("Begin using controller");
                Controller controller = new Controller();
                controller.Initialize();
                //Logger.LogInformation("End using controller");
            }
            //Logger.Log(LogLevel.Information, 0, "Shutting Down...", null, null);
        }
    }
}