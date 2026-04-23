using Aspose.Email;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "sample.pst";
            const string outputFolder = "SplitParts";
            const long chunkSize = 10 * 1024 * 1024; // 10 MB

            // Ensure input PST exists; create a minimal placeholder if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Ensure output folder exists.
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output folder: {ex.Message}");
                return;
            }

            // Prepare cancellation token source.
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                // Listen for user abort (press 'c' to cancel).
                Task.Run(() =>
                {
                    Console.WriteLine("Press 'c' to cancel the split operation...");
                    while (true)
                    {
                        if (Console.ReadKey(true).KeyChar == 'c')
                        {
                            cts.Cancel();
                            break;
                        }
                    }
                });

                try
                {
                    using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                    {
                        // Perform asynchronous split with cancellation support.
                        Task splitTask = pst.SplitIntoAsync(chunkSize, outputFolder, cts.Token);
                        splitTask.Wait();
                        Console.WriteLine("PST split completed successfully.");
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("PST split operation was canceled by the user.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during PST split: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
