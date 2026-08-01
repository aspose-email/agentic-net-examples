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
            // Paths for input and output MSG files
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the Outlook MSG file
            MapiMessage mapiMessage = MapiMessage.Load(inputPath);

            // Convert to MailMessage for attachment manipulation
            MailConversionOptions conversionOptions = new MailConversionOptions();
            using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
            {
                // Define the attachment file name to remove
                string targetFileName = "remove.txt";

                // Locate the attachment
                Attachment attachmentToRemove = null;
                foreach (Attachment att in mailMessage.Attachments)
                {
                    if (string.Equals(att.Name, targetFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        attachmentToRemove = att;
                        break;
                    }
                }

                // Remove if found
                if (attachmentToRemove != null)
                {
                    mailMessage.Attachments.Remove(attachmentToRemove);
                    Console.WriteLine($"Removed attachment: {targetFileName}");
                }
                else
                {
                    Console.WriteLine($"Attachment not found: {targetFileName}");
                }

                // Save the modified message back to MSG format
                mailMessage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
