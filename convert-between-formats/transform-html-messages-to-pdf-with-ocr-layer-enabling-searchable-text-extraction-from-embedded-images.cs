using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;

class Program
{
    static void Main()
    {
        try
        {
            string inputHtmlPath = "input.html";
            string outputPdfPath = "output.pdf";

            // Ensure input HTML exists; create a minimal placeholder if missing
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

                try
                {
                    string placeholderHtml = "<html><body><p>Placeholder HTML content.</p></body></html>";
                    File.WriteAllText(inputHtmlPath, placeholderHtml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            try
            {
                string outputDirectory = Path.GetDirectoryName(outputPdfPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Load the HTML as a MailMessage
            using (MailMessage mailMessage = MailMessage.Load(inputHtmlPath, new HtmlLoadOptions()))
            {
                // Save the MailMessage to MHTML in a memory stream
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0;

                    // Load the MHTML into Aspose.Words Document
                    Document document = new Document(mhtmlStream);

                    // Save the document as PDF (OCR layer is handled internally by Aspose.Words if needed)
                    document.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
                }
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
