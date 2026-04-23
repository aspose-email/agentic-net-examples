using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string pstPath = "input.pst";
            string outputFolder = "output";

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Guard file I/O
            if (!File.Exists(pstPath))
            {
                // Create a minimal placeholder PST file
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode)) { }
                    Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Cancellation token source for monitoring
                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    // Example: cancel after 10 seconds (replace with real condition as needed)
                    Task.Run(async () =>
                    {
                        await Task.Delay(TimeSpan.FromSeconds(10));
                        cts.Cancel();
                    });

                    // Start asynchronous split operation (split by size, e.g., 1 MB)
                    Task splitTask = pst.SplitIntoAsync(1_000_000, outputFolder, cts.Token);

                    // Periodically monitor cancellation token and report progress
                    while (!splitTask.IsCompleted)
                    {
                        if (cts.Token.IsCancellationRequested)
                        {
                            Console.WriteLine("Cancellation requested. Waiting for split operation to stop...");
                            break;
                        }

                        Console.WriteLine("Splitting PST in progress...");
                        await Task.Delay(500);
                    }

                    try
                    {
                        await splitTask;
                        Console.WriteLine("PST split operation completed.");
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("PST split operation was cancelled.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during PST split: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
