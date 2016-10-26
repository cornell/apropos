using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.File;

namespace Apropos.Console
{
    public static class SerilogLogger
    {
        public static void Trace1()
        {
            // Create Logger
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger();

            // Prepare data
            var order = new { Id = 123, CustomerId = "JG", Total = 345.50 };
            var customer = new Customer { Id = "JG", Name = "Johnny Graber" };

            // Create log message
            Log.Information("Processed order {orderId} by {customer}",
                             order.Id, customer);
        }

        public static void Trace2()
        {
            // Create Logger
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .WriteTo.Sink(
                        new FileSink("mylogs.txt", new JsonFormatter(), null))
                    .CreateLogger();

            // Prepare data
            var order = new { Id = 123, CustomerId = "JG", Total = 345.50 };
            var customer = new Customer { Id = "JG", Name = "Johnny Graber" };

            // Create log message
            Log.Information("Processed order {orderId} by {customer}",
                             order.Id, customer);
        }
        public class Customer
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }

}
