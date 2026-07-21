using System;
using System.IO;
using Aspose.Email;

namespace EmailConversionSample
{
    // Author: Generated example for loading HTML and saving as EML with preserved structure
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input HTML file and output EML file paths
                string inputHtmlPath = "email.html";
                string outputEmlPath = "output.eml";

                // Verify that the input HTML file exists
                if (!File.Exists(inputHtmlPath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputHtmlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Input file not found: {inputHtmlPath}");
                    return;
                }

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputEmlPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the HTML file as a MailMessage using HtmlLoadOptions
                HtmlLoadOptions htmlLoadOptions = new HtmlLoadOptions();
                using (MailMessage mailMessage = MailMessage.Load(inputHtmlPath, htmlLoadOptions))
                {
                    // Configure EML save options to preserve embedded message format
                    EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                    {
                        PreserveEmbeddedMessageFormat = true
                    };

                    // Save the MailMessage as an EML file
                    mailMessage.Save(outputEmlPath, emlSaveOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
