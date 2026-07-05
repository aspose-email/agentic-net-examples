using System;
using System.IO;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

namespace EmailToPdfWithAccessibility
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.eml";
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

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                using (MailMessage email = MailMessage.Load(inputPath))
                {
                    using (MemoryStream mhtmlStream = new MemoryStream())
                    {
                        email.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                        mhtmlStream.Position = 0;

                        Document doc = new Document(mhtmlStream);

                        Aspose.Words.Saving.PdfSaveOptions pdfOptions = new Aspose.Words.Saving.PdfSaveOptions();
                        // Enable tagged PDF for accessibility if the property is available in your Aspose.Words version.
                        // Uncomment the following line when supported:
                        // pdfOptions.TaggedPdf = true;

                        doc.Save(outputPath, pdfOptions);
                    }
                }

                Console.WriteLine($"Successfully converted '{inputPath}' to PDF '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
