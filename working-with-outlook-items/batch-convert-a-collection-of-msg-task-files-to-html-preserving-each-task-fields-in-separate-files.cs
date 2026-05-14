using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input folder containing MSG task files
            string inputFolder = "Tasks";
            // Output folder for generated HTML files
            string outputFolder = "HtmlOutput";

            // Ensure input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Ensure output folder exists or create it
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output folder: {ex.Message}");
                return;
            }

            // Gather all .msg files in the input folder
            List<string> msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputFolder, "*.msg").ToList();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate MSG files: {ex.Message}");
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

                    Console.Error.WriteLine($"File not found, skipping: {msgPath}");
                    continue;
                }

                try
                {
                    // Load the MSG file
                    using (MapiMessage msg = MapiMessage.Load(msgPath))
                    {
                        // Verify that the MSG represents a Task
                        if (msg.SupportedType != MapiItemType.Task)
                        {
                            Console.Error.WriteLine($"File is not a Task, skipping: {msgPath}");
                            continue;
                        }

                        // Convert to MapiTask
                        MapiTask task = (MapiTask)msg.ToMapiMessageItem();

                        // Build simple HTML representation of the task fields
                        string title = WebUtility.HtmlEncode(task.Subject ?? "Task");
                        string htmlContent = "<!DOCTYPE html>\n<html>\n<head>\n<meta charset=\"UTF-8\">\n<title>"
                                             + title
                                             + "</title>\n</head>\n<body>\n<h1>"
                                             + title
                                             + "</h1>\n<ul>\n";

                        htmlContent += "<li><strong>Start Date:</strong> " + (task.StartDate != DateTime.MinValue ? task.StartDate.ToString("u") : "N/A") + "</li>\n";
                        htmlContent += "<li><strong>Due Date:</strong> " + (task.DueDate != DateTime.MinValue ? task.DueDate.ToString("u") : "N/A") + "</li>\n";
                        htmlContent += "<li><strong>Status:</strong> " + WebUtility.HtmlEncode(task.Status.ToString()) + "</li>\n";
                        htmlContent += "<li><strong>Percent Complete:</strong> " + task.PercentComplete + "%</li>\n";
                        htmlContent += "<li><strong>Priority:</strong> " + WebUtility.HtmlEncode(task.Priority.ToString()) + "</li>\n";

                        string categories = (task.Categories != null && task.Categories.Length > 0)
                                            ? string.Join(", ", task.Categories)
                                            : "N/A";
                        htmlContent += "<li><strong>Categories:</strong> " + WebUtility.HtmlEncode(categories) + "</li>\n";

                        htmlContent += "</ul>\n";

                        string body = task.Body ?? string.Empty;
                        htmlContent += "<h2>Body</h2>\n<div>" + WebUtility.HtmlEncode(body) + "</div>\n";

                        htmlContent += "</body>\n</html>";

                        // Create a safe file name for the HTML file
                        string safeFileName = string.IsNullOrWhiteSpace(task.Subject) ? "Task" : task.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeFileName = safeFileName.Replace(c, '_');
                        }
                        string htmlPath = Path.Combine(outputFolder, safeFileName + ".html");

                        // Write HTML to file
                        try
                        {
                            File.WriteAllText(htmlPath, htmlContent);
                            Console.WriteLine($"Converted '{msgPath}' to '{htmlPath}'.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to write HTML file '{htmlPath}': {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{msgPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
