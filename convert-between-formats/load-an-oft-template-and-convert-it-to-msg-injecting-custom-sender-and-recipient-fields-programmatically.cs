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
            string templatePath = "template.oft";
            string outputPath = "output.msg";

            // Ensure the directory for the output exists
            try
            {
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Guard input file existence; create a minimal placeholder if missing
            if (!File.Exists(templatePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder OFT";
                        placeholder.Body = "This is a placeholder Outlook template.";
                        placeholder.SaveAsTemplate(templatePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder OFT: {ex.Message}");
                    return;
                }
            }

            // Load the OFT template
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(templatePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load OFT template: {ex.Message}");
                return;
            }

            using (message)
            {
                // Inject custom sender information
                message.SenderName = "John Doe";
                message.SenderEmailAddress = "john.doe@example.com";
                message.SenderSmtpAddress = "john.doe@example.com";

                // Clear existing recipients and add a custom recipient
                message.Recipients.Clear();
                message.Recipients.Add("jane.smith@example.com", "Jane Smith", MapiRecipientType.MAPI_TO);

                // Save as MSG
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
