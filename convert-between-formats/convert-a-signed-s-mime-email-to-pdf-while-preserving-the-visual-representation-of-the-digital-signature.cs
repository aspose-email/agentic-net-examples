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
            string inputPath = "signed.eml";
            string outputPath = "signed.pdf";

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

                // Create a placeholder EML file if the input does not exist.

                Console.Error.WriteLine($"Input file not found. Placeholder created at {inputPath}");
                return;
            }

            // Load the email message.
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Convert the email to MHTML and then to PDF via Aspose.Words.
                using (MemoryStream mhtmlStream = new MemoryStream())
                {
                    mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0;

                    Document doc = new Document(mhtmlStream);
                    doc.Save(outputPath, Aspose.Words.SaveFormat.Pdf);
                }

                Console.WriteLine($"PDF saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
