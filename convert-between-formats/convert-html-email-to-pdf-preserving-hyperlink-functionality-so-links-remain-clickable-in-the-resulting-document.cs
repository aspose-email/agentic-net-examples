using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            string inputHtmlPath = "email.html";
            string outputPdfPath = "output.pdf";

            // Verify input file exists
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
                        placeholder.Save(inputHtmlPath, Aspose.Email.SaveOptions.DefaultEml);
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

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPdfPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the HTML email into a MailMessage
            using (MailMessage mailMessage = MailMessage.Load(inputHtmlPath, new HtmlLoadOptions()))
            {
                // Save the MailMessage as MHTML into a memory stream
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0;

                    // Load the MHTML into an Aspose.Words Document
                    Document doc = new Document(mhtmlStream);

                    // Save the document as PDF, preserving hyperlinks
                    doc.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
                }
            }

            Console.WriteLine($"Conversion completed successfully. PDF saved to: {outputPdfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
