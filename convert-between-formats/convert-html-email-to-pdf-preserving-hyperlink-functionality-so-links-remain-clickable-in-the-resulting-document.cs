using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

namespace HtmlEmailToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input .eml file containing the HTML email
                string inputPath = "email.eml";
                // Desired output PDF file
                string outputPath = "email.pdf";

                // Verify input file exists; create a placeholder if it does not
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
                        placeholder.Save(inputPath, Aspose.Email.SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    try
                    {
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                        return;
                    }

                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the email message
                using (MailMessage mailMessage = MailMessage.Load(inputPath))
                {
                    // Save the email as MHTML into a memory stream
                    using (MemoryStream mhtmlStream = new MemoryStream())
                    {
                        mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                        mhtmlStream.Position = 0;

                        // Load the MHTML into Aspose.Words Document
                        Document wordsDoc = new Document(mhtmlStream);

                        // Save the document as PDF, preserving hyperlinks
                        Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions
                        {
                            // Ensure hyperlinks are retained (default behavior)
                            PreserveFormFields = true
                        };
                        wordsDoc.Save(outputPath, pdfOptions);
                    }
                }

                Console.WriteLine($"PDF successfully created at: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
