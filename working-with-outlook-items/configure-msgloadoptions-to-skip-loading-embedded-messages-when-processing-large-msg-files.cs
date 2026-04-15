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
            string msgPath = "largeMessage.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body"))
                    {
                        placeholder.Save(msgPath);
                        Console.WriteLine($"Placeholder MSG created at '{msgPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Configure load options to skip embedded messages.
            MsgLoadOptions loadOptions = new MsgLoadOptions
            {
                PreserveEmbeddedMessageFormat = false
            };

            // Load the MSG file with the specified options.
            try
            {
                using (MailMessage message = MailMessage.Load(msgPath, loadOptions))
                {
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.From}");
                    Console.WriteLine($"To: {string.Join(", ", message.To)}");
                    Console.WriteLine($"Body preview: {message.Body?.Substring(0, Math.Min(100, message.Body?.Length ?? 0))}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
