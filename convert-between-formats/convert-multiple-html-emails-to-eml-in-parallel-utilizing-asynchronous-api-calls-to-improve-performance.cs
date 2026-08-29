using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;

namespace EmailConversionSample
{
    class Program
    {
        // Author note: This sample converts HTML email files to EML format using asynchronous processing.
        static async Task Main(string[] args)
        {
            try
            {
                string inputDirectory = "InputHtml";
                string outputDirectory = "OutputEml";

                // Ensure input directory exists
                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Input directory '{inputDirectory}' does not exist.");
                    return;
                }

                // Ensure output directory exists or create it
                if (!Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create output directory '{outputDirectory}': {dirEx.Message}");
                        return;
                    }
                }

                // Get all HTML files in the input directory
                string[] htmlFiles;
                try
                {
                    htmlFiles = Directory.GetFiles(inputDirectory, "*.html");
                }
                catch (Exception fileEx)
                {
                    Console.Error.WriteLine($"Error accessing files in '{inputDirectory}': {fileEx.Message}");
                    return;
                }

                if (htmlFiles.Length == 0)
                {
                    Console.WriteLine("No HTML files found to process.");
                    return;
                }

                List<Task> conversionTasks = new List<Task>();

                foreach (string htmlFilePath in htmlFiles)
                {
                    // Process each file in its own task
                    Task conversionTask = Task.Run(() =>
                    {
                        try
                        {
                            // Guard against missing file (should not happen as we enumerated them)
                            if (!File.Exists(htmlFilePath))
                            {
                                Console.Error.WriteLine($"File not found: {htmlFilePath}");
                                return;
                            }

                            string htmlContent;
                            try
                            {
                                htmlContent = File.ReadAllText(htmlFilePath);
                            }
                            catch (Exception readEx)
                            {
                                Console.Error.WriteLine($"Failed to read '{htmlFilePath}': {readEx.Message}");
                                return;
                            }

                            // Create a new MailMessage and set basic properties
                            using (MailMessage message = new MailMessage())
                            {
                                // Placeholder addresses – adjust as needed
                                message.From = new MailAddress("sender@example.com");
                                message.To.Add(new MailAddress("recipient@example.com"));
                                message.HtmlBody = htmlContent;
                                message.Subject = Path.GetFileNameWithoutExtension(htmlFilePath);

                                string outputFilePath = Path.Combine(outputDirectory,
                                    Path.GetFileNameWithoutExtension(htmlFilePath) + ".eml");

                                try
                                {
                                    message.Save(outputFilePath);
                                    Console.WriteLine($"Converted '{htmlFilePath}' to '{outputFilePath}'.");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save EML for '{htmlFilePath}': {saveEx.Message}");
                                }
                            }
                        }
                        catch (Exception taskEx)
                        {
                            Console.Error.WriteLine($"Unexpected error processing '{htmlFilePath}': {taskEx.Message}");
                        }
                    });

                    conversionTasks.Add(conversionTask);
                }

                // Await all conversion tasks
                await Task.WhenAll(conversionTasks);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
