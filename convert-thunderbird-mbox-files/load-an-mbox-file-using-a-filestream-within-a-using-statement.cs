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
            string mboxPath = "sample.mbox";

            // Ensure the MBOX file exists; create a minimal placeholder if it does not.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    // Minimal MBOX content with a single message.
                    string placeholder = "From - Mon Jan 01 00:00:00 2020\r\nSubject: Placeholder\r\n\r\nThis is a placeholder message.\r\n";
                    File.WriteAllText(mboxPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Open the MBOX file within a FileStream.
            try
            {
                using (FileStream fileStream = File.OpenRead(mboxPath))
                {
                    // Create the MboxStorageReader with load options.
                    using (MboxStorageReader reader = MboxStorageReader.CreateReader(fileStream, new MboxLoadOptions()))
                    {
                        // Read messages sequentially.
                        MailMessage message = reader.ReadNextMessage();
                        while (message != null)
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                            // Dispose the message after use.
                            message.Dispose();
                            message = reader.ReadNextMessage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MBOX file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
