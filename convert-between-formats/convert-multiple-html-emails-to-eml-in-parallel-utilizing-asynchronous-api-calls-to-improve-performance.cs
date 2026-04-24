using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;

class Program
{
    static async Task Main()
    {
        try
        {
            // Input HTML email files
            List<string> inputFiles = new List<string>
            {
                "email1.html",
                "email2.html",
                "email3.html"
            };

            // Output directory for EML files
            string outputDir = "ConvertedEml";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Prepare conversion tasks
            List<Task> conversionTasks = new List<Task>();

            foreach (string htmlPath in inputFiles)
            {
                // Guard missing input file
                if (!File.Exists(htmlPath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(htmlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Input file not found: {htmlPath}");
                    continue;
                }

                // Create a task for each conversion
                Task conversionTask = Task.Run(() =>
                {
                    try
                    {
                        // Load HTML email
                        using (MailMessage message = MailMessage.Load(htmlPath))
                        {
                            // Determine output EML path
                            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(htmlPath);
                            string emlPath = Path.Combine(outputDir, fileNameWithoutExt + ".eml");

                            // Save as EML using SaveOptions
                            EmlSaveOptions emlOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);
                            message.Save(emlPath, emlOptions);
                            Console.WriteLine($"Converted '{htmlPath}' to '{emlPath}'.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing '{htmlPath}': {ex.Message}");
                    }
                });

                conversionTasks.Add(conversionTask);
            }

            // Await all conversions
            await Task.WhenAll(conversionTasks);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
