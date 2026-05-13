using System;
using System.IO;
using System.Threading;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input MSG file path
            string inputFile = "input.msg";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputFile))
            {
                try
                {
                    var placeholder = new MailMessage("from@example.com", "to@example.com", "Placeholder Subject", "Placeholder body");
                    var saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                    placeholder.Save(inputFile, saveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Output directory for processed files
            string outputDir = "output";
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Cancellation token source for the batch operation
            using (var cts = new CancellationTokenSource())
            {
                // Example: cancel after 5 seconds (adjust as needed)
                cts.CancelAfter(TimeSpan.FromSeconds(5));
                CancellationToken token = cts.Token;

                // Simulate a batch of files (here only the single input file)
                string[] filesToProcess = new[] { inputFile };

                foreach (string filePath in filesToProcess)
                {
                    // Check for cancellation before starting each iteration
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Operation cancelled before processing file: " + Path.GetFileName(filePath));
                        break;
                    }

                    try
                    {
                        // Load the MSG file
                        MailMessage message = MailMessage.Load(filePath);

                        // Example processing: save as EML in the output directory
                        string outputFile = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(filePath) + ".eml");
                        message.Save(outputFile);

                        Console.WriteLine($"Processed '{Path.GetFileName(filePath)}' -> '{Path.GetFileName(outputFile)}'");

                        // Simulate work that respects cancellation
                        token.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Processing was cancelled during file: " + Path.GetFileName(filePath));
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing '{Path.GetFileName(filePath)}': {ex.Message}");
                        // Continue with next file or break based on requirements
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
