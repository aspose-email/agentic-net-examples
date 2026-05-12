using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input MIME files to merge
            string[] inputFiles = new string[] { "msg1.eml", "msg2.eml", "msg3.eml" };
            string outputFile = "merged.eml";

            // Ensure input files exist; create minimal placeholders if missing
            foreach (string inputPath in inputFiles)
            {
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
                        using (MailMessage placeholder = new MailMessage("placeholder@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message."))
                        {
                            placeholder.Save(inputPath);
                            Console.WriteLine($"Created placeholder for missing file: {inputPath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder for '{inputPath}': {ex.Message}");
                        return;
                    }
                }
            }

            // Create the container message
            using (MailMessage container = new MailMessage())
            {
                container.From = "sender@example.com";
                container.To.Add("recipient@example.com");
                container.Subject = "Combined MIME Messages";
                container.Body = "Please find the combined messages attached.";

                // Load each message and attach it as a message/rfc822 part
                foreach (string inputPath in inputFiles)
                {
                    try
                    {
                        using (MailMessage partMessage = MailMessage.Load(inputPath))
                        {
                            using (MemoryStream partStream = new MemoryStream())
                            {
                                partMessage.Save(partStream);
                                partStream.Position = 0;
                                string attachmentName = Path.GetFileName(inputPath);
                                Attachment attachment = new Attachment(partStream, attachmentName, "message/rfc822");
                                container.Attachments.Add(attachment);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process '{inputPath}': {ex.Message}");
                        return;
                    }
                }

                // Save the combined message
                try
                {
                    container.Save(outputFile);
                    Console.WriteLine($"Combined message saved to '{outputFile}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save combined message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
