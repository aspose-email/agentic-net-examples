using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string mboxPath = "storage.mbox";
            string outputDirectory = "SplitParts";
            long partSizeInBytes = 10 * 1024 * 1024; // 10 MB per part

            // Verify input file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            using (CancellationTokenSource cancellationSource = new CancellationTokenSource())
            {
                // Listen for a key press to abort the operation
                Task.Run(() =>
                {
                    Console.WriteLine("Press 'c' to cancel the splitting process...");
                    while (true)
                    {
                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.C)
                        {
                            cancellationSource.Cancel();
                            break;
                        }
                        Thread.Sleep(100);
                    }
                });

                // Create the MBOX reader
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    try
                    {
                        // Perform the split operation asynchronously with cancellation support
                        await mboxReader.SplitIntoAsync(partSizeInBytes, outputDirectory, cancellationSource.Token);
                        Console.WriteLine("MBOX splitting completed successfully.");
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("MBOX splitting was cancelled by the user.");
                    }
                    catch (Exception splitEx)
                    {
                        Console.Error.WriteLine($"Error during splitting: {splitEx.Message}");
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
