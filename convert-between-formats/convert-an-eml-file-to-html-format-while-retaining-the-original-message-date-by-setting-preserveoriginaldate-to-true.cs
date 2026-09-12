using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Search; // For ResourceRenderingMode enum if needed

class Program
{
    static void Main()
    {
        try
        {
            // Author note: This sample converts an EML file to HTML while attempting to preserve the original message date.
            string inputPath = "input.eml";
            string outputPath = "output.html";

            // Verify input file exists
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the EML message
            using (MailMessage eml = MailMessage.Load(inputPath))
            {
                // Create HTML save options
                MhtSaveOptions htmlOptions = new MhtSaveOptions
                {
                    // Embed resources (images, etc.) into the HTML
                };

                // NOTE: PreserveOriginalDate property is not available on MhtSaveOptions in the documented API.
                // If a future version provides such a property, set it here, e.g.:
                // htmlOptions.PreserveOriginalDate = true;

                // Save as HTML
                eml.Save(outputPath, htmlOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
