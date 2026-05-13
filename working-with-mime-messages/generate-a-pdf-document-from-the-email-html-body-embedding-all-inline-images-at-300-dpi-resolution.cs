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
            string inputPath = "email.eml";
            string outputPath = "output.pdf";

            // Ensure the input file exists; create a placeholder if it does not.
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

            // Ensure the output directory exists.
            try
            {
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Load the email message.
            MailMessage emailMessage;
            try
            {
                emailMessage = MailMessage.Load(inputPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load email: {loadEx.Message}");
                return;
            }

            // Convert the email to MHTML (preserves inline images).
            using (MemoryStream mhtmlStream = new MemoryStream())
            {
                try
                {
                    emailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                    mhtmlStream.Position = 0;
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to convert email to MHTML: {saveEx.Message}");
                    return;
                }

                // Load the MHTML into an Aspose.Words Document.
                Document doc;
                try
                {
                    doc = new Document(mhtmlStream);
                }
                catch (Exception docEx)
                {
                    Console.Error.WriteLine($"Failed to load MHTML into Document: {docEx.Message}");
                    return;
                }

                // Configure PDF save options.
                Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions();
                // Note: If the used Aspose.Words version supports ImageResolution,
                // you can uncomment the following line to set 300 DPI.
                // pdfOptions.ImageResolution = 300;

                // Save the document as PDF.
                try
                {
                    doc.Save(outputPath, pdfOptions);
                    Console.WriteLine($"PDF generated successfully at: {outputPath}");
                }
                catch (Exception pdfEx)
                {
                    Console.Error.WriteLine($"Failed to save PDF: {pdfEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
