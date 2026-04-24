using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Input folder containing .eml files
            string inputFolder = "Emails";
            // Output folder for generated PDFs
            string outputFolder = "PdfSummaries";

            // Guard input folder existence
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                return;
            }

            // Ensure output folder exists
            try
            {
                Directory.CreateDirectory(outputFolder);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output folder '{outputFolder}': {ex.Message}");
                return;
            }

            // Gather .eml files
            string[] emlFiles;
            try
            {
                emlFiles = Directory.GetFiles(inputFolder, "*.eml");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing files in '{inputFolder}': {ex.Message}");
                return;
            }

            if (emlFiles.Length == 0)
            {
                Console.Error.WriteLine("No .eml files found to process.");
                return;
            }

            // Process each email asynchronously
            List<Task> tasks = new List<Task>();
            foreach (string emlPath in emlFiles)
            {
                tasks.Add(ProcessEmailAsync(emlPath, outputFolder));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("PDF summaries generated successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Asynchronously converts a single .eml file to PDF
    private static async Task ProcessEmailAsync(string emlPath, string outputFolder)
    {
        // Verify the .eml file exists
        if (!File.Exists(emlPath))
        {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, Aspose.Email.SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

            Console.Error.WriteLine($"File not found: {emlPath}");
            return;
        }

        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(emlPath);
        string mhtmlPath = Path.Combine(outputFolder, fileNameWithoutExt + ".mhtml");
        string pdfPath = Path.Combine(outputFolder, fileNameWithoutExt + ".pdf");

        try
        {
            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Save as MHTML (intermediate format)
                mailMessage.Save(mhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);
            }

            // Load MHTML into Aspose.Words Document
            Document doc = new Document(mhtmlPath);

            // Save Document as PDF
            doc.Save(pdfPath, Aspose.Words.SaveFormat.Pdf);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to process '{emlPath}': {ex.Message}");
            return;
        }
        finally
        {
            // Clean up intermediate MHTML file
            try
            {
                if (File.Exists(mhtmlPath))
                {
                    File.Delete(mhtmlPath);
                }
            }
            catch
            {
                // Suppress any cleanup errors
            }
        }

        // Simulate asynchronous operation
        await Task.Yield();
    }
}
