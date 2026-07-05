using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

namespace MboxAsyncCancellationExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                const string mboxPath = "storage.mbox";

                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                    return;
                }

                // Prepare output directory
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
                Directory.CreateDirectory(outputDir);

                using var cts = new CancellationTokenSource();

                // Listen for user cancellation
                Task cancelTask = Task.Run(() =>
                {
                    Console.WriteLine("Press 'c' to cancel the operation...");
                    while (Console.ReadKey(true).KeyChar != 'c')
                    {
                        // ignore other keys
                    }
                    cts.Cancel();
                });

                // Read MBOX messages in a background task to keep Main responsive
                await Task.Run(() =>
                {
                    using var mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());

                    while (true)
                    {
                        if (cts.Token.IsCancellationRequested)
                        {
                            Console.WriteLine("Operation cancelled by user.");
                            break;
                        }

                        // Read the next message; returns null when end of file is reached
                        MailMessage message = mboxReader.ReadNextMessage();
                        if (message == null)
                            break;

                        using (message)
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"To: {message.To}");

                            string safeSubject = MakeSafeFileName(message.Subject);
                            string outputFile = Path.Combine(outputDir, $"{safeSubject}.eml");

                            // Save the message
                            message.Save(outputFile);
                            Console.WriteLine($"Saved: {outputFile}");
                        }
                    }
                }, cts.Token);

                // Ensure the cancel listener task ends
                cts.Cancel();
                await cancelTask;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        private static string MakeSafeFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "Untitled";

            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(invalidChar, '_');
            }
            return name;
        }
    }
}
