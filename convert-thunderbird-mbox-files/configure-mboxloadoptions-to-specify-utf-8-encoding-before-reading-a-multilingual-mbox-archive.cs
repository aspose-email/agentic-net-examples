using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "multilingual.mbox";

            // Verify that the MBOX file exists before attempting to read it.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Configure load options to use UTF‑8 encoding.
            MboxLoadOptions loadOptions = new MboxLoadOptions
            {
                PreferredTextEncoding = Encoding.UTF8
            };

            // Create the reader using the factory method as required.
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                // Read messages one by one.
                MailMessage message = reader.ReadNextMessage();
                while (message != null)
                {
                    try
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {message.To}");
                        Console.WriteLine();
                    }
                    finally
                    {
                        // Ensure each MailMessage is disposed after use.
                        message.Dispose();
                    }

                    // Read the next message.
                    message = reader.ReadNextMessage();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
