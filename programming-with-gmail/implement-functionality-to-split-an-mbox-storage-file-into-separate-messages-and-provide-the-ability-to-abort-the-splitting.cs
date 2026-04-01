using System;
using System.IO;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input MBOX file path
            string mboxPath = "input.mbox";
            // Output directory for split messages
            string outputDir = "output";

            // Guard input file existence
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Cancellation token source to allow aborting
            CancellationTokenSource cts = new CancellationTokenSource();

            // Create the MBOX reader using the required factory method
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                int messageIndex = 0;

                while (true)
                {
                    // Check for cancellation request
                    if (cts.Token.IsCancellationRequested)
                    {
                        Console.WriteLine("Splitting operation aborted by user.");
                        break;
                    }

                    // Read the next message from the MBOX storage
                    MailMessage message = reader.ReadNextMessage();
                    if (message == null)
                    {
                        // No more messages
                        break;
                    }

                    using (message)
                    {
                        messageIndex++;
                        string outputPath = Path.Combine(outputDir, $"Message_{messageIndex}.eml");
                        message.Save(outputPath);
                        Console.WriteLine($"Saved: {outputPath}");
                    }

                    // Simple abort trigger: press any key to stop
                    if (Console.KeyAvailable)
                    {
                        Console.ReadKey(true); // consume the key
                        cts.Cancel();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
