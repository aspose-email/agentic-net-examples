using Aspose.Email;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Storage.Mbox;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Input MBOX file path
            string mboxPath = "input.mbox";
            // Output folder for split parts
            string outputFolder = "SplitParts";
            // Approximate size of each chunk (e.g., 5 MB)
            long chunkSize = 5 * 1024 * 1024;

            // Create a cancellation token source that can be triggered by Ctrl+C
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                Console.CancelKeyPress += (sender, e) =>
                {
                    Console.Error.WriteLine("Cancellation requested...");
                    cts.Cancel();
                    e.Cancel = true;
                };

                await SplitMboxAsync(mboxPath, outputFolder, chunkSize, cts.Token);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static async Task SplitMboxAsync(string mboxPath, string outputFolder, long chunkSize, CancellationToken token)
    {
        // Guard input file existence
        if (!File.Exists(mboxPath))
        {
            try
            {
                // Create a minimal placeholder MBOX file
                using (FileStream placeholder = File.Create(mboxPath))
                {
                    // Write a simple From line to make it a valid MBOX
                    byte[] header = System.Text.Encoding.UTF8.GetBytes("From placeholder@example.com Sat Jan 01 00:00:00 2022\r\n\r\n");
                    placeholder.Write(header, 0, header.Length);
                }
                Console.WriteLine($"Placeholder MBOX created at '{mboxPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create placeholder MBOX: {ex.Message}");
                return;
            }
        }

        // Ensure output directory exists
        try
        {
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
            return;
        }

        // Perform splitting
        try
        {
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Optionally read first message to demonstrate ReadNextMessage usage
                MailMessage firstMessage = reader.ReadNextMessage();
                if (firstMessage != null)
                {
                    Console.WriteLine($"First message subject: {firstMessage.Subject}");
                }

                // Split asynchronously
                await reader.SplitIntoAsync(chunkSize, outputFolder, token);
                Console.WriteLine("MBOX splitting completed.");
            }
        }
        catch (OperationCanceledException)
        {
            Console.Error.WriteLine("MBOX splitting was canceled.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during MBOX splitting: {ex.Message}");
        }
    }
}
