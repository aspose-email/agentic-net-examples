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
            string inputPath = "input.html";
            string outputPath = "output.pdf";

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

            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load HTML content into a MailMessage
            HtmlLoadOptions htmlLoadOptions = new HtmlLoadOptions();
            MailMessage mailMessage = MailMessage.Load(inputPath, htmlLoadOptions);

            // Configure MHTML save options to scale images
            MhtSaveOptions mhtSaveOptions = new MhtSaveOptions();
            mhtSaveOptions.CssStyles = "img{max-width:100%;height:auto;}";

            using (MemoryStream mhtmlStream = new MemoryStream())
            {
                // Save the MailMessage as MHTML with the scaling CSS
                mailMessage.Save(mhtmlStream, mhtSaveOptions);
                mhtmlStream.Position = 0;

                // Load the MHTML into Aspose.Words Document
                Document document = new Document(mhtmlStream);

                // Save the document as PDF
                document.Save(outputPath, Aspose.Words.SaveFormat.Pdf);
            }

            Console.WriteLine($"PDF successfully saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
