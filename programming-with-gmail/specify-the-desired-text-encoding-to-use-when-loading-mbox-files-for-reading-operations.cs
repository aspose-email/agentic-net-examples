using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MBOX file
            string mboxPath = "sample.mbox";

            // Verify that the file exists before attempting to read it
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Error: File not found – {mboxPath}");
                return;
            }

            // Configure load options with the desired text encoding
            MboxLoadOptions loadOptions = new MboxLoadOptions
            {
                PreferredTextEncoding = Encoding.UTF8
            };

            // Create the MBOX reader using the factory method
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                while (true)
                {
                    // Read the next message; returns null when no more messages are available
                    MailMessage message = mboxReader.ReadNextMessage();
                    if (message == null)
                        break;

                    // Ensure the message is disposed after use
                    using (message)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {message.To}");
                        Console.WriteLine();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Gracefully handle any unexpected errors
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
