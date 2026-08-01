using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailSample
{
    // Author: Aspose.Email .NET sample
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input MSG file path
                string inputPath = "sample.msg";
                // Output EML file path
                string outputPath = "sample.eml";

                // Guard file existence
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
                    try
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                        return;
                    }
                }

                // Load MSG file
                MapiMessage mapMsg;
                try
                {
                    mapMsg = MapiMessage.Load(inputPath);
                }
                catch (Exception loadEx)
                {
                    Console.Error.WriteLine($"Failed to load MSG file: {loadEx.Message}");
                    return;
                }

                // Convert to MailMessage
                MailMessage mailMsg;
                try
                {
                    mailMsg = mapMsg.ToMailMessage(new MailConversionOptions());
                }
                catch (Exception convEx)
                {
                    Console.Error.WriteLine($"Conversion to MailMessage failed: {convEx.Message}");
                    return;
                }

                // Use using to ensure disposal of MailMessage
                using (mailMsg)
                {
                    // Display basic information
                    Console.WriteLine($"Subject: {mailMsg.Subject}");
                    Console.WriteLine($"From: {mailMsg.From}");
                    Console.WriteLine($"To: {mailMsg.To}");
                    Console.WriteLine($"Body (Text): {mailMsg.Body}");

                    // List attachments if any
                    if (mailMsg.Attachments != null && mailMsg.Attachments.Count > 0)
                    {
                        Console.WriteLine("Attachments:");
                        foreach (Attachment attachment in mailMsg.Attachments)
                        {
                            Console.WriteLine($" - {attachment.Name}");
                        }
                    }

                    // Save as EML
                    try
                    {
                        mailMsg.Save(outputPath);
                        Console.WriteLine($"Message saved as EML to: {outputPath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save EML file: {saveEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
