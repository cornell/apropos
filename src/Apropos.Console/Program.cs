using Serilog;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.FileProviders.Physical;

namespace Apropos.Console
{
    public class Program
    {
        private static PhysicalFileProvider _fileProvider =
            new PhysicalFileProvider(Directory.GetCurrentDirectory() + "\\data");
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Monitoring quotes.txt for changes (ctrl-c to quit)...");
            while (true)
            {
                MainAsync().GetAwaiter().GetResult();
                //Test1().GetAwaiter().GetResult();
            }
        }

        private static async Task MainAsync()
        {
            IChangeToken token = _fileProvider.Watch("*.txt");
            var tcs = new TaskCompletionSource<object>();
            
            token.RegisterChangeCallback(state => ((TaskCompletionSource<object>)state).TrySetResult(null), tcs);
            await tcs.Task.ConfigureAwait(false);
            System.Console.WriteLine("quotes.txt changed");
        }

        private static async Task Test1()
        {
            var tcs = new TaskCompletionSource<object>();

            string path = Directory.GetCurrentDirectory() + "\\data";
            using (var fileSystemWatcher = new FileSystemWatcher())
            {
                using (var physicalFilesWatcher = new PhysicalFilesWatcher(path + Path.DirectorySeparatorChar, fileSystemWatcher, pollForChanges: false))
                {
                    using (var provider = new PhysicalFileProvider(path))
                    {
                        var token = provider.Watch("*.txt");
                        //Assert.NotNull(token);
                        //Assert.False(token.HasChanged);
                        //Assert.True(token.ActiveChangeCallbacks);

                        WaitForChangedResult result = fileSystemWatcher.WaitForChanged(WatcherChangeTypes.All);
                        

                        //fileSystemWatcher.Changed += new FileSystemEventArgs(WatcherChangeTypes.All, path, path));
                        //await Task.Delay(WaitTimeForTokenToFire);

                        //Assert.True(token.HasChanged);
                    }
                }
            }
            await tcs.Task.ConfigureAwait(false);
        }
    }
}
