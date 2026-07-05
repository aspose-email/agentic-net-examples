using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Tables;

class Program
{
    static void Main()
    {
        // Author note: This example converts an HTML email to PDF with a custom footer (page numbers and generation date).

        string inputHtmlPath = "email.html";
        string tempMhtmlPath = "temp.mhtml";
        string outputPdfPath = "email.pdf";

        // Ensure the input HTML file exists; create a minimal placeholder if missing.
        try
        {
            if (!File.Exists(inputHtmlPath))
            {
                string placeholderHtml = "<html><body><h1>Sample Email</h1><p>This is a placeholder email body.</p></body></html>";
                File.WriteAllText(inputHtmlPath, placeholderHtml);
                Console.WriteLine($"Created placeholder HTML file at '{inputHtmlPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to prepare input HTML file: {ex.Message}");
            return;
        }

        // Load HTML content and save it as MHTML using Aspose.Email.
        try
        {
            string htmlContent = File.ReadAllText(inputHtmlPath);

            using (MailMessage message = new MailMessage())
            {
                // Dummy addresses to satisfy MailMessage requirements.
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Converted Email";
                message.HtmlBody = htmlContent;

                // Save as MHTML (Mht) format.
                message.Save(tempMhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during HTML to MHTML conversion: {ex.Message}");
            return;
        }

        // Load the MHTML into Aspose.Words Document.
        Document doc;
        try
        {
            doc = new Document(tempMhtmlPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load MHTML into Aspose.Words: {ex.Message}");
            return;
        }

        // Add a footer with page numbers and generation date.
        try
        {
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToHeaderFooter(HeaderFooterType.FooterPrimary);
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;

            builder.Write("Page ");
            builder.InsertField("PAGE", "");
            builder.Write(" of ");
            builder.InsertField("NUMPAGES", "");
            builder.Write(" - Generated on ");
            builder.Write(DateTime.Now.ToString("yyyy-MM-dd"));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to add footer: {ex.Message}");
            // Continue to attempt saving without custom footer.
        }

        // Save the final PDF.
        try
        {
            doc.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
            Console.WriteLine($"PDF successfully created at '{outputPdfPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error saving PDF: {ex.Message}");
        }
        finally
        {
            // Clean up temporary MHTML file.
            try
            {
                if (File.Exists(tempMhtmlPath))
                {
                    File.Delete(tempMhtmlPath);
                }
            }
            catch
            {
                // Suppress any cleanup errors.
            }
        }
    }
}
