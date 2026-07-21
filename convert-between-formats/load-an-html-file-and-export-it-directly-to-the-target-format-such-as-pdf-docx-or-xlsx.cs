using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            // Input HTML file path
            string inputHtmlPath = "input.html";

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

            // Temporary MHTML file path
            string tempMhtmlPath = "temp.mhtml";

            // Output files
            string outputPdfPath = "output.pdf";
            string outputDocxPath = "output.docx";
            string outputXlsxPath = "output.xlsx";

            // Load HTML as MailMessage with HtmlLoadOptions
            HtmlLoadOptions htmlLoadOptions = new HtmlLoadOptions();
            using (MailMessage mailMessage = MailMessage.Load(inputHtmlPath, htmlLoadOptions))
            {
                // Save to MHTML using default options
                mailMessage.Save(tempMhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);
            }

            // Load the MHTML into Aspose.Words Document
            Document document = new Document(tempMhtmlPath);
            {
                // Export to PDF
                document.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);

                // Export to DOCX
                document.Save(outputDocxPath, Aspose.Words.SaveFormat.Docx);

                // Export to XLSX (as a single sheet workbook)
                document.Save(outputXlsxPath, SaveFormat.Xlsx);
            }

            // Clean up temporary MHTML file
            try
            {
                if (File.Exists(tempMhtmlPath))
                {
                    File.Delete(tempMhtmlPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to delete temporary file: {ex.Message}");
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
