using System;
using System.IO;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths for input MBOX and output PST
            string inputMboxPath = "input.mbox";
            string outputPstPath = "output.pst";

            // Verify input file exists
            if (!File.Exists(inputMboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputMboxPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPstPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Create a cancellation token source (could be triggered by external logic)
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            // Example: cancel if a specific command‑line argument is present
            if (args.Length > 0 && args[0].Equals("cancel", StringComparison.OrdinalIgnoreCase))
            {
                cancellationTokenSource.Cancel();
            }

            try
            {
                // Check for cancellation before starting the conversion
                if (cancellationTokenSource.Token.IsCancellationRequested)
                {
                    throw new OperationCanceledException(cancellationTokenSource.Token);
                }

                // Perform the conversion from MBOX to PST
                // The synchronous API does not accept a token, so we honor cancellation manually
                PersonalStorage pst = MailStorageConverter.MboxToPst(inputMboxPath, outputPstPath);

                // Dispose the PST after use
                using (pst)
                {
                    Console.WriteLine("Conversion completed successfully.");
                }
            }
            catch (OperationCanceledException)
            {
                // Clean up any partially created PST file
                try
                {
                    if (File.Exists(outputPstPath))
                    {
                        File.Delete(outputPstPath);
                        Console.WriteLine("Partial output file deleted due to cancellation.");
                    }
                }
                catch (Exception cleanupEx)
                {
                    Console.Error.WriteLine($"Failed to delete partial file: {cleanupEx.Message}");
                }

                Console.WriteLine("Operation was canceled.");
                return;
            }
            catch (Exception ex)
            {
                // General error handling for conversion failures
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                // Attempt to clean up partial output as well
                try
                {
                    if (File.Exists(outputPstPath))
                    {
                        File.Delete(outputPstPath);
                    }
                }
                catch
                {
                    // Suppress any cleanup exceptions
                }

                return;
            }
        }
        catch (Exception outerEx)
        {
            Console.Error.WriteLine($"Unexpected error: {outerEx.Message}");
        }
    }
}
