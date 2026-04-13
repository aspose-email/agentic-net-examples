using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputFolder = "Tasks";
            string outputFolder = "HtmlOutput";

            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Error: Input folder not found – {inputFolder}");
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create output folder – {ex.Message}");
                    return;
                }
            }

            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputFolder, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Unable to enumerate .msg files – {ex.Message}");
                return;
            }

            foreach (string msgPath in msgFiles)
            {
                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Warning: File not found – {msgPath}");
                    continue;
                }

                try
                {
                    using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
                    {
                        // Verify that the message is a task; otherwise create a minimal placeholder HTML.
                        bool isTask = mapiMessage.SupportedType == MapiItemType.Task;

                        string htmlFileName = Path.GetFileNameWithoutExtension(msgPath) + ".html";
                        string htmlFilePath = Path.Combine(outputFolder, htmlFileName);

                        string htmlContent;

                        if (isTask)
                        {
                            // Extract common task fields.
                            string subject = mapiMessage.Subject ?? string.Empty;
                            string body = mapiMessage.Body ?? string.Empty;

                            // Attempt to retrieve task-specific properties if they exist.
                            // Example: TaskStartDate and TaskDueDate are stored as custom properties.
                            // For simplicity, we include only subject and body here.
                            htmlContent = $"<html><head><meta charset=\"UTF-8\"><title>{System.Net.WebUtility.HtmlEncode(subject)}</title></head><body>";
                            htmlContent += $"<h1>{System.Net.WebUtility.HtmlEncode(subject)}</h1>";
                            htmlContent += $"<pre>{System.Net.WebUtility.HtmlEncode(body)}</pre>";
                            htmlContent += "</body></html>";
                        }
                        else
                        {
                            // Placeholder for non‑task MSG files.
                            htmlContent = "<html><head><meta charset=\"UTF-8\"><title>Placeholder</title></head><body>";
                            htmlContent += "<p>Not a task message. Placeholder content generated.</p>";
                            htmlContent += "</body></html>";
                        }

                        try
                        {
                            File.WriteAllText(htmlFilePath, htmlContent);
                            Console.WriteLine($"Converted: {msgPath} -> {htmlFilePath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error: Unable to write HTML file – {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file {msgPath}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
