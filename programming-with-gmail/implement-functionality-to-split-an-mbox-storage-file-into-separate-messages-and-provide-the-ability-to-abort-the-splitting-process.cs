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
            // Input MBOX file path
            string mboxPath = "input.mbox";
            // Output folder where split parts will be created
            string outputFolder = "SplitParts";
            // Approximate size of each split part (10 MB)
            long chunkSize = 10L * 1024L * 1024L;

            // Verify input file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Create a cancellation token source to allow aborting the split operation
            using (CancellationTokenSource cancellationSource = new CancellationTokenSource())
            {
                // Create the MBOX reader
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    // Start the asynchronous split operation
                    Task splitTask = mboxReader.SplitIntoAsync(chunkSize, outputFolder, cancellationSource.Token);

                    Console.WriteLine("Splitting in progress... Press 'c' to cancel.");

                    // Monitor for user cancellation
                    while (!splitTask.IsCompleted)
                    {
                        if (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                            if (keyInfo.KeyChar == 'c' || keyInfo.KeyChar == 'C')
                            {
                                cancellationSource.Cancel();
                                Console.WriteLine("Cancellation requested.");
                                break;
                            }
                        }
                        await Task.Delay(100);
                    }

                    try
                    {
                        await splitTask;
                        Console.WriteLine("MBOX splitting completed successfully.");
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("MBOX splitting was canceled by the user.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during splitting: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
