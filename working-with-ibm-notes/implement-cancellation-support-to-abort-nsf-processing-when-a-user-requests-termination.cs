using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static async Task Main(string[] args)
    {
        // Top‑level exception guard
        try
        {
            string nsfPath = "sample.nsf";

            // Guard file I/O
            if (!File.Exists(nsfPath))
            {
                Console.Error.WriteLine($"NSF file not found: {nsfPath}");
                return;
            }

            // Cancellation token source for user‑initiated abort
            using CancellationTokenSource cts = new CancellationTokenSource();

            // Task that monitors console input for a cancel command
            Task monitorTask = Task.Run(() =>
            {
                Console.WriteLine("Press 'c' to cancel NSF processing...");
                while (!cts.IsCancellationRequested)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(intercept: true);
                        if (key.KeyChar == 'c' || key.KeyChar == 'C')
                        {
                            cts.Cancel();
                            break;
                        }
                    }
                    Thread.Sleep(100);
                }
            });

            // Open the NSF storage with cancellation support
            using (NotesStorageFacility nsf = new NotesStorageFacility(nsfPath, cts.Token))
            {
                // Enumerate messages; abort if cancellation is requested
                foreach (MailMessage message in nsf.EnumerateMessages())
                {
                    if (cts.IsCancellationRequested)
                    {
                        Console.WriteLine("Processing cancelled by user.");
                        break;
                    }

                    // Ensure each message is disposed after use
                    using (message)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        // Additional processing can be placed here
                    }
                }
            }

            // Wait for the monitor task to finish cleanly
            await monitorTask;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
