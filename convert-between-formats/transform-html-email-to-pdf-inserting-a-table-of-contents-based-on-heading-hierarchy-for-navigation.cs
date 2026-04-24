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

            // Ensure input file exists; create a minimal placeholder if missing
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

                Directory.CreateDirectory(Path.GetDirectoryName(inputHtmlPath) ?? ".");
                File.WriteAllText(inputHtmlPath, "<html><body><h1>Sample Title</h1><p>Content.</p></body></html>");
                Console.Error.WriteLine($"Input file not found. Created placeholder at '{inputHtmlPath}'.");
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPdfPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the HTML email using Aspose.Email with explicit HtmlLoadOptions
            Aspose.Email.HtmlLoadOptions htmlLoadOptions = new Aspose.Email.HtmlLoadOptions();
            using (MailMessage email = MailMessage.Load(inputHtmlPath, htmlLoadOptions))
            {
                // Save the email to MHTML in a memory stream
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    email.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0; // Reset stream position for reading

                    // Load the MHTML into Aspose.Words Document
                    Document doc = new Document(mhtmlStream);

                    // Insert a Table of Contents at the beginning of the document
                    DocumentBuilder builder = new DocumentBuilder(doc);
                    builder.MoveToDocumentStart();
                    builder.InsertTableOfContents("\\o \"1-3\" \\h \\z \\u");

                    // Save the final document as PDF
                    doc.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
                }
            }

            Console.WriteLine($"Conversion completed. PDF saved to '{outputPdfPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
