using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source email file (EML or MSG)
            string inputFilePath = "input.eml";

            // Verify that the input file exists before attempting to read it
            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            // Open a file stream for reading the email
            using (FileStream fileStream = File.OpenRead(inputFilePath))
            {
                // Load the email message from the stream
                using (MailMessage message = MailMessage.Load(fileStream))
                {
                    // Prepare HTML save options (embed resources into the HTML)
                    HtmlSaveOptions htmlOptions = new HtmlSaveOptions
                    {
                        ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                    };

                    // Memory buffer to hold the resulting HTML
                    using (MemoryStream htmlBuffer = new MemoryStream())
                    {
                        // Save the message as HTML into the memory buffer
                        message.Save(htmlBuffer, htmlOptions);

                        // Optionally, retrieve the HTML bytes for further processing
                        byte[] htmlBytes = htmlBuffer.ToArray();

                        Console.WriteLine($"Email successfully converted to HTML. Output size: {htmlBytes.Length} bytes.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
