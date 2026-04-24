using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputDirectory = "InputMsgs";
            string outputDirectory = "OutputHtml";
            string headerHtml = "<div style=\"font-weight:bold; font-size:24px;\">Company Header</div>";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate MSG files: {ex.Message}");
                return;
            }

            foreach (string msgFilePath in msgFiles)
            {
                if (!File.Exists(msgFilePath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found, skipping: {msgFilePath}");
                    continue;
                }

                try
                {
                    using (MailMessage mailMessage = MailMessage.Load(msgFilePath))
                    {
                        HtmlSaveOptions htmlOptions = new HtmlSaveOptions
                        {
                            ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml,
                            MailMessageSaveType = MailMessageSaveType.HtmlFormat
                        };

                        string tempHtmlPath = Path.Combine(outputDirectory,
                            Path.GetFileNameWithoutExtension(msgFilePath) + ".html");

                        // Save to temporary HTML file
                        mailMessage.Save(tempHtmlPath, htmlOptions);

                        // Read generated HTML, prepend header, and overwrite file
                        string htmlContent;
                        try
                        {
                            htmlContent = File.ReadAllText(tempHtmlPath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to read generated HTML: {ex.Message}");
                            continue;
                        }

                        string finalHtml = headerHtml + Environment.NewLine + htmlContent;

                        try
                        {
                            File.WriteAllText(tempHtmlPath, finalHtml);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to write final HTML: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{msgFilePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
