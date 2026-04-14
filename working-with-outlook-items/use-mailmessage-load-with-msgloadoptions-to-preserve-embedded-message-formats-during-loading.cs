using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                try
                {
                    // Create a simple MapiMessage and save as MSG
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder Subject", "Placeholder body"))
                    {
                        placeholder.Save(msgPath);
                    }
                    Console.WriteLine($"Placeholder MSG created at '{msgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Configure load options to preserve embedded message formats
            MsgLoadOptions loadOptions = new MsgLoadOptions
            {
                PreserveEmbeddedMessageFormat = true
            };

            // Load the message with the specified options
            using (MailMessage message = MailMessage.Load(msgPath, loadOptions))
            {
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"From: {message.From}");
                Console.WriteLine($"To: {string.Join(", ", message.To)}");
                // Additional processing can be done here
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
