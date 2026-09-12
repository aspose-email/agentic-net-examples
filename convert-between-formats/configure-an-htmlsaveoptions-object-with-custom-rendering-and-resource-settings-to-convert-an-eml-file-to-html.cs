using System;
using System.IO;
using Aspose.Email;

namespace EmailConversionExample
{
    // Author: Aspose.Email .NET sample
    class Program
    {
        static void Main()
        {
            try
            {
                // Input and output file paths
                string inputEmlPath = "input.eml";
                string outputHtmlPath = "output.html";

                // Verify that the input EML file exists
                if (!File.Exists(inputEmlPath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputEmlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Input file not found: {inputEmlPath}");
                    return;
                }

                // Load the EML message
                using (MailMessage mailMessage = MailMessage.Load(inputEmlPath))
                {
                    // Configure HTML save options with custom resource rendering
                    HtmlSaveOptions htmlOptions = new HtmlSaveOptions();
                    htmlOptions.ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml;

                    // Save the message as HTML using the configured options
                    mailMessage.Save(outputHtmlPath, htmlOptions);
                }

                Console.WriteLine($"Successfully converted '{inputEmlPath}' to HTML at '{outputHtmlPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
