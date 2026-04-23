using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Tables;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "input.html";
            string outputPath = "output.pdf";

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
                        placeholder.Save(inputPath, Aspose.Email.SaveOptions.DefaultEml);
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

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the HTML email
            using (MailMessage email = MailMessage.Load(inputPath))
            {
                // Convert to MHTML in memory
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    email.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0;

                    // Load MHTML into Aspose.Words document
                    Document doc = new Document(mhtmlStream);
            {
                        // Create a footer with page numbers and generation date
                        DocumentBuilder builder = new DocumentBuilder(doc);
                        builder.MoveToHeaderFooter(HeaderFooterType.FooterPrimary);
                        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;

                        builder.Write("Page ");
                        builder.InsertField("PAGE", "");
                        builder.Write(" of ");
                        builder.InsertField("NUMPAGES", "");
                        builder.Write(" | Generated on ");
                        builder.InsertField(@"DATE \@ ""yyyy-MM-dd HH:mm:ss""", "");

                        // Save the document as PDF
                        doc.Save(outputPath, Aspose.Words.SaveFormat.Pdf);
                    }
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
