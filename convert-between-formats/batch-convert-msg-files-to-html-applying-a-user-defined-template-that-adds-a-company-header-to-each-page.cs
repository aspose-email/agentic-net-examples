using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Merging;

namespace BatchMsgToHtml
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output directories
                string inputDirectory = "InputMsgs";
                string outputDirectory = "OutputHtml";

                // Verify input directory exists
                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Input directory '{inputDirectory}' does not exist.");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory '{outputDirectory}': {ex.Message}");
                        return;
                    }
                }

                // Define the HTML header template to prepend to each converted page
                string headerHtml = "<div style='background:#eee;padding:10px;'><h1>Company Header</h1></div>";

                // Process each MSG file in the input directory
                string[] msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
                foreach (string msgPath in msgFiles)
                {
                    try
                    {
                        string fileBaseName = Path.GetFileNameWithoutExtension(msgPath);
                        string htmlPath = Path.Combine(outputDirectory, fileBaseName + ".html");

                        // Load the MSG file as a MailMessage
                        using (MailMessage message = MailMessage.Load(msgPath, new MsgLoadOptions()))
                        {
                            // Save the message as HTML with embedded resources
                            HtmlSaveOptions htmlOptions = new HtmlSaveOptions
                            {
                                ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                            };
                            message.Save(htmlPath, htmlOptions);
                        }

                        // Read the generated HTML, prepend the header, and overwrite the file
                        string htmlContent = File.ReadAllText(htmlPath);
                        string finalContent = headerHtml + Environment.NewLine + htmlContent;
                        File.WriteAllText(htmlPath, finalContent);
                    }
                    catch (Exception exFile)
                    {
                        Console.Error.WriteLine($"Error processing file '{msgPath}': {exFile.Message}");
                        // Continue with next file
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
