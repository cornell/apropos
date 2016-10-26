using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Apropos.Domain
{
    public class AsyncFileWriter
    {
        public async void ProcessWrite()
        {
            string filePath = @"temp2.txt";
            string text = "Hello World\r\n";

            await WriteTextAsync(filePath, text);
        }

        private async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }

        // https://msdn.microsoft.com/en-us/library/mt674879.aspx
        public async void ProcessWriteMult()
        {
            string folder = @"tempfolder\";
            List<Task> tasks = new List<Task>();
            List<FileStream> sourceStreams = new List<FileStream>();

            try
            {
                for (int index = 1; index <= 10; index++)
                {
                    string text = "In file " + index.ToString() + "\r\n";

                    string fileName = "file" + index.ToString("00") + ".txt";
                    string filePath = folder + fileName;

                    byte[] encodedText = Encoding.Unicode.GetBytes(text);

                    FileStream sourceStream = new FileStream(filePath,
                        FileMode.Append, FileAccess.Write, FileShare.None,
                        bufferSize: 4096, useAsync: true);

                    Task theTask = sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                    sourceStreams.Add(sourceStream);

                    tasks.Add(theTask);
                }

                await Task.WhenAll(tasks);
            }

            finally
            {
                foreach (FileStream sourceStream in sourceStreams)
                {
                    sourceStream.Flush(false);
                }
            }
        }
    }
}