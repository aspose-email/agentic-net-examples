using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

namespace MboxSplitExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Top-level exception guard
            try
            {
                // Input MBOX file path
                string mboxPath = "storage.mbox";
                // Output folder for split parts
                string outputFolder = "SplitParts";
                // Approximate size of each split part (e.g., 5 MB)
                long chunkSize = 5 * 1024 * 1024;

                // Ensure the input MBOX file exists; create an empty placeholder if missing
                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"Input file '{mboxPath}' not found. Creating an empty placeholder.");
                    try
                    {
                        using (FileStream placeholder = File.Create(mboxPath)) { }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                        return;
                    }
                }

                // Ensure the output directory exists
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
                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    // Register a console key press (e.g., 'c') to cancel the operation
                    Task.Run(() =>
                    {
                        Console.WriteLine("Press 'c' to cancel the splitting process...");
                        while (true)
                        {
                            if (Console.ReadKey(true).KeyChar == 'c')
                            {
                                cts.Cancel();
                                break;
                            }
                        }
                    });

                    // Create the MBOX storage reader
                    using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                    {
                        // Optional: subscribe to events for progress (no access to message content)
                        mboxReader.EmlCopying += (sender, e) =>
                        {
                            // e does not expose a Message property; we can only log that a copy is starting
                            Console.WriteLine("Copying a message to a split part...");
                        };

                        mboxReader.EmlCopied += (sender, e) =>
                        {
                            Console.WriteLine("Message copied successfully.");
                        };

                        // Perform the split asynchronously with cancellation support
                        Task splitTask = mboxReader.SplitIntoAsync(chunkSize, outputFolder, cts.Token);

                        try
                        {
                            splitTask.GetAwaiter().GetResult();
                            Console.WriteLine("MBOX splitting completed.");
                        }
                        catch (OperationCanceledException)
                        {
                            Console.WriteLine("MBOX splitting was cancelled by the user.");
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
}