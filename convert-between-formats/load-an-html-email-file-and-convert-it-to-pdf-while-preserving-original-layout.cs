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

            // Load the HTML email into a MailMessage
            HtmlLoadOptions loadOptions = new HtmlLoadOptions();
            using (MailMessage email = MailMessage.Load(inputHtmlPath, loadOptions))
            {
                // Save the MailMessage to MHTML in a memory stream
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    email.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0; // Reset stream position for reading

                    // Load the MHTML into Aspose.Words Document
                    Document doc = new Document(mhtmlStream);
            {
                        // Save the document as PDF
                        doc.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
                    }
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
