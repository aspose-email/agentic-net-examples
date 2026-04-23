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
            string inputPath = "message.eml";
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

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            string tempMhtmlPath = "temp.mhtml";
            string outputPdfPath = "output.pdf";

            // Load the multi‑part MIME message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Save the message (including attachments) as MHTML
                message.Save(tempMhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);
            }

            // Convert the MHTML to PDF using Aspose.Words
            Document doc = new Document(tempMhtmlPath);
            {
                doc.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
            }

            // Clean up the temporary MHTML file
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

            Console.WriteLine($"PDF created at '{outputPdfPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
