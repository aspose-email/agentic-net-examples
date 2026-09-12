using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            const string emlPath = "sample.eml";
            const string msgPath = "output.msg";
            const string attachmentPath = "sample.txt";

            // Ensure placeholder attachment exists
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Sample attachment content.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create attachment file: {ex.Message}");
                    return;
                }
            }

            // Ensure input EML exists; create minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    using (var placeholder = new MailMessage())
                    {
                        placeholder.From = "sender@example.com";
                        placeholder.To = "receiver@example.com";
                        placeholder.Subject = "Placeholder EML";
                        placeholder.Body = "This is a placeholder EML message.";
                        placeholder.Save(emlPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
                    return;
                }
            }

            // Load EML with options preserving TNEF and embedded messages
            var emlLoadOptions = new EmlLoadOptions
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            try
            {
                using (MailMessage message = MailMessage.Load(emlPath, emlLoadOptions))
                {
                    // Add attachment from file system
                    var attachment = new Attachment(attachmentPath);
                    message.Attachments.Add(attachment);

                    // Save as MSG using default MSG save options
                    message.Save(msgPath, SaveOptions.DefaultMsg);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing EML file: {ex.Message}");
                return;
            }

            Console.WriteLine($"Conversion completed. MSG saved to '{msgPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
