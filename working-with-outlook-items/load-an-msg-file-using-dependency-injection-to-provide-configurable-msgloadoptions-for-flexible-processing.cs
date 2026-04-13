using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            const string msgPath = "sample.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = "sender@example.com";
                        placeholder.To.Add("recipient@example.com");
                        placeholder.Subject = "Placeholder Subject";
                        placeholder.Body = "This is a placeholder message.";
                        // Save as MSG using default options.
                        placeholder.Save(msgPath, SaveOptions.DefaultMsg);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Configure load options (could be injected or configured elsewhere).
            MsgLoadOptions loadOptions = new MsgLoadOptions
            {
                KeepOriginalEmailAddresses = true,
                PreserveRtfContent = true,
                PreserveEmbeddedMessageFormat = true
            };

            // Load the MSG file with the specified options.
            using (MailMessage message = MailMessage.Load(msgPath, loadOptions))
            {
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"From: {message.From}");
                Console.WriteLine($"To: {string.Join(", ", message.To)}");
                Console.WriteLine($"Body: {message.Body}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
