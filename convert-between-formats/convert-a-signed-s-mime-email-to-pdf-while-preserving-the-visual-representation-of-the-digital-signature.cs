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
            string inputEmlPath = "signed.eml";
            string tempMhtmlPath = "temp.mhtml";
            string outputPdfPath = "signed.pdf";

            // Verify input file exists
            if (!File.Exists(inputEmlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputEmlPath, Aspose.Email.SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputEmlPath}");
                return;
            }

            // Load the signed S/MIME email
            using (MailMessage mailMessage = MailMessage.Load(inputEmlPath))
            {
                // Save the email as MHTML to preserve visual layout (including signature)
                mailMessage.Save(tempMhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);
            }

            // Convert MHTML to PDF using Aspose.Words
            Document document = new Document(tempMhtmlPath);
            {
                document.Save(outputPdfPath, Aspose.Words.SaveFormat.Pdf);
            }

            // Clean up temporary MHTML file
            try
            {
                if (File.Exists(tempMhtmlPath))
                {
                    File.Delete(tempMhtmlPath);
                }
            }
            catch (Exception cleanupEx)
            {
                Console.Error.WriteLine($"Failed to delete temporary file: {cleanupEx.Message}");
            }

            Console.WriteLine($"Conversion completed successfully. PDF saved to: {outputPdfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
