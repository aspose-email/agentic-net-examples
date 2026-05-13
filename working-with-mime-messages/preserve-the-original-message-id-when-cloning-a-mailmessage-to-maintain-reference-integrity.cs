using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "cloned.eml";

            // Ensure the input file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    using (MailMessage placeholder = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message."))
                    {
                        placeholder.MessageId = "<placeholder-id@example.com>";
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the original message.
            MailMessage originalMessage;
            try
            {
                originalMessage = MailMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            using (originalMessage)
            {
                // Clone the message.
                MailMessage clonedMessage = originalMessage.Clone();

                // Verify that the MessageId is preserved.
                if (originalMessage.MessageId != clonedMessage.MessageId)
                {
                    Console.Error.WriteLine("MessageId was not preserved during cloning.");
                }

                // Save the cloned message.
                try
                {
                    clonedMessage.Save(outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save cloned EML file: {ex.Message}");
                }
                finally
                {
                    clonedMessage.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
