using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MBOX file to be verified.
            string mboxPath = "sample.mbox";

            // Ensure the MBOX file exists; create a minimal placeholder if it does not.
            try
            {
                if (!File.Exists(mboxPath))
                {
                    using (StreamWriter writer = new StreamWriter(mboxPath))
                    {
                        // Minimal MBOX message format.
                        writer.WriteLine("From - Mon Jan 01 00:00:00 2020");
                        writer.WriteLine("Subject: Placeholder");
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder message.");
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                return;
            }

            // Open the MBOX storage reader with default load options.
            try
            {
                using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    MailMessage message;
                    // Read each message sequentially to verify integrity.
                    while ((message = reader.ReadNextMessage()) != null)
                    {
                        // Simple verification: output subject and sender.
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine();
                        // Dispose the message after use.
                        message.Dispose();
                    }
                }
            }
            catch (Exception readerEx)
            {
                Console.Error.WriteLine($"MBOX reading error: {readerEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
