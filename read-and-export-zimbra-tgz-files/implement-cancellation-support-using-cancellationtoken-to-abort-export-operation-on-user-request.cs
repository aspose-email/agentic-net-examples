using Aspose.Email;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Storage.Zimbra;

namespace AsposeEmailExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Input TGZ file path
                string inputFilePath = "sample.tgz";

                // Verify input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Output directory for extracted messages
                string outputDirectory = "ExportedMessages";

                // Ensure output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Set up cancellation support
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                // Start a background task to listen for user cancellation (press 'c')
                Task.Run(() =>
                {
                    Console.WriteLine("Press 'c' to cancel the export operation...");
                    while (true)
                    {
                        if (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                            if (keyInfo.KeyChar == 'c' || keyInfo.KeyChar == 'C')
                            {
                                cancellationTokenSource.Cancel();
                                break;
                            }
                        }
                        Thread.Sleep(200);
                    }
                });

                // Perform the export with cancellation token
                using (TgzReader tgzReader = new TgzReader(inputFilePath))
                {
                    await tgzReader.ExportToAsync(outputDirectory, cancellationTokenSource.Token);
                }

                Console.WriteLine("Export completed successfully.");
            }
            catch (OperationCanceledException)
            {
                Console.Error.WriteLine("Export operation was canceled by the user.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
