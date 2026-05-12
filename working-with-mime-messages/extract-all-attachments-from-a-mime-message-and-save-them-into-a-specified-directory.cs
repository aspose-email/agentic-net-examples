using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MIME (EML) file
            string emlPath = "message.eml";
            // Directory where attachments will be saved
            string outputDirectory = "Attachments";

            // Verify input file exists
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

                Console.Error.WriteLine($"Input file not found: {emlPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MIME message
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                // Iterate through each attachment
                foreach (Attachment attachment in message.Attachments)
                {
                    string outputPath = Path.Combine(outputDirectory, attachment.Name);
                    try
                    {
                        // Save the attachment to the specified directory
                        attachment.Save(outputPath);
                        Console.WriteLine($"Saved attachment: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.Name}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
