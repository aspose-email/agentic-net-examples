using System;
using System.IO;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "sample.mbox";

            // Ensure the MBOX file exists; create an empty placeholder if missing.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            const int maxRetries = 3;
            int attempt = 0;
            bool readSuccessful = false;

            while (attempt < maxRetries && !readSuccessful)
            {
                attempt++;
                try
                {
                    // Create the reader with default load options.
                    using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                    {
                        MailMessage message;
                        // Read messages sequentially.
                        while ((message = reader.ReadNextMessage()) != null)
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                        }
                    }

                    readSuccessful = true;
                }
                catch (IOException ioEx)
                {
                    // Likely a file lock; log and retry after a short delay.
                    Console.Error.WriteLine($"Attempt {attempt}: Unable to read MBOX file - {ioEx.Message}");
                    if (attempt >= maxRetries)
                    {
                        Console.Error.WriteLine("Maximum retry attempts reached. Exiting.");
                        return;
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    // Any other unexpected error aborts the operation.
                    Console.Error.WriteLine($"Unexpected error on attempt {attempt}: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level guard for any unforeseen exceptions.
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
