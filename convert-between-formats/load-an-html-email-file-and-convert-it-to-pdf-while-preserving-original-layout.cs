using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Words;

class Program
{
    static void Main()
    {
        try
        {
            // Input HTML email file path
            string inputPath = "email.html";

            // Verify input file exists; create a placeholder if missing
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

            // Output PDF file path
            string outputPath = "email.pdf";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the HTML email
            HtmlLoadOptions loadOptions = new HtmlLoadOptions();
            using (MailMessage message = MailMessage.Load(inputPath, loadOptions))
            {
                // Convert the email to MHTML in memory
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    message.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0;

                    // Load MHTML into Aspose.Words and save as PDF
                    Document doc = new Document(mhtmlStream);
                    doc.Save(outputPath, Aspose.Words.SaveFormat.Pdf);
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
