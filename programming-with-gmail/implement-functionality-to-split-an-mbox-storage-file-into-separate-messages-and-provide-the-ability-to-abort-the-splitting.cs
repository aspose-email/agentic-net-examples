using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        // Input MBOX file path
        const string inputMboxPath = "input.mbox";
        // Output directory for extracted messages
        const string outputDirectory = "output";

        // Ensure the input file exists
        if (!File.Exists(inputMboxPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputMboxPath}");
            return;
        }

        // Ensure the output directory exists
        try
        {
            Directory.CreateDirectory(outputDirectory);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
            return;
        }

        // Cancellation token source to allow aborting the split operation
        using var cts = new CancellationTokenSource();

        // Optional: press any key to cancel the operation
        Task.Run(() =>
        {
            Console.WriteLine("Press any key to abort the splitting process...");
            Console.ReadKey(true);
            cts.Cancel();
        });

        try
        {
            // Create the MBOX reader using the required factory method
            using var reader = MboxStorageReader.CreateReader(inputMboxPath, new MboxLoadOptions());

            int messageIndex = 0;
            while (!cts.Token.IsCancellationRequested)
            {
                // Read the next message; returns null when end of file is reached
                MailMessage message = reader.ReadNextMessage();
                if (message == null)
                {
                    break; // No more messages
                }

                // Build a safe file name for the extracted message
                string subject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    subject = subject.Replace(c, '_');
                }

                string outputPath = Path.Combine(outputDirectory, $"{messageIndex:D5}_{subject}.eml");

                try
                {
                    // Save the message as an .eml file
                    message.Save(outputPath);
                    Console.WriteLine($"Saved: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message #{messageIndex}: {ex.Message}");
                }

                messageIndex++;
            }

            if (cts.IsCancellationRequested)
            {
                Console.WriteLine("Splitting operation was aborted by the user.");
            }
            else
            {
                Console.WriteLine("Splitting operation completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred during processing: {ex.Message}");
        }
    }
}
